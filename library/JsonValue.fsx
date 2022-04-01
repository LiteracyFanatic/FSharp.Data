(**
// can't yet format YamlFrontmatter (["category: Utilities"; "categoryindex: 1"; "index: 5"], Some { StartLine = 2 StartColumn = 0 EndLine = 5 EndColumn = 8 }) to pynb markdown

*)
#r "nuget: FSharp.Data,4.2.8"
(**
[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/fsprojects/FSharp.Data/gh-pages?filepath=library/JsonValue.ipynb)&emsp;
[![Script](../img/badge-script.svg)](https://fsprojects.github.io/FSharp.Data//library/JsonValue.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](https://fsprojects.github.io/FSharp.Data//library/JsonValue.ipynb)

# JSON Parser

The F# [JSON Type Provider](JsonProvider.html) is built on top of an efficient JSON parser written
in F#.

When working with well-defined JSON documents, it is easier to use the
[type provider](JsonProvider.html), but in a more dynamic scenario or when writing
quick and simple scripts, the parser might be a simpler option.

## Loading JSON documents

To load a sample JSON document, we first need to reference the `FSharp.Data` package.

*)
open FSharp.Data
(**
The `FSharp.Data` namespace contains the [JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html) type that can be used
to parse strings formatted using JSON as follows:

*)
let info =
  JsonValue.Parse("""
    { "name": "Tomas", "born": 1985,
      "siblings": [ "Jan", "Alexander" ] } """)(* output: 
val info : JsonValue =
  {
  "name": "Tomas",
  "born": 1985,
  "siblings": [
    "Jan",
    "Alexander"
  ]
}*)
(**
The parsed value can be processed using pattern matching - the [JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html) type
is a discriminated union with cases such as `Record`, `Collection` and others that
can be used to examine the structure.

## Using JSON extensions

We do not cover this technique in this introduction. Instead, we look at a number
of extensions that become available after opening the [JsonExtensions](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonextensionsmodule.html)
module. Once opened, we can write:

// can't yet format Span ([InlineCode ("value.AsBoolean()", Some { StartLine = 12 StartColumn = 4 EndLine = 12 EndColumn = 85 }); Literal (" returns the value as boolean if it is either ", Some { StartLine = 12 StartColumn = 3 EndLine = 12 EndColumn = 49 }); InlineCode ("true", Some { StartLine = 12 StartColumn = 50 EndLine = 12 EndColumn = 85 }); Literal (" or ", Some { StartLine = 12 StartColumn = 49 EndLine = 12 EndColumn = 53 }); InlineCode ("false", Some { StartLine = 12 StartColumn = 54 EndLine = 12 EndColumn = 85 }); Literal (".", Some { StartLine = 12 StartColumn = 53 EndLine = 12 EndColumn = 54 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsInteger()", Some { StartLine = 13 StartColumn = 4 EndLine = 15 EndColumn = 37 }); Literal (" returns the value as integer if it is numeric and can be
converted to an integer; ", Some { StartLine = 13 StartColumn = 3 EndLine = 15 EndColumn = 87 }); InlineCode ("value.AsInteger64()", Some { StartLine = 13 StartColumn = 88 EndLine = 15 EndColumn = 37 }); Literal (", ", Some { StartLine = 13 StartColumn = 87 EndLine = 15 EndColumn = 89 }); InlineCode ("value.AsDecimal()", Some { StartLine = 13 StartColumn = 90 EndLine = 15 EndColumn = 37 }); Literal (" and
", Some { StartLine = 13 StartColumn = 89 EndLine = 15 EndColumn = 95 }); InlineCode ("value.AsFloat()", Some { StartLine = 13 StartColumn = 96 EndLine = 15 EndColumn = 37 }); Literal (" behave similarly.", Some { StartLine = 13 StartColumn = 95 EndLine = 15 EndColumn = 113 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsString()", Some { StartLine = 16 StartColumn = 4 EndLine = 16 EndColumn = 51 }); Literal (" returns the value as a string.", Some { StartLine = 16 StartColumn = 3 EndLine = 16 EndColumn = 34 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsDateTime()", Some { StartLine = 17 StartColumn = 4 EndLine = 19 EndColumn = 80 }); Literal (" parses the string as a ", Some { StartLine = 17 StartColumn = 3 EndLine = 19 EndColumn = 27 }); InlineCode ("DateTime", Some { StartLine = 17 StartColumn = 28 EndLine = 19 EndColumn = 80 }); Literal (" value using either the
", Some { StartLine = 17 StartColumn = 27 EndLine = 19 EndColumn = 52 }); DirectLink ([Literal ("ISO 8601", Some { StartLine = 17 StartColumn = 52 EndLine = 19 EndColumn = 60 })], "http://en.wikipedia.org/wiki/ISO_8601", None, Some { StartLine = 17 StartColumn = 52 EndLine = 19 EndColumn = 81 }); Literal (" format, or using the
", Some { StartLine = 17 StartColumn = 52 EndLine = 19 EndColumn = 75 }); InlineCode ("\/Date(...)\/", Some { StartLine = 17 StartColumn = 76 EndLine = 19 EndColumn = 80 }); Literal (" JSON format containing number of milliseconds since 1/1/1970.", Some { StartLine = 17 StartColumn = 75 EndLine = 19 EndColumn = 137 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsDateTimeOffset()", Some { StartLine = 20 StartColumn = 4 EndLine = 23 EndColumn = 63 }); Literal (" parses the string as a ", Some { StartLine = 20 StartColumn = 3 EndLine = 23 EndColumn = 27 }); InlineCode ("DateTimeOffset", Some { StartLine = 20 StartColumn = 28 EndLine = 23 EndColumn = 63 }); Literal (" value using either the
", Some { StartLine = 20 StartColumn = 27 EndLine = 23 EndColumn = 52 }); DirectLink ([Literal ("ISO 8601", Some { StartLine = 20 StartColumn = 52 EndLine = 23 EndColumn = 60 })], "http://en.wikipedia.org/wiki/ISO_8601", None, Some { StartLine = 20 StartColumn = 52 EndLine = 23 EndColumn = 64 }); Literal (" format, or using the
", Some { StartLine = 20 StartColumn = 52 EndLine = 23 EndColumn = 75 }); InlineCode ("\/Date(...[+/-]offset)\/", Some { StartLine = 20 StartColumn = 76 EndLine = 23 EndColumn = 63 }); Literal (" JSON format containing number of milliseconds since 1/1/1970,
", Some { StartLine = 20 StartColumn = 75 EndLine = 23 EndColumn = 139 }); IndirectLink ([Literal ("+/-", Some { StartLine = 20 StartColumn = 139 EndLine = 23 EndColumn = 142 })], "", "+/-", Some { StartLine = 20 StartColumn = 139 EndLine = 23 EndColumn = 64 }); Literal (" the 4 digit offset. Example- ", Some { StartLine = 20 StartColumn = 139 EndLine = 23 EndColumn = 169 }); InlineCode ("\/Date(1231456+1000)\/", Some { StartLine = 20 StartColumn = 170 EndLine = 23 EndColumn = 63 }); Literal (".", Some { StartLine = 20 StartColumn = 169 EndLine = 23 EndColumn = 170 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsTimeSpan()", Some { StartLine = 24 StartColumn = 4 EndLine = 24 EndColumn = 63 }); Literal (" parses the string as a ", Some { StartLine = 24 StartColumn = 3 EndLine = 24 EndColumn = 27 }); InlineCode ("TimeSpan", Some { StartLine = 24 StartColumn = 28 EndLine = 24 EndColumn = 63 }); Literal (" value.", Some { StartLine = 24 StartColumn = 27 EndLine = 24 EndColumn = 34 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsGuid()", Some { StartLine = 25 StartColumn = 4 EndLine = 25 EndColumn = 55 }); Literal (" parses the string as a ", Some { StartLine = 25 StartColumn = 3 EndLine = 25 EndColumn = 27 }); InlineCode ("Guid", Some { StartLine = 25 StartColumn = 28 EndLine = 25 EndColumn = 55 }); Literal (" value.", Some { StartLine = 25 StartColumn = 27 EndLine = 25 EndColumn = 34 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value?child", Some { StartLine = 26 StartColumn = 4 EndLine = 28 EndColumn = 19 }); Literal (" uses the dynamic operator to obtain a record member named ", Some { StartLine = 26 StartColumn = 3 EndLine = 28 EndColumn = 62 }); InlineCode ("child", Some { StartLine = 26 StartColumn = 63 EndLine = 28 EndColumn = 19 }); Literal (";
alternatively, you can also use ", Some { StartLine = 26 StartColumn = 62 EndLine = 28 EndColumn = 97 }); InlineCode ("value.GetProperty(child)", Some { StartLine = 26 StartColumn = 98 EndLine = 28 EndColumn = 19 }); Literal (" or an indexer
", Some { StartLine = 26 StartColumn = 97 EndLine = 28 EndColumn = 113 }); InlineCode ("value.[child]", Some { StartLine = 26 StartColumn = 114 EndLine = 28 EndColumn = 19 }); Literal (".", Some { StartLine = 26 StartColumn = 113 EndLine = 28 EndColumn = 114 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.TryGetProperty(child)", Some { StartLine = 29 StartColumn = 4 EndLine = 31 EndColumn = 19 }); Literal (" can be used to safely obtain a record member
(if the member is missing or the value is not a record then, ", Some { StartLine = 29 StartColumn = 3 EndLine = 31 EndColumn = 111 }); InlineCode ("TryGetProperty", Some { StartLine = 29 StartColumn = 112 EndLine = 31 EndColumn = 19 }); Literal ("
returns ", Some { StartLine = 29 StartColumn = 111 EndLine = 31 EndColumn = 121 }); InlineCode ("None", Some { StartLine = 29 StartColumn = 122 EndLine = 31 EndColumn = 19 }); Literal (").", Some { StartLine = 29 StartColumn = 121 EndLine = 31 EndColumn = 123 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("[ for v in value -> v ]", Some { StartLine = 32 StartColumn = 4 EndLine = 34 EndColumn = 20 }); Literal (" treats ", Some { StartLine = 32 StartColumn = 3 EndLine = 34 EndColumn = 11 }); InlineCode ("value", Some { StartLine = 32 StartColumn = 12 EndLine = 34 EndColumn = 20 }); Literal (" as a collection and iterates over it;
alternatively, it is possible to obtain all elements as an array using
", Some { StartLine = 32 StartColumn = 11 EndLine = 34 EndColumn = 123 }); InlineCode ("value.AsArray()", Some { StartLine = 32 StartColumn = 124 EndLine = 34 EndColumn = 20 }); Literal (".", Some { StartLine = 32 StartColumn = 123 EndLine = 34 EndColumn = 124 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.Properties()", Some { StartLine = 35 StartColumn = 4 EndLine = 35 EndColumn = 73 }); Literal (" returns a list of all properties of a record node.", Some { StartLine = 35 StartColumn = 3 EndLine = 35 EndColumn = 54 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.InnerText()", Some { StartLine = 36 StartColumn = 4 EndLine = 38 EndColumn = -3 }); Literal (" concatenates all text or text in an array
(representing e.g. multi-line string).", Some { StartLine = 36 StartColumn = 3 EndLine = 38 EndColumn = 85 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 86 }) to pynb markdown
Methods that may need to parse a numeric value or date (such as `AsFloat` and
`AsDateTime`) receive an optional culture parameter.

The following example shows how to process the sample JSON value:

*)
open FSharp.Data.JsonExtensions

// Print name and birth year
let n = info?name
printfn "%s (%d)" (info?name.AsString()) (info?born.AsInteger())

// Print names of all siblings
for sib in info?siblings do
  printfn "%s" (sib.AsString())(* output: 
Tomas (1985)
Jan
Alexander
val n : JsonValue = "Tomas"
val it : unit = ()*)
(**
Note that the [JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html) type does not actually implement the `IEnumerable<'T>`
interface (meaning that it cannot be passed to `Seq.xyz` functions). It only has
the `GetEnumerator` method, which makes it possible to use it in sequence expressions
and with the `for` loop.

## Parsing WorldBank response

To look at a more complex example, consider a sample document
[`data/WorldBank.json`](../data/WorldBank.json) which was obtained as a response to
a WorldBank request (you can access the WorldBank data more conveniently using
[a type provider](WorldBank.html)). The document looks as follows:

    [lang=js]
    [ { "page": 1, "pages": 1, "total": 53 },
      [ { "indicator": {"value": "Central government debt, total (% of GDP)"},
          "country": {"id":"CZ","value":"Czech Republic"},
          "value":null,"decimal":"1","date":"2000"},
        { "indicator": {"value": "Central government debt, total (% of GDP)"},
          "country": {"id":"CZ","value":"Czech Republic"},
          "value":"16.6567773464055","decimal":"1","date":"2010"} ] ]

The document is formed by an array that contains a record as the first element
and a collection of data points as the second element. The following code
reads the document and parses it:

*)
let value = JsonValue.Load(__SOURCE_DIRECTORY__ + "../../data/WorldBank.json")
(**
Note that we can also load the data directly from the web, and there's an
asynchronous version available too:

*)
let wbReq =
  "http://api.worldbank.org/country/cz/indicator/" +
  "GC.DOD.TOTL.GD.ZS?format=json"

let valueAsync =
  JsonValue.AsyncLoad(wbReq)(* output: 
val wbReq : string =
  "http://api.worldbank.org/country/cz/indicator/GC.DOD.TOTL.GD."+[14 chars]
val valueAsync : Async<JsonValue>*)
(**
To split the top-level array into the first record (with overall information)
and the collection of data points, we use pattern matching and match the `value`
against the `JsonValue.Array` constructor:

*)
match value with
| JsonValue.Array [| info; data |] ->
    // Print overall information
    let page, pages, total = info?page, info?pages, info?total
    printfn
      "Showing page %d of %d. Total records %d"
      (page.AsInteger()) (pages.AsInteger()) (total.AsInteger())

    // Print every non-null data point
    for record in data do
      if record?value <> JsonValue.Null then
        printfn "%d: %f" (record?date.AsInteger())
                         (record?value.AsFloat())
| _ -> printfn "failed"(* output: 
Showing page 1 of 1. Total records 53
2010: 35.142297
2009: 31.034880
2008: 25.475164
2007: 24.193320
2006: 23.708055
2005: 22.033462
2004: 20.108379
2003: 18.267725
2002: 15.425565
2001: 14.874434
2000: 13.218869
1999: 11.356696
1998: 10.178780
1997: 10.153566
1996: 10.520301
1995: 12.707834
1994: 14.781808
1993: 16.656777
val it : unit = ()*)
(**
The `value` property of a data point is not always available - as demonstrated
above, the value may be `null`. In that case, we want to skip the data point.
To check whether the property is `null` we simply compare it with `JsonValue.Null`.

The `date` values will be parsed as `DateTimeOffset` if there is an offset present.
However, for a mixed collection of `DateTime` (that is, without the offset) and
`DateTimeOffset` values, the type of the collection will be collection of `DateTime`
after parsing. Also note that the `date` and `value` properties are formatted as strings
in the source file (e.g. `"1990"`) instead of numbers (e.g. `1990`). When you try
accessing the value as an integer or float, the [JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html) automatically parses
the string into the desired format. In general, the API attempts to be as tolerant
as possible when parsing the file.

## Related articles

// can't yet format Span ([DirectLink ([Literal ("JsonValue", Some { StartLine = 17 StartColumn = 4 EndLine = 17 EndColumn = 32 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html", None, Some { StartLine = 17 StartColumn = 4 EndLine = 17 EndColumn = 32 })], Some { StartLine = 17 StartColumn = 0 EndLine = 17 EndColumn = 33 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("JSON Type Provider", Some { StartLine = 18 StartColumn = 3 EndLine = 19 EndColumn = 21 })], "JsonProvider.html", None, Some { StartLine = 18 StartColumn = 3 EndLine = 19 EndColumn = 46 }); Literal (" - discusses a F# type provider
that provides type-safe access to JSON data", Some { StartLine = 18 StartColumn = 3 EndLine = 19 EndColumn = 79 })], Some { StartLine = 17 StartColumn = 0 EndLine = 17 EndColumn = 33 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("WorldBank Provider", Some { StartLine = 20 StartColumn = 3 EndLine = 21 EndColumn = 21 })], "WorldBank.html", None, Some { StartLine = 20 StartColumn = 3 EndLine = 21 EndColumn = 55 }); Literal (" - the WorldBank type provider
can be used to easily access data from the WorldBank", Some { StartLine = 20 StartColumn = 3 EndLine = 21 EndColumn = 87 })], Some { StartLine = 17 StartColumn = 0 EndLine = 17 EndColumn = 33 }) to pynb markdown
// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 22 StartColumn = 3 EndLine = 22 EndColumn = 18 }); DirectLink ([Literal ("JsonValue", Some { StartLine = 22 StartColumn = 19 EndLine = 22 EndColumn = 47 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html", None, Some { StartLine = 22 StartColumn = 19 EndLine = 22 EndColumn = 47 })], Some { StartLine = 17 StartColumn = 0 EndLine = 17 EndColumn = 33 }) to pynb markdown
// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 23 StartColumn = 3 EndLine = 23 EndColumn = 18 }); DirectLink ([Literal ("JsonExtensions", Some { StartLine = 23 StartColumn = 19 EndLine = 23 EndColumn = 57 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonextensions.html", None, Some { StartLine = 23 StartColumn = 19 EndLine = 23 EndColumn = 57 }); Literal (" type", Some { StartLine = 23 StartColumn = 18 EndLine = 23 EndColumn = 23 })], Some { StartLine = 17 StartColumn = 0 EndLine = 17 EndColumn = 33 }) to pynb markdown
// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 24 StartColumn = 3 EndLine = 25 EndColumn = 18 }); DirectLink ([Literal ("JsonExtensions", Some { StartLine = 24 StartColumn = 19 EndLine = 25 EndColumn = -3 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonextensionsmodule.html", None, Some { StartLine = 24 StartColumn = 19 EndLine = 25 EndColumn = -3 }); Literal (" module", Some { StartLine = 24 StartColumn = 18 EndLine = 25 EndColumn = 25 })], Some { StartLine = 17 StartColumn = 0 EndLine = 17 EndColumn = 33 }) to pynb markdown
*)

