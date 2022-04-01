(**
# FSharp.Data: Data Access Made Simple

The FSharp.Data package implements core functionality to
access common data formats in your F# applications and scripts. It contains F# type
providers for working with structured file formats (CSV, HTML, JSON and XML) and helpers for parsing
CSV, HTML and JSON files and for sending HTTP requests.

This library focuses on providing simple access to the structured documents
and other data sources.

FSharp.Data stems from [Types from data Making structured data first-class citizens in F#](http://tomasp.net/academic/papers/fsharp-data/) by Petricek, Syme and Guerra. This paper
received a Distinguished Paper award at PLDI 2016 and was selected as one of three CACM Research
Highlight in 2018. 🏆🏆🏆

The package is available on <a href="https://nuget.org/packages/FSharp.Data">NuGet</a>. [![NuGet Status](//img.shields.io/nuget/v/FSharp.Data.svg?style=flat)](https://www.nuget.org/packages/FSharp.Data/)

## Type Providers

*)
<div class="container-fluid" style="margin:15px 0px 15px 0px;">
    <div class="row-fluid">
        <div class="span1"></div>
        <div class="span10" id="anim-holder">
            <a id="lnk" href="images/json.gif"><img id="anim" src="images/json.gif" /></a>
        </div>
        <div class="span1"></div>
    </div>
</div>
(**
The FSharp.Data type providers for CSV, HTML, JSON and XML infer types from the structure of a sample
document (or a document containing multiple samples). The structure is then used
to provide easy to use type-safe access to documents that follow the same structure.

// can't yet format Span ([DirectLink ([Literal ("CSV Type Provider", Some { StartLine = 36 StartColumn = 3 EndLine = 36 EndColumn = 20 })], "library/CsvProvider.html", None, Some { StartLine = 36 StartColumn = 3 EndLine = 36 EndColumn = 87 }); Literal (" - discusses the ", Some { StartLine = 36 StartColumn = 3 EndLine = 36 EndColumn = 20 }); InlineCode ("CsvProvider<..>", Some { StartLine = 36 StartColumn = 21 EndLine = 36 EndColumn = 86 }); Literal (" type", Some { StartLine = 36 StartColumn = 20 EndLine = 36 EndColumn = 25 })], Some { StartLine = 36 StartColumn = 0 EndLine = 36 EndColumn = 87 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("HTML Type Provider", Some { StartLine = 37 StartColumn = 3 EndLine = 37 EndColumn = 21 })], "library/HtmlProvider.html", None, Some { StartLine = 37 StartColumn = 3 EndLine = 37 EndColumn = 91 }); Literal (" - discusses the ", Some { StartLine = 37 StartColumn = 3 EndLine = 37 EndColumn = 20 }); InlineCode ("HtmlProvider<...>", Some { StartLine = 37 StartColumn = 21 EndLine = 37 EndColumn = 90 }); Literal (" type", Some { StartLine = 37 StartColumn = 20 EndLine = 37 EndColumn = 25 })], Some { StartLine = 36 StartColumn = 0 EndLine = 36 EndColumn = 87 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("JSON Type Provider", Some { StartLine = 38 StartColumn = 3 EndLine = 38 EndColumn = 21 })], "library/JsonProvider.html", None, Some { StartLine = 38 StartColumn = 3 EndLine = 38 EndColumn = 90 }); Literal (" - discusses the ", Some { StartLine = 38 StartColumn = 3 EndLine = 38 EndColumn = 20 }); InlineCode ("JsonProvider<..>", Some { StartLine = 38 StartColumn = 21 EndLine = 38 EndColumn = 89 }); Literal (" type", Some { StartLine = 38 StartColumn = 20 EndLine = 38 EndColumn = 25 })], Some { StartLine = 36 StartColumn = 0 EndLine = 36 EndColumn = 87 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("XML Type Provider", Some { StartLine = 39 StartColumn = 3 EndLine = 40 EndColumn = 20 })], "library/XmlProvider.html", None, Some { StartLine = 39 StartColumn = 3 EndLine = 40 EndColumn = -2 }); Literal (" - discusses the ", Some { StartLine = 39 StartColumn = 3 EndLine = 40 EndColumn = 20 }); InlineCode ("XmlProvider<..>", Some { StartLine = 39 StartColumn = 21 EndLine = 40 EndColumn = -3 }); Literal (" type", Some { StartLine = 39 StartColumn = 20 EndLine = 40 EndColumn = 25 })], Some { StartLine = 36 StartColumn = 0 EndLine = 36 EndColumn = 87 }) to pynb markdown
The package also contains a type provider for accessing data from
[the WorldBank](library/WorldBank.html).

## Data Access Tools

The package contains functionality to simplify data access. In particular, it includes tools for HTTP web requests and
CSV, HTML, and JSON parsers with simple dynamic API. For more information, see the
following topics:

// can't yet format Span ([DirectLink ([Literal ("HTTP Utilities", Some { StartLine = 50 StartColumn = 3 EndLine = 51 EndColumn = 17 })], "library/Http.html", None, Some { StartLine = 50 StartColumn = 3 EndLine = 51 EndColumn = 29 }); Literal (" - discusses the ", Some { StartLine = 50 StartColumn = 3 EndLine = 51 EndColumn = 20 }); InlineCode ("Http", Some { StartLine = 50 StartColumn = 21 EndLine = 51 EndColumn = 28 }); Literal (" type that can be used
to send HTTP web requests.", Some { StartLine = 50 StartColumn = 20 EndLine = 51 EndColumn = 70 })], Some { StartLine = 50 StartColumn = 0 EndLine = 50 EndColumn = 83 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("CSV Parser", Some { StartLine = 52 StartColumn = 3 EndLine = 53 EndColumn = 13 })], "library/CsvFile.html", None, Some { StartLine = 52 StartColumn = 3 EndLine = 53 EndColumn = 36 }); Literal (" - introduces the CSV parser 
(without using the type provider)", Some { StartLine = 52 StartColumn = 3 EndLine = 53 EndColumn = 67 })], Some { StartLine = 50 StartColumn = 0 EndLine = 50 EndColumn = 83 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("HTML Parser", Some { StartLine = 54 StartColumn = 3 EndLine = 55 EndColumn = 14 })], "library/HtmlParser.html", None, Some { StartLine = 54 StartColumn = 3 EndLine = 55 EndColumn = 36 }); Literal (" - introduces the HTML parser 
(without using the type provider)", Some { StartLine = 54 StartColumn = 3 EndLine = 55 EndColumn = 68 })], Some { StartLine = 50 StartColumn = 0 EndLine = 50 EndColumn = 83 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("JSON Parser", Some { StartLine = 56 StartColumn = 3 EndLine = 58 EndColumn = 14 })], "library/JsonValue.html", None, Some { StartLine = 56 StartColumn = 3 EndLine = 58 EndColumn = -2 }); Literal (" - introduces the JSON parser 
(without using the type provider)", Some { StartLine = 56 StartColumn = 3 EndLine = 58 EndColumn = 68 })], Some { StartLine = 50 StartColumn = 0 EndLine = 50 EndColumn = 83 }) to pynb markdown
## Tutorials

The following tutorials contain additional examples that
use multiple features together:

// can't yet format Span ([DirectLink ([Literal ("Converting between JSON and XML", Some { StartLine = 64 StartColumn = 3 EndLine = 66 EndColumn = 34 })], "tutorials/JsonToXml.html", None, Some { StartLine = 64 StartColumn = 3 EndLine = 66 EndColumn = 61 }); Literal (" - implements two serialization 
functions that convert between the standard .NET ", Some { StartLine = 64 StartColumn = 3 EndLine = 66 EndColumn = 86 }); InlineCode ("XElement", Some { StartLine = 64 StartColumn = 87 EndLine = 66 EndColumn = 60 }); Literal (" and the ", Some { StartLine = 64 StartColumn = 86 EndLine = 66 EndColumn = 95 }); InlineCode ("JsonValue", Some { StartLine = 64 StartColumn = 96 EndLine = 66 EndColumn = 60 }); Literal (" from FSharp.Data.
The tutorial demonstrates pattern matching on ", Some { StartLine = 64 StartColumn = 95 EndLine = 66 EndColumn = 161 }); InlineCode ("JsonValue", Some { StartLine = 64 StartColumn = 162 EndLine = 66 EndColumn = 60 }); Literal (".", Some { StartLine = 64 StartColumn = 161 EndLine = 66 EndColumn = 162 })], Some { StartLine = 64 StartColumn = 0 EndLine = 64 EndColumn = 94 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("Anonymizing JSON", Some { StartLine = 67 StartColumn = 3 EndLine = 69 EndColumn = 19 })], "tutorials/JsonAnonymizer.html", None, Some { StartLine = 67 StartColumn = 3 EndLine = 69 EndColumn = -2 }); Literal (" - implements a function to anonymize a ", Some { StartLine = 67 StartColumn = 3 EndLine = 69 EndColumn = 43 }); InlineCode ("JsonValue", Some { StartLine = 67 StartColumn = 44 EndLine = 69 EndColumn = -3 }); Literal (" from FSharp.Data.
The tutorial demonstrates pattern matching on ", Some { StartLine = 67 StartColumn = 43 EndLine = 69 EndColumn = 109 }); InlineCode ("JsonValue", Some { StartLine = 67 StartColumn = 110 EndLine = 69 EndColumn = -3 }); Literal (".", Some { StartLine = 67 StartColumn = 109 EndLine = 69 EndColumn = 110 })], Some { StartLine = 64 StartColumn = 0 EndLine = 64 EndColumn = 94 }) to pynb markdown
Below is a brief practical demonstration of using FSharp.Data:

*)
<div style="padding-left:20px"><iframe src="https://channel9.msdn.com/posts/Understanding-the-World-with-F/player" width="640" height="360" allowFullScreen frameBorder="0"></iframe></div>
(**
## Reference Documentation

There's also [reference documentation](reference) available. Please note that everything under
the `FSharp.Data.Runtime` namespace is not considered as part of the public API and can change without notice.

## Contributing and license

The library is available under Apache 2.0. For more information see the
[License file](https://github.com/fsharp/FSharp.Data/blob/master/LICENSE.md) in the GitHub repository. In summary, this means that you can
use the library for commercial purposes, fork it, and modify it as you wish. FSharp.Data is made possible by the volunteer work [of more than a dozen
contributors](https://github.com/fsharp/FSharp.Data/graphs/contributors) and we're open to
contributions from anyone. If you want to help out but don't know where to start, you
can take one of the [Up-For-Grabs](https://github.com/fsharp/FSharp.Data/issues?labels=up-for-grabs&state=open)
issues, or help to improve the documentation.

The project is hosted on [GitHub](https://github.com/fsharp/FSharp.Data) where you can [report issues](https://github.com/fsharp/FSharp.Data/issues), fork
the project and submit pull requests. If you're adding new public API's, please also
contribute [samples](https://github.com/fsprojects/FSharp.Data/tree/master/docs/) to the docs.

*)

