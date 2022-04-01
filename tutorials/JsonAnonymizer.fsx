(**
// can't yet format YamlFrontmatter (["category: Tutorials"; "categoryindex: 3"; "index: 1"], Some { StartLine = 2 StartColumn = 0 EndLine = 5 EndColumn = 8 }) to pynb markdown

*)
#r "nuget: FSharp.Data,4.2.8"
(**
# Anonymizing JSON

[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/diffsharp/diffsharp.github.io/master?filepath=tutorials/JsonAnonymizer.ipynb)&emsp;
[![Script](../img/badge-script.svg)](https://fsprojects.github.io/FSharp.Data//tutorials/JsonAnonymizer.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](https://fsprojects.github.io/FSharp.Data//tutorials/JsonAnonymizer.ipynb)

This tutorial shows how to implement an anonymizer for a JSON document (represented using
the [JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html) type discussed in [JSON parser article](JsonValue.html))
This functionality is not directly available in the FSharp.Data package, but it can
be very easily implemented by recursively walking over the JSON document.

If you want to use the JSON anonymizer in your code, you can copy the
[source from GitHub](https://github.com/fsharp/FSharp.Data/blob/master/docs/content/tutorials/JsonAnonymizer.fsx) and just include it in your project. If you use these
functions often and would like to see them in the FSharp.Data package, please submit
a [feature request](https://github.com/fsharp/FSharp.Data/issues).

**DISCLAIMER**: Don't use this for sensitive data as it's just a sample

*)
open System
open System.Globalization
open FSharp.Data

type JsonAnonymizer(?propertiesToSkip, ?valuesToSkip) =

  let propertiesToSkip = Set.ofList (defaultArg propertiesToSkip [])
  let valuesToSkip = Set.ofList (defaultArg valuesToSkip [])

  let rng = Random()

  let digits = [| '0' .. '9' |]
  let lowerLetters = [| 'a' .. 'z' |]
  let upperLetters = [| 'A' .. 'Z' |]

  let getRandomChar (c:char) =
      if Char.IsDigit c then digits.[rng.Next(10)]
      elif Char.IsLetter c then
          if Char.IsLower c
          then lowerLetters.[rng.Next(26)]
          else upperLetters.[rng.Next(26)]
      else c

  let randomize (str:string) =
      String(str.ToCharArray() |> Array.map getRandomChar)

  let rec anonymize json =
      match json with
      | JsonValue.String s when valuesToSkip.Contains s -> json
      | JsonValue.String s ->
          let typ =
            Runtime.StructuralInference.inferPrimitiveType
              CultureInfo.InvariantCulture s

          ( if typ = typeof<Guid> then Guid.NewGuid().ToString()
            elif typ = typeof<Runtime.StructuralTypes.Bit0> ||
              typ = typeof<Runtime.StructuralTypes.Bit1> then s
            elif typ = typeof<DateTime> then s
            else
              let prefix, s =
                if s.StartsWith "http://" then
                  "http://", s.Substring("http://".Length)
                elif s.StartsWith "https://" then
                  "https://", s.Substring("https://".Length)
                else "", s
              prefix + randomize s )
          |> JsonValue.String
      | JsonValue.Number d ->
          let typ =
            Runtime.StructuralInference.inferPrimitiveType
              CultureInfo.InvariantCulture (d.ToString())
          if typ = typeof<Runtime.StructuralTypes.Bit0> ||
            typ = typeof<Runtime.StructuralTypes.Bit1> then json
          else d.ToString() |> randomize |> Decimal.Parse |> JsonValue.Number
      | JsonValue.Float f ->
          f.ToString()
          |> randomize
          |> Double.Parse
          |> JsonValue.Float
      | JsonValue.Boolean _  | JsonValue.Null -> json
      | JsonValue.Record props ->
          props
          |> Array.map (fun (key, value) ->
              let newValue = if propertiesToSkip.Contains key then value else anonymize value
              key, newValue)
          |> JsonValue.Record
      | JsonValue.Array array ->
          array
          |> Array.map anonymize
          |> JsonValue.Array

  member __.Anonymize json = anonymize json

let json = JsonValue.Load (__SOURCE_DIRECTORY__ + "../../data/TwitterStream.json")
printfn "%O" json

let anonymizedJson = (JsonAnonymizer ["lang"]).Anonymize json
printfn "%O" anonymizedJson
(**
## Related articles

// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 5 StartColumn = 3 EndLine = 5 EndColumn = 18 }); DirectLink ([Literal ("JsonValue", Some { StartLine = 5 StartColumn = 19 EndLine = 5 EndColumn = 47 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html", None, Some { StartLine = 5 StartColumn = 19 EndLine = 5 EndColumn = 47 })], Some { StartLine = 5 StartColumn = 0 EndLine = 5 EndColumn = 48 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("JSON Parser", Some { StartLine = 6 StartColumn = 3 EndLine = 7 EndColumn = 14 })], "../library/JsonValue.html", None, Some { StartLine = 6 StartColumn = 3 EndLine = 7 EndColumn = 75 }); Literal (" - a tutorial that introduces
", Some { StartLine = 6 StartColumn = 3 EndLine = 7 EndColumn = 34 }); DirectLink ([Literal ("JsonValue", Some { StartLine = 6 StartColumn = 35 EndLine = 7 EndColumn = 74 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html", None, Some { StartLine = 6 StartColumn = 35 EndLine = 7 EndColumn = 74 }); Literal (" for working with JSON values dynamically.", Some { StartLine = 6 StartColumn = 34 EndLine = 7 EndColumn = 76 })], Some { StartLine = 5 StartColumn = 0 EndLine = 5 EndColumn = 48 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("JSON Type Provider", Some { StartLine = 8 StartColumn = 3 EndLine = 10 EndColumn = 21 })], "../library/JsonProvider.html", None, Some { StartLine = 8 StartColumn = 3 EndLine = 10 EndColumn = -2 }); Literal (" - discusses F# type provider
that provides type-safe access to JSON data.", Some { StartLine = 8 StartColumn = 3 EndLine = 10 EndColumn = 78 })], Some { StartLine = 5 StartColumn = 0 EndLine = 5 EndColumn = 48 }) to pynb markdown
*)

