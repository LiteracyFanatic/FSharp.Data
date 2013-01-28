﻿namespace FSharp.Data.RuntimeImplementation.WorldBank

open System
open System.Collections
open System.Diagnostics
open System.Globalization
open System.Net
open FSharp.Data.Json
open FSharp.Data.Json.Extensions
open FSharp.Data.RuntimeImplementation.Caching
open FSharp.Net

[<AutoOpen>]
module Implementation = 

    let retryCount = 5
    let parallelIndicatorPageDownloads = 8

    type internal IndicatorRecord = 
        { Id : string
          Name: string
          TopicIds : string list
          Source : string
          Description : string }

    type internal CountryRecord = 
        { Id : string
          Name : string
          CapitalCity : string
          Region : string }
        member x.IsRegion = x.Region = "Aggregates"

    type internal TopicRecord = 
        { Id : string
          Name : string
          Description : string }

    type internal ServiceConnection(restCache:ICache<_>,serviceUrl:string, sources) =

        let worldBankUrl (functions: string list) (props: (string * string) list) = 
            seq { yield serviceUrl
                  for item in functions do
                      yield "/" + Uri.EscapeUriString(item)
                  yield "?per_page=1000"
                  for key, value in props do
                      yield "&" + key + "=" + Uri.EscapeUriString(value:string) 
                  yield "&format=json" } 
            |> String.concat ""

        // The WorldBank data changes very slowly indeed (monthly updates to values, rare updates to schema), hence caching it is ok.

        let rec worldBankRequest attempt funcs args : Async<string> = 
            async { 
                let url = worldBankUrl funcs args
                match restCache.TryRetrieve(url) with
                | Some res -> return res
                | None -> 
                    Debug.WriteLine (sprintf "[WorldBank] downloading (%d): %s" attempt url)
                    try
                        let! doc = Http.AsyncRequest(url)
                        Debug.WriteLine (sprintf "[WorldBank] got text: %s" (if doc = null then "null" elif doc.Length > 50 then doc.[0..49] + "..." else doc))
                        if not (String.IsNullOrEmpty doc) then 
                            restCache.Set(url, doc)
                        return doc 
                    with e ->
                        Debug.WriteLine (sprintf "[WorldBank] error: %s" (e.ToString()))
                        if attempt > 0 then
                            return! worldBankRequest (attempt - 1) funcs args
                        else return! failwithf "failed to request '%s'" url }

        let rec getDocuments funcs args page parallelPages = 
            async { let! docs = 
                        Async.Parallel 
                            [ for i in 0 .. parallelPages - 1 -> 
                                  worldBankRequest retryCount funcs (args@["page", string (page+i)]) ]
                    let docs = docs |> Array.map JsonValue.Parse
                    Debug.WriteLine (sprintf "[WorldBank] geting page count")
                    let pages = docs.[0].[0]?pages.AsInteger
                    Debug.WriteLine (sprintf "[WorldBank] got page count = %d" pages)
                    if (pages < page + parallelPages) then 
                        return Array.toList docs
                    else 
                        let! rest = getDocuments funcs args (page + parallelPages) (pages - parallelPages)
                        return Array.toList docs @ rest }

        let getIndicators() = 
            // Get the indicators in parallel, initially using 'parallelIndicatorPageDownloads' pages
            async { let! docs = getDocuments ["indicator"] [] 1 parallelIndicatorPageDownloads
                    return 
                        [ for doc in docs do
                            for ind in doc.[1] do
                                let id = ind?id.AsString
                                let name = (ind?name.AsString).Trim([|'"'|]).Trim()
                                let sourceName = ind?source?value.AsString
                                if sources |> List.exists (fun source -> String.Compare(source, sourceName, StringComparison.OrdinalIgnoreCase) = 0) then 
                                    let topicIds = Seq.toList <| seq {
                                        for item in ind?topics do
                                            yield item?id.AsString
                                    }
                                    let sourceNote = ind?sourceNote.AsString
                                    yield { Id = id
                                            Name = name
                                            TopicIds = topicIds
                                            Source = sourceName
                                            Description = sourceNote} ] }

        let getTopics() = 
            async { let! docs = getDocuments ["topic"] [] 1 1
                    return 
                        [ for doc in docs do
                            for topic in doc.[1] do
                                let id = topic?id.AsString
                                let name = topic?value.AsString
                                let sourceNote = topic?sourceNote.AsString
                                yield { Id = id
                                        Name = name
                                        Description = sourceNote } ] }

        let getCountries(args) = 
            async { let! docs = getDocuments ["country"] args 1 1
                    return 
                        [ for doc in docs do
                            for country in doc.[1] do
                                let region = country?region?value.AsString
                                yield { Id = country?id.AsString
                                        Name = country?name.AsString
                                        CapitalCity = country?capitalCity.AsString
                                        Region = region } ] }

        let getRegions() = 
            async { let! docs = getDocuments ["region"] [] 1 1
                    return 
                        [ for doc in docs do
                            for ind in doc.[1] do
                                yield ind?code.AsString,
                                      ind?name.AsString ] }

        let getData funcs args key = 
            async { let! docs = getDocuments funcs args 1 1
                    return
                        [ for doc in docs do
                            for ind in doc.[1] do
                                yield ind |> JsonValue.getProperty key |> JsonValue.asString,
                                      ind?value.AsString ] }

        /// At compile time, download the schema
        let topics = lazy (getTopics() |> Async.RunSynchronously)
        let topicsIndexed = lazy (topics.Force() |> Seq.map (fun t -> t.Id, t) |> dict)
        let indicators = lazy (getIndicators() |> Async.RunSynchronously)
        let indicatorsIndexed = lazy (indicators.Force() |> Seq.map (fun i -> i.Id, i) |> dict)
        let indicatorsByTopic = lazy (
            indicators.Force() 
            |> Seq.collect (fun i -> i.TopicIds |> Seq.map (fun topicId -> topicId, i.Id)) 
            |> Seq.groupBy fst
            |> Seq.map (fun (topicId, indicatorIds) -> topicId, indicatorIds |> Seq.map snd |> Seq.cache)
            |> dict)
        let countries = lazy (getCountries [] |> Async.RunSynchronously)
        let countriesIndexed = lazy (countries.Force() |> Seq.map (fun c -> c.Id, c) |> dict)
        let regions = lazy (getRegions() |> Async.RunSynchronously)
        let regionsIndexed = lazy (regions.Force() |> dict)

        member internal __.Topics = topics.Force()
        member internal __.TopicsIndexed = topicsIndexed.Force()
        member internal __.Indicators = indicators.Force()
        member internal __.IndicatorsIndexed = indicatorsIndexed.Force()
        member internal __.IndicatorsByTopic = indicatorsByTopic.Force()
        member internal __.Countries = countries.Force()
        member internal __.CountriesIndexed = countriesIndexed.Force()
        member internal __.Regions = regions.Force()
        member internal __.RegionsIndexed = regionsIndexed.Force()
        /// At runtime, download the data
        member internal __.GetDataAsync(countryOrRegionCode, indicatorCode) = 
            async { let! data = 
                      getData
                        [ yield "countries"
                          yield countryOrRegionCode
                          yield "indicators";
                          yield indicatorCode ]
                        [ "date", "1900:2050" ]
                        "date"
                    return 
                      seq { for (k, v) in data do
                              if not (String.IsNullOrEmpty v) then 
                                 yield int k, float v } 
                      // It's a time series - sort it :-)  We should probably also interpolate (e.g. see R time series library)
                      |> Seq.sortBy fst } 

        member internal x.GetData(countryOrRegionCode, indicatorCode) = 
             x.GetDataAsync(countryOrRegionCode, indicatorCode) |> Async.RunSynchronously
        member internal __.GetCountriesInRegion region = getCountries ["region", region] |> Async.RunSynchronously
  
[<DebuggerDisplay("{Name}")>]
[<StructuredFormatDisplay("{Name}")>]
type Indicator internal (connection:ServiceConnection, countryOrRegionCode:string, indicatorCode:string) = 
    let data = connection.GetData(countryOrRegionCode, indicatorCode) |> Seq.cache
    let dataDict = lazy (dict data)
    /// Get the code for the country or region of the indicator
    member x.Code = countryOrRegionCode
    /// Get the code for the indicator
    member x.IndicatorCode = indicatorCode
    /// Get the name of the indicator
    member x.Name = connection.IndicatorsIndexed.[indicatorCode].Name
    /// Get the source of the indicator
    member x.Source = connection.IndicatorsIndexed.[indicatorCode].Source
    /// Get the description of the indicator
    member x.Description = connection.IndicatorsIndexed.[indicatorCode].Description
    /// Get a value for a year for the indicator
    member x.Item with get idx = dataDict.Force().[idx]
    /// Get the years for which the indicator has values
    member x.Years = dataDict.Force().Keys
    /// Get the values for the indicator (without years)
    member x.Values = dataDict.Force().Values
    interface seq<int * float> with member x.GetEnumerator() = data.GetEnumerator()
    interface IEnumerable with member x.GetEnumerator() = (data.GetEnumerator() :> _)
    member x.GetValueAtOrZero(time:int) = 
        x |> Seq.tryPick (fun (x,y) -> if time = x then Some y else None)
          |> function None -> 0.0 | Some x -> x

[<DebuggerDisplay("{Name}")>]
[<StructuredFormatDisplay("{Name}")>]
type IndicatorDescription internal (connection:ServiceConnection, topicCode:string, indicatorCode:string) = 
    /// Get the code for the topic of the indicator
    member x.Code = topicCode
    /// Get the code for the indicator
    member x.IndicatorCode = indicatorCode
    /// Get the name of the indicator
    member x.Name = connection.IndicatorsIndexed.[indicatorCode].Name
    /// Get the source of the indicator
    member x.Source = connection.IndicatorsIndexed.[indicatorCode].Source
    /// Get the description of the indicator
    member x.Description = connection.IndicatorsIndexed.[indicatorCode].Description

type IIndicators =
    abstract GetIndicator : indicatorCode:string -> Indicator
    abstract AsyncGetIndicator : indicatorCode:string -> Async<Indicator>

type Indicators internal (connection:ServiceConnection, countryOrRegionCode) = 
    let indicators = seq { for indicator in connection.Indicators -> Indicator(connection, countryOrRegionCode, indicator.Id) }
    interface IIndicators with
        member x.GetIndicator(indicatorCode) = Indicator(connection, countryOrRegionCode, indicatorCode)
        member x.AsyncGetIndicator(indicatorCode) = async { return Indicator(connection, countryOrRegionCode, indicatorCode) }
    interface seq<Indicator> with member x.GetEnumerator() = indicators.GetEnumerator()
    interface IEnumerable with member x.GetEnumerator() = indicators.GetEnumerator() :> _

type IIndicatorsDescriptions =
    abstract GetIndicator : indicatorCode:string -> IndicatorDescription

type IndicatorsDescriptions internal (connection:ServiceConnection, topicCode) = 
    let indicatorsDescriptions = seq { for indicatorId in connection.IndicatorsByTopic.[topicCode] -> IndicatorDescription(connection, topicCode, indicatorId) }
    interface IIndicatorsDescriptions with member x.GetIndicator(indicatorCode) = IndicatorDescription(connection, topicCode, indicatorCode)
    interface seq<IndicatorDescription> with member x.GetEnumerator() = indicatorsDescriptions.GetEnumerator()
    interface IEnumerable with member x.GetEnumerator() = indicatorsDescriptions.GetEnumerator() :> _

type ICountry = 
    abstract GetIndicators : unit -> Indicators

[<DebuggerDisplay("{Name}")>]
[<StructuredFormatDisplay("{Name}")>]
type Country internal (connection:ServiceConnection, countryCode:string) = 
    let indicators = new Indicators(connection, countryCode)
    /// Get the WorldBank code of the country
    member x.Code = countryCode
    /// Get the name of the country 
    member x.Name = connection.CountriesIndexed.[countryCode].Name
    /// Get the capital city of the country 
    member x.CapitalCity = connection.CountriesIndexed.[countryCode].CapitalCity
    /// Get the region of the country 
    member x.Region = connection.CountriesIndexed.[countryCode].Region
    interface ICountry with member x.GetIndicators() = indicators

type ICountryCollection =
    abstract GetCountry : countryCode:string -> Country

type CountryCollection<'T when 'T :> Country> internal (connection: ServiceConnection, regionCodeOpt) = 
    let items = 
        seq { let countries = 
                  match regionCodeOpt with 
                  | None -> connection.Countries 
                  | Some r -> connection.GetCountriesInRegion(r)
              for country in countries do 
                if not country.IsRegion then
                  yield Country(connection, country.Id) :?> 'T }  
    interface seq<'T> with member x.GetEnumerator() = items.GetEnumerator()
    interface IEnumerable with member x.GetEnumerator() = (items :> IEnumerable).GetEnumerator()
    interface ICountryCollection with member x.GetCountry(countryCode) = Country(connection, countryCode)
    
type IRegion =
    abstract GetCountries<'T when 'T :> Country> : unit -> CountryCollection<'T>
    abstract GetIndicators : unit -> Indicators

[<DebuggerDisplay("{Name}")>]
[<StructuredFormatDisplay("{Name}")>]
type Region internal (connection:ServiceConnection, regionCode:string) = 
    let indicators = new Indicators(connection, regionCode)
    /// Get the WorldBank code for the region
    member x.RegionCode = regionCode
    /// Get the name of the region
    member x.Name = connection.RegionsIndexed.[regionCode]
    interface IRegion with
        member x.GetCountries() = CountryCollection(connection,Some regionCode)
        member x.GetIndicators() = indicators
    
type IRegionCollection =
    abstract GetRegion : regionCode:string -> Region

type RegionCollection<'T when 'T :> Region> internal (connection: ServiceConnection) = 
    let items = seq { for (code, _) in connection.Regions -> Region(connection, code) :?> 'T } 
    interface seq<'T> with member x.GetEnumerator() = items.GetEnumerator()
    interface IEnumerable with member x.GetEnumerator() = (items :> IEnumerable).GetEnumerator()
    interface IRegionCollection with member x.GetRegion(regionCode) = Region(connection, regionCode)

type ITopic = 
    abstract GetIndicators : unit -> IndicatorsDescriptions

[<DebuggerDisplay("{Name}")>]
[<StructuredFormatDisplay("{Name}")>]
type Topic internal (connection:ServiceConnection, topicCode:string) = 
    let indicatorsDescriptions = new IndicatorsDescriptions(connection, topicCode)
    /// Get the WorldBank code of the topic
    member x.Code = topicCode
    /// Get the name of the topic 
    member x.Name = connection.TopicsIndexed.[topicCode].Name
    /// Get the description of the topic 
    member x.Description = connection.TopicsIndexed.[topicCode].Description
    interface ITopic with member x.GetIndicators() = indicatorsDescriptions

type ITopicCollection =
    abstract GetTopic : topicCode:string -> Topic

type TopicCollection<'T when 'T :> Topic> internal (connection: ServiceConnection) = 
    let items = seq { for topic in connection.Topics -> Topic(connection, topic.Id) :?> 'T } 
    interface seq<'T> with member x.GetEnumerator() = items.GetEnumerator()
    interface IEnumerable with member x.GetEnumerator() = (items :> IEnumerable).GetEnumerator()
    interface ITopicCollection with member x.GetTopic(topicCode) = Topic(connection, topicCode)

type IWorldBankData =
    abstract GetCountries<'T when 'T :> Country> : unit -> seq<'T>
    abstract GetRegions<'T when 'T :> Region> : unit -> seq<'T>
    abstract GetTopics<'T when 'T :> Topic> : unit -> seq<'T>

type WorldBankData(serviceUrl:string, sources:string) = 
    let sources = sources.Split([| ';' |], StringSplitOptions.RemoveEmptyEntries) |> Array.toList
    let restCache, _ = createInternetFileCache "WorldBankRuntime"
    let connection = new ServiceConnection(restCache, serviceUrl, sources)
    interface IWorldBankData with
        member x.GetCountries() = CountryCollection(connection, None) :> seq<_>
        member x.GetRegions() = RegionCollection(connection) :> seq<_>
        member x.GetTopics() = TopicCollection(connection) :> seq<_>
