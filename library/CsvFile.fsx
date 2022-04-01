(**
// can't yet format YamlFrontmatter (["category: Utilities"; "categoryindex: 2"; "index: 2"], Some { StartLine = 2 StartColumn = 0 EndLine = 5 EndColumn = 8 }) to pynb markdown

*)
#r "nuget: FSharp.Data,4.2.8"
(**
[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/fsprojects/FSharp.Data/gh-pages?filepath=library/CsvFile.ipynb)&emsp;
[![Script](../img/badge-script.svg)](https://fsprojects.github.io/FSharp.Data//library/CsvFile.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](https://fsprojects.github.io/FSharp.Data//library/CsvFile.ipynb)

# CSV Parser

The F# [CSV Type Provider](CsvProvider.html) is built on top of an efficient CSV parser written
in F#. There's also a simple API that can be used to access values dynamically.

When working with well-defined CSV documents, it is easier to use the
[type provider](CsvProvider.html), but in a more dynamic scenario or when writing
quick and simple scripts, the parser might be a simpler option.

## Loading CSV documents

To load a sample CSV document, we first need to reference the `FSharp.Data` package.

*)
open FSharp.Data
(**
The `FSharp.Data` namespace contains the [CsvFile](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvfile.html) type that provides two static methods
for loading data. The [CsvFile.Parse](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvfile.html) method can be used if we have the data in a `string` value.
The [CsvFile.Load](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvfile.html) method allows reading the data from a file or from a web resource (and there's
also an asynchronous [CsvFile.AsyncLoad](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvfile.html) version). The following sample calls [CsvFile.Load](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvfile.html) with a URL that
points to a live CSV file on the Yahoo finance web site:

*)
// Download the stock prices
let msft = CsvFile.Load(__SOURCE_DIRECTORY__ + "/../data/MSFT.csv").Cache()

// Print the prices in the HLOC format
for row in msft.Rows |> Seq.truncate 10 do
  printfn "HLOC: (%s, %s, %s)"
    (row.GetColumn "High") (row.GetColumn "Low") (row.GetColumn "Date")(* output: 
HLOC: (76.55, 75.86, 9-Oct-17)
HLOC: (76.03, 75.54, 6-Oct-17)
HLOC: (76.12, 74.96, 5-Oct-17)
HLOC: (74.72, 73.71, 4-Oct-17)
HLOC: (74.88, 74.20, 3-Oct-17)
HLOC: (75.01, 74.30, 2-Oct-17)
HLOC: (74.54, 73.88, 29-Sep-17)
HLOC: (73.97, 73.31, 28-Sep-17)
HLOC: (74.17, 73.17, 27-Sep-17)
HLOC: (73.81, 72.99, 26-Sep-17)
val msft : Runtime.CsvFile<CsvRow>
val it : unit = ()*)
(**
Note that unlike `CsvProvider`, [CsvFile](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvfile.html) works in streaming mode for performance reasons, which means
that [CsvFile.Rows](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-runtime-csvfile-1.html#Rows) can only be iterated once. If you need to iterate multiple times, use the [CsvFile.Cache](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-runtime-csvfile-1.html#Cache) method,
but please note that this will increase memory usage and should not be used in large datasets.

## Using CSV extensions

Now we look at a number of extensions that become available after
opening the [CsvExtensions](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvextensionsmodule.html) namespace. Once opened, we can write:

// can't yet format Span ([InlineCode ("row?column", Some { StartLine = 12 StartColumn = 4 EndLine = 13 EndColumn = 61 }); Literal (" uses the dynamic operator to obtain the column value named ", Some { StartLine = 12 StartColumn = 3 EndLine = 13 EndColumn = 63 }); InlineCode ("column", Some { StartLine = 12 StartColumn = 64 EndLine = 13 EndColumn = 61 }); Literal (";
alternatively, you can also use an indexer ", Some { StartLine = 12 StartColumn = 63 EndLine = 13 EndColumn = 109 }); InlineCode ("row.[column]", Some { StartLine = 12 StartColumn = 110 EndLine = 13 EndColumn = 61 }); Literal (".", Some { StartLine = 12 StartColumn = 109 EndLine = 13 EndColumn = 110 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 84 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsBoolean()", Some { StartLine = 14 StartColumn = 4 EndLine = 14 EndColumn = 138 }); Literal (" returns the value as boolean if it is either ", Some { StartLine = 14 StartColumn = 3 EndLine = 14 EndColumn = 49 }); InlineCode ("true", Some { StartLine = 14 StartColumn = 50 EndLine = 14 EndColumn = 138 }); Literal (" or ", Some { StartLine = 14 StartColumn = 49 EndLine = 14 EndColumn = 53 }); InlineCode ("false", Some { StartLine = 14 StartColumn = 54 EndLine = 14 EndColumn = 138 }); Literal (" (see ", Some { StartLine = 14 StartColumn = 53 EndLine = 14 EndColumn = 59 }); DirectLink ([Literal ("StringExtensions.AsBoolean", Some { StartLine = 14 StartColumn = 60 EndLine = 14 EndColumn = 138 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-stringextensions.html", None, Some { StartLine = 14 StartColumn = 60 EndLine = 14 EndColumn = 138 }); Literal (")", Some { StartLine = 14 StartColumn = 59 EndLine = 14 EndColumn = 60 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 84 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsInteger()", Some { StartLine = 15 StartColumn = 4 EndLine = 17 EndColumn = 37 }); Literal (" returns the value as integer if it is numeric and can be
converted to an integer; ", Some { StartLine = 15 StartColumn = 3 EndLine = 17 EndColumn = 87 }); InlineCode ("value.AsInteger64()", Some { StartLine = 15 StartColumn = 88 EndLine = 17 EndColumn = 37 }); Literal (", ", Some { StartLine = 15 StartColumn = 87 EndLine = 17 EndColumn = 89 }); InlineCode ("value.AsDecimal()", Some { StartLine = 15 StartColumn = 90 EndLine = 17 EndColumn = 37 }); Literal (" and
", Some { StartLine = 15 StartColumn = 89 EndLine = 17 EndColumn = 95 }); InlineCode ("value.AsFloat()", Some { StartLine = 15 StartColumn = 96 EndLine = 17 EndColumn = 37 }); Literal (" behave similarly.", Some { StartLine = 15 StartColumn = 95 EndLine = 17 EndColumn = 113 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 84 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsDateTime()", Some { StartLine = 18 StartColumn = 4 EndLine = 20 EndColumn = 80 }); Literal (" returns the value as a ", Some { StartLine = 18 StartColumn = 3 EndLine = 20 EndColumn = 27 }); InlineCode ("DateTime", Some { StartLine = 18 StartColumn = 28 EndLine = 20 EndColumn = 80 }); Literal (" value using either the
", Some { StartLine = 18 StartColumn = 27 EndLine = 20 EndColumn = 52 }); DirectLink ([Literal ("ISO 8601", Some { StartLine = 18 StartColumn = 52 EndLine = 20 EndColumn = 60 })], "http://en.wikipedia.org/wiki/ISO_8601", None, Some { StartLine = 18 StartColumn = 52 EndLine = 20 EndColumn = 81 }); Literal (" format, or using the
", Some { StartLine = 18 StartColumn = 52 EndLine = 20 EndColumn = 75 }); InlineCode ("\/Date(...)\/", Some { StartLine = 18 StartColumn = 76 EndLine = 20 EndColumn = 80 }); Literal (" JSON format containing number of milliseconds since 1/1/1970.", Some { StartLine = 18 StartColumn = 75 EndLine = 20 EndColumn = 137 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 84 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsDateTimeOffset()", Some { StartLine = 21 StartColumn = 4 EndLine = 24 EndColumn = 63 }); Literal (" parses the string as a ", Some { StartLine = 21 StartColumn = 3 EndLine = 24 EndColumn = 27 }); InlineCode ("DateTimeOffset", Some { StartLine = 21 StartColumn = 28 EndLine = 24 EndColumn = 63 }); Literal (" value using either the
", Some { StartLine = 21 StartColumn = 27 EndLine = 24 EndColumn = 52 }); DirectLink ([Literal ("ISO 8601", Some { StartLine = 21 StartColumn = 52 EndLine = 24 EndColumn = 60 })], "http://en.wikipedia.org/wiki/ISO_8601", None, Some { StartLine = 21 StartColumn = 52 EndLine = 24 EndColumn = 64 }); Literal (" format, or using the
", Some { StartLine = 21 StartColumn = 52 EndLine = 24 EndColumn = 75 }); InlineCode ("\/Date(...[+/-]offset)\/", Some { StartLine = 21 StartColumn = 76 EndLine = 24 EndColumn = 63 }); Literal (" JSON format containing number of milliseconds since 1/1/1970,
", Some { StartLine = 21 StartColumn = 75 EndLine = 24 EndColumn = 139 }); IndirectLink ([Literal ("+/-", Some { StartLine = 21 StartColumn = 139 EndLine = 24 EndColumn = 142 })], "", "+/-", Some { StartLine = 21 StartColumn = 139 EndLine = 24 EndColumn = 64 }); Literal (" the 4 digit offset. Example- ", Some { StartLine = 21 StartColumn = 139 EndLine = 24 EndColumn = 169 }); InlineCode ("\/Date(1231456+1000)\/", Some { StartLine = 21 StartColumn = 170 EndLine = 24 EndColumn = 63 }); Literal (".", Some { StartLine = 21 StartColumn = 169 EndLine = 24 EndColumn = 170 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 84 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsTimeSpan()", Some { StartLine = 25 StartColumn = 4 EndLine = 25 EndColumn = 63 }); Literal (" parses the string as a ", Some { StartLine = 25 StartColumn = 3 EndLine = 25 EndColumn = 27 }); InlineCode ("TimeSpan", Some { StartLine = 25 StartColumn = 28 EndLine = 25 EndColumn = 63 }); Literal (" value.", Some { StartLine = 25 StartColumn = 27 EndLine = 25 EndColumn = 34 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 84 }) to pynb markdown
// can't yet format Span ([InlineCode ("value.AsGuid()", Some { StartLine = 26 StartColumn = 4 EndLine = 27 EndColumn = -3 }); Literal (" returns the value as a ", Some { StartLine = 26 StartColumn = 3 EndLine = 27 EndColumn = 27 }); InlineCode ("Guid", Some { StartLine = 26 StartColumn = 28 EndLine = 27 EndColumn = -3 }); Literal (" value.", Some { StartLine = 26 StartColumn = 27 EndLine = 27 EndColumn = 34 })], Some { StartLine = 12 StartColumn = 0 EndLine = 12 EndColumn = 84 }) to pynb markdown
Methods that may need to parse a numeric value or date (such as `AsFloat` and
`AsDateTime`) receive an optional culture parameter.

The following example shows how to process the sample previous CSV sample using these extensions:

*)
open FSharp.Data.CsvExtensions

for row in msft.Rows |> Seq.truncate 10 do
  printfn "HLOC: (%f, %M, %O)"
    (row.["High"].AsFloat()) (row?Low.AsDecimal()) (row?Date.AsDateTime())(* output: 
HLOC: (76.550000, 75.86, 10/9/2017 12:00:00 AM)
HLOC: (76.030000, 75.54, 10/6/2017 12:00:00 AM)
HLOC: (76.120000, 74.96, 10/5/2017 12:00:00 AM)
HLOC: (74.720000, 73.71, 10/4/2017 12:00:00 AM)
HLOC: (74.880000, 74.20, 10/3/2017 12:00:00 AM)
HLOC: (75.010000, 74.30, 10/2/2017 12:00:00 AM)
HLOC: (74.540000, 73.88, 9/29/2017 12:00:00 AM)
HLOC: (73.970000, 73.31, 9/28/2017 12:00:00 AM)
HLOC: (74.170000, 73.17, 9/27/2017 12:00:00 AM)
HLOC: (73.810000, 72.99, 9/26/2017 12:00:00 AM)
val it : unit = ()*)
(**
## Transforming CSV files

In addition to reading, [CsvFile](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-runtime-csvfile-1.html) also has support for transforming CSV files. The operations
available are [CsvFile](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-runtime-csvfile-1.html), `Take`, `TakeWhile`, `Skip`, `SkipWhile`, and `Truncate`. After transforming
you can save the results by using one of the overloads of the `Save` method. You can choose different
separator and quote characters when saving.

*)
// Saving the first 10 stock prices where the closing price is higher than the opening price in TSV format:
msft.Filter(fun row -> row?Close.AsFloat() > row?Open.AsFloat())
    .Truncate(10)
    .SaveToString('\t')(* output: 
val it : string =
  "Date	Open	High	Low	Close	Volume
9-Oct-17	75.97	76.55	75.86	76.29	11386502
6-Oct-17	75.67	76.03	75.54	76.00	13959814
5-Oct-17	75.22	76.12	74.96	75.97	21195261
4-Oct-17	74.09	74.72	73.71	74.69	13317681
29-Sep-17	73.94	74.54	73.88	74.49	17079114
28-Sep-17	73.54	73.97	73.31	73.87	10883787
27-Sep-17	73.55	74.17	73.17	73.85	19375099
22-Sep-17	73.99	74.51	73.85	74.41	14111365
19-Sep-17	75.21	75.71	75.01	75.44	16093344
15-Sep-17	74.83	75.39	74.07	75.31	38578441
"*)
(**
## Related articles

// can't yet format Span ([DirectLink ([Literal ("CSV Type Provider", Some { StartLine = 5 StartColumn = 3 EndLine = 6 EndColumn = 20 })], "CsvProvider.html", None, Some { StartLine = 5 StartColumn = 3 EndLine = 6 EndColumn = 45 }); Literal (" - discusses F# type provider
that provides type-safe access to CSV data", Some { StartLine = 5 StartColumn = 3 EndLine = 6 EndColumn = 76 })], Some { StartLine = 5 StartColumn = 0 EndLine = 5 EndColumn = 69 }) to pynb markdown
// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 7 StartColumn = 3 EndLine = 7 EndColumn = 18 }); DirectLink ([Literal ("CsvFile", Some { StartLine = 7 StartColumn = 19 EndLine = 7 EndColumn = 45 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvfile.html", None, Some { StartLine = 7 StartColumn = 19 EndLine = 7 EndColumn = 45 })], Some { StartLine = 5 StartColumn = 0 EndLine = 5 EndColumn = 69 }) to pynb markdown
// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 8 StartColumn = 3 EndLine = 8 EndColumn = 18 }); DirectLink ([Literal ("CsvRow", Some { StartLine = 8 StartColumn = 19 EndLine = 8 EndColumn = 44 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvrow.html", None, Some { StartLine = 8 StartColumn = 19 EndLine = 8 EndColumn = 44 })], Some { StartLine = 5 StartColumn = 0 EndLine = 5 EndColumn = 69 }) to pynb markdown
// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 9 StartColumn = 3 EndLine = 10 EndColumn = 18 }); DirectLink ([Literal ("CsvExtensions", Some { StartLine = 9 StartColumn = 19 EndLine = 10 EndColumn = -3 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-csvextensionsmodule.html", None, Some { StartLine = 9 StartColumn = 19 EndLine = 10 EndColumn = -3 })], Some { StartLine = 5 StartColumn = 0 EndLine = 5 EndColumn = 69 }) to pynb markdown
*)

