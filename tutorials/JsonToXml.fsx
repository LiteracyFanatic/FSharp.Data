(**
// can't yet format YamlFrontmatter (["category: Tutorials"; "categoryindex: 3"; "index: 2"], Some { StartLine = 2 StartColumn = 0 EndLine = 5 EndColumn = 8 }) to pynb markdown

*)
#r "nuget: FSharp.Data,4.2.8"
(**
# Converting between JSON and XML

[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/fsprojects/FSharp.Data/gh-pages?filepath=tutorials/JsonToXml.ipynb)&emsp;
[![Script](../img/badge-script.svg)](https://fsprojects.github.io/FSharp.Data//tutorials/JsonToXml.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](https://fsprojects.github.io/FSharp.Data//tutorials/JsonToXml.ipynb)

This tutorial shows how to implement convert JSON document (represented using
the [JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html) type discussed in [JSON parser article](JsonValue.html)) to an
XML document (represented as `XElement`) and the other way round.
This functionality is not directly available in the FSharp.Data package, but it can
be very easily implemented by recursively walking over the JSON (or XML) document.

If you want to use the JSON to/from XML conversion in your code, you can copy the
[source from GitHub](https://github.com/fsharp/FSharp.Data/blob/master/docs/content/tutorials/JsonToXml.fsx) and just include it in your project. If you use these
functions often and would like to see them in the FSharp.Data package, please submit
a [feature request](https://github.com/fsharp/FSharp.Data/issues).

## Initialization

We will be using the LINQ to XML API (available in `System.Xml.Linq.dll`) and the
[JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html) which is available in the `FSharp.Data` namespace:

*)
#r "System.Xml.Linq.dll"
open System.Xml.Linq
open FSharp.Data
(**
In this script, we create a conversion that returns an easy to process value, but the
conversion is not reversible (e.g. converting JSON to XML and then back to JSON will
produce a different value).

## Converting XML to JSON

Although XML and JSON are quite similar formats, there is a number of subtle differences.
In particular, XML distinguishes between **attributes** and **child elements**. Moreover,
all XML elements have a name, while JSON arrays or records are anonymous (but records
have named fields). Consider, for example, the following XML:

    [lang=xml]
    <channel version="1.0">
      <title text="Sample input" />
      <item value="First" />
      <item value="Second" />
    </channel>

The JSON that we produce will ignore the top-level element name (`channel`). It produces
a record that contains a unique field for every attribute and a name of a child element.
If an element appears multiple times, it is turned into an array:

    [lang=js]
    { "version": "1.0",
      "title": { "text": "Sample input" },
      "items": [ { "value": "First" },
                 { "value": "Second" } ]  }

As you can see, the `item` element has been automatically pluralized to `items` and the
array contains two record values that consist of the `value` attribute.

The conversion function is a recursive function that takes a `XElement` and produces
[JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html). It builds JSON records (using `JsonValue.Record`) and arrays (using
`JsonValue.Array`). All attribute values are turned into `JsonValue.String` - the
sample does not imlement more sophisticated conversion that would turn numeric
attributes to a corresponding JSON type:

*)
/// Creates a JSON representation of a XML element
let rec fromXml (xml:XElement) =

  // Create a collection of key/value pairs for all attributes
  let attrs =
    [ for attr in xml.Attributes() ->
        (attr.Name.LocalName, JsonValue.String attr.Value) ]

  // Function that turns a collection of XElement values
  // into an array of JsonValue (using fromXml recursively)
  let createArray xelems =
    [| for xelem in xelems -> fromXml xelem |]
    |> JsonValue.Array

  // Group child elements by their name and then turn all single-
  // element groups into a record (recursively) and all multi-
  // element groups into a JSON array using createArray
  let children =
    xml.Elements()
    |> Seq.groupBy (fun x -> x.Name.LocalName)
    |> Seq.map (fun (key, childs) ->
        match Seq.toList childs with
        | [child] -> key, fromXml child
        | children -> key + "s", createArray children )

  // Concatenate elements produced for child elements & attributes
  Array.append (Array.ofList attrs) (Array.ofSeq children)
  |> JsonValue.Record
(**
## Converting JSON to XML

When converting JSON value to XML, we fact the same mismatch. Consider the following JSON value:

    [lang=js]
    { "title" : "Sample input",
      "paging" : { "current": 1 },
      "items" : [ "First", "Second" ] }

The top-level record does not have a name, so our conversion produces a list of `XObject`
values that can be wrapped into an `XElement` by the user (who has to specify the root
name). Record fields that are a primitive value are turned into attributes, while
complex values (array or record) become objects:

    [lang=xml]
    <root title="Sample input">
      <items>
        <item>First</item>
        <item>Second</item>
      </items>
      <paging current="1" />
    </root>

The conversion function is, again, implemented as a recursive function. This time, we use
pattern matching to distinguish between the different possible cases of [JsonValue](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html).
The cases representing a primitive value simply return the value as `obj`, while array
and record construct nested element(s) or attribute:

*)
/// Creates an XML representation of a JSON value (works
/// only when the top-level value is an object or an array)
let toXml(x:JsonValue) =
  // Helper functions for constructing XML
  // attributes and XML elements
  let attr name value =
    XAttribute(XName.Get name, value) :> XObject
  let elem name (value:obj) =
    XElement(XName.Get name, value) :> XObject

  // Inner recursive function that implements the conversion
  let rec toXml = function
    // Primitive values are returned as objects
    | JsonValue.Null -> null
    | JsonValue.Boolean b -> b :> obj
    | JsonValue.Number number -> number :> obj
    | JsonValue.Float number -> number :> obj
    | JsonValue.String s -> s :> obj

    // JSON object becomes a collection of XML
    // attributes (for primitives) or child elements
    | JsonValue.Record properties ->
      properties
      |> Array.map (fun (key, value) ->
          match value with
          | JsonValue.String s -> attr key s
          | JsonValue.Boolean b -> attr key b
          | JsonValue.Number n -> attr key n
          | JsonValue.Float n -> attr key n
          | _ -> elem key (toXml value)) :> obj

    // JSON array is turned into a
    // sequence of <item> elements
    | JsonValue.Array elements ->
        elements |> Array.map (fun item ->
          elem "item" (toXml item)) :> obj

  // Perform the conversion and cast the result to sequence
  // of objects (may fail for unexpected inputs!)
  (toXml x) :?> XObject seq
(**
## Related articles

// can't yet format Span ([Literal ("API Reference: ", Some { StartLine = 6 StartColumn = 3 EndLine = 6 EndColumn = 18 }); DirectLink ([Literal ("JsonValue", Some { StartLine = 6 StartColumn = 19 EndLine = 6 EndColumn = 47 })], "https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-jsonvalue.html", None, Some { StartLine = 6 StartColumn = 19 EndLine = 6 EndColumn = 47 })], Some { StartLine = 6 StartColumn = 0 EndLine = 6 EndColumn = 48 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("JSON Parser", Some { StartLine = 7 StartColumn = 3 EndLine = 8 EndColumn = 14 })], "../library/JsonValue.html", None, Some { StartLine = 7 StartColumn = 3 EndLine = 8 EndColumn = 56 }); Literal (" - a tutorial that introduces
", Some { StartLine = 7 StartColumn = 3 EndLine = 8 EndColumn = 34 }); InlineCode ("JsonValue", Some { StartLine = 7 StartColumn = 35 EndLine = 8 EndColumn = 55 }); Literal (" for working with JSON values dynamically.", Some { StartLine = 7 StartColumn = 34 EndLine = 8 EndColumn = 76 })], Some { StartLine = 6 StartColumn = 0 EndLine = 6 EndColumn = 48 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("JSON Type Provider", Some { StartLine = 9 StartColumn = 3 EndLine = 10 EndColumn = 21 })], "../library/JsonProvider.html", None, Some { StartLine = 9 StartColumn = 3 EndLine = 10 EndColumn = 47 }); Literal (" - discusses F# type provider
that provides type-safe access to JSON data.", Some { StartLine = 9 StartColumn = 3 EndLine = 10 EndColumn = 78 })], Some { StartLine = 6 StartColumn = 0 EndLine = 6 EndColumn = 48 }) to pynb markdown
// can't yet format Span ([DirectLink ([Literal ("XML Type Provider", Some { StartLine = 11 StartColumn = 3 EndLine = 13 EndColumn = 20 })], "../library/XmlProvider.html", None, Some { StartLine = 11 StartColumn = 3 EndLine = 13 EndColumn = -2 }); Literal (" - discusses the F# type provider
that provides type-safe access to XML data.", Some { StartLine = 11 StartColumn = 3 EndLine = 13 EndColumn = 81 })], Some { StartLine = 6 StartColumn = 0 EndLine = 6 EndColumn = 48 }) to pynb markdown
*)

