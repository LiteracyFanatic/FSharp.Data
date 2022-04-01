(**
// can't yet format YamlFrontmatter (["category: Utilities"; "categoryindex: 1"; "index: 3"], Some { StartLine = 2 StartColumn = 0 EndLine = 5 EndColumn = 8 }) to pynb markdown

*)
#r "nuget: FSharp.Data,4.2.8"
(**
[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/fsprojects/FSharp.Data/gh-pages?filepath=library/HtmlParser.ipynb)&emsp;
[![Script](../img/badge-script.svg)](https://fsprojects.github.io/FSharp.Data//library/HtmlParser.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](https://fsprojects.github.io/FSharp.Data//library/HtmlParser.ipynb)

# HTML Parser

This article demonstrates how to use the HTML Parser to parse HTML files.

The HTML parser takes any fragment of HTML, uri or a stream and trys to parse it into a DOM.
The parser is based on the [HTML Living Standard](http://www.whatwg.org/specs/web-apps/current-work/multipage/index.html#contents)
Once a document/fragment has been parsed, a set of extension methods over the HTML DOM elements allow you to extract information from a web page
independently of the actual HTML Type provider.

*)
open FSharp.Data
(**
The following example uses Google to search for `FSharp.Data` then parses the first set of
search results from the page, extracting the URL and Title of the link.
We use the [HtmlDocument](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-htmldocument.html) type.

To achieve this we must first parse the webpage into our DOM. We can do this using
the [HtmlDocument.Load](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-htmldocument.html) method. This method will take a URL and make a synchronous web call
to extract the data from the page. Note: an asynchronous variant [HtmlDocument.AsyncLoad](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-htmldocument.html) is also available

*)
let results = HtmlDocument.Load("http://www.google.co.uk/search?q=FSharp.Data")(* output: 
val results : HtmlDocument =
  <!-- html>--><html lang="en">
  <head>
    <meta charset="UTF-8" /><meta content="/images/branding/googleg/1x/googleg_standard_color_128dp.png" itemprop="image" /><title>FSharp.Data - Google Search</title><script nonce="+pjjo4RhqN939LcCNXTbqQ==">(function(){
document.documentElement.addEventListener("submit",function(b){var a;if(a=b.target){var c=a.getAttribute("data-submitfalse");a="1"===c||"q"===c&&!a.elements.q.value?!0:!1}else a=!1;a&&(b.preventDefault(),b.stopPropagation())},!0);document.documentE...*)
(**
Now that we have a loaded HTML document we can begin to extract data from it.
Firstly we want to extract all of the anchor tags `a` out of the document, then
inspect the links to see if it has a `href` attribute, using [HtmlDocumentExtensions.Descendants](https://fsprojects.github.io/FSharp.Data/reference/fsharp-data-htmldocumentextensions.html). If it does, extract the value,
which in this case is the url that the search result is pointing to, and additionally the
`InnerText` of the anchor tag to provide the name of the web page for the search result
we are looking at.

*)
let links =
    results.Descendants ["a"]
    |> Seq.choose (fun x ->
           x.TryGetAttribute("href")
           |> Option.map (fun a -> x.InnerText(), a.Value())
    )
    |> Seq.truncate 10
    |> Seq.toList(* output: 
val links : (string * string) list =
  [("Google", "/?sa=X&ved=0ahUKEwiC6_G6svP2AhWFknIEHf9SCw8QOwgC");
   ("Google",
    "/?output=search&ie=UTF-8&sa=X&ved=0ahUKEwiC6_G6svP2AhWFknIEHf9SCw8QPAgE");
   ("here", "/search?q=FSharp.Data&ie=UTF-8&gbv=1&sei=BjdHYoLQFIWlytMP_6WteA");
   ("News",
    "/search?q=FSharp.Data&ie=UTF-8&source=lnms&tbm=nws&sa=X&ved=0"+[40 chars]);
   ("Videos",
    "/search?q=FSharp.Data&ie=UTF-8&source=lnms&tbm=vid&sa=X&ved=0"+[40 chars]);
   ("Images",
    "/search?q=FSharp.Data&ie=UTF-8&source=lnms&tbm=isch&sa=X&ved="+[41 chars]);
   ("Maps",
    "http://maps.google.co.uk/maps?q=FSharp.Data&um=1&ie=UTF-8&sa="+[47 chars]);
   ("Shopping",
    "/search?q=FSharp.Data&ie=UTF-8&source=lnms&tbm=shop&sa=X&ved="+[41 chars]);
   ("Books",
    "/search?q=FSharp.Data&ie=UTF-8&source=lnms&tbm=bks&sa=X&ved=0"+[40 chars]);
   ("Search tools", "/advanced_search")]*)
(**
Now that we have extracted our search results you will notice that there are lots of
other links to various Google services and cached/similar results. Ideally we would
like to filter these results as we are probably not interested in them.
At this point we simply have a sequence of Tuples, so F# makes this trivial using `Seq.filter`
and `Seq.map`.

*)
let searchResults =
    links
    |> List.filter (fun (name, url) ->
                    name <> "Cached" && name <> "Similar" && url.StartsWith("/url?"))
    |> List.map (fun (name, url) -> name, url.Substring(0, url.IndexOf("&sa=")).Replace("/url?q=", ""))(* output: 
val searchResults : (string * string) list = []*)

