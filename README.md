## LuceneNet48Demo

A demo of using [Lucene.NET 4.8](https://github.com/apache/lucenenet) for .NET Core 2+ environment. 

What's inside:

* [AliasMultiFieldQueryParser](/Search/AliasMultiFieldQueryParser.cs) - Multifield query parser with alias support
```c#
// make alias for fields: "t" -> "title"; "s" -> "summary"
_queryParser = new AliasMultiFieldQueryParser(MATCH_LUCENE_VERSION, null, _analyzer,
	new Dictionary<string, string>()
	{
		{ "t", "title" },
		{ "s", "summary" }
	});
	
// (query: title:term summary:cyborg)
result = search.Search("t:term s:cyborg");
```
* [EnhancedEnglishAnalyzer](/Search/EnhancedEnglishAnalyzer.cs) - Enhanced English Lucene analyzer (based on article: [Tuning Lucene to Get the Most Relevant Results](https://blog.swwomm.com/2013/07/tuning-lucene-to-get-most-relevant.html) and [Search with Lucene.Net 4.8 (Part 2) - A Few Tricks and Tips](http://programagic.ca/blog/rest-api-lucenenet-part-2-a-few-tricks-and-tips))

```
Original text: My friends are visiting Montréal's engineering institutions
->
Tokens from analyzers: friend visit montreal engin institut
```
* using **SearcherManager** + `MaybeRefreshBlocking()`, `Acquire()` and `Release()`
* using **UpdateDocument()** + `new Term(keyField, ...)` (based on article: [Lucene .NET Update data](https://stackoverflow.com/questions/26094224/lucene-net-update-data))
* use example data **MovieDatabase** + `int Id` (based on code: [r15h1/lucenedemo](https://github.com/r15h1/lucenedemo))

Packages:
* NuGet: [Lucene.Net 4.8.0-beta00005](https://www.nuget.org/packages/Lucene.Net/4.8.0-beta00005)
* NuGet: [Lucene.Net.QueryParser 4.8.0-beta00005](https://www.nuget.org/packages/Lucene.Net.QueryParser/4.8.0-beta00005)
