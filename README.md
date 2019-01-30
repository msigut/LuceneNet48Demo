## LuceneNet48Demo

A demo of using [Apache Lucene.NET 4.8](https://github.com/apache/lucenenet) for .NET Core 2+ environment. 

What's inside:

* [AliasMultiFieldQueryParser](/src/LuceneNet48Demo/Search/AliasMultiFieldQueryParser.cs) - Multifield query parser with alias support
```c#
// make alias for fields: "t" -> "title"; "s" -> "summary"
_queryParser = new AliasMultiFieldQueryParser(MATCH_LUCENE_VERSION, new[] { "title", "sumary" }, _analyzer,
	new Dictionary<string, string>()
	{
		{ "t", "title" },
		{ "s", "summary" }
	});
	
// (query: title:term summary:cyborg)
result = search.Search("t:term s:cyborg");
```
* [EnhEnglishAnalyzer](/src/LuceneNet48Demo/Search/EnhEnglishAnalyzer.cs) - Enhanced English Lucene analyzer (based on article: [Tuning Lucene to Get the Most Relevant Results](https://blog.swwomm.com/2013/07/tuning-lucene-to-get-most-relevant.html) and [Search with Lucene.Net 4.8 (Part 2) - A Few Tricks and Tips](http://programagic.ca/blog/rest-api-lucenenet-part-2-a-few-tricks-and-tips))

```
Original text: My friends are visiting Montréal's engineering institutions
->
Tokens from analyzers: friend visit montreal engin institut
```

* [MultiFieldAnalyzerWrapper](/src/LuceneNet48Demo/Search/MultiFieldAnalyzerWrapper.cs) - For assign multiple **fields** to Analyzer + one as **default** for all other fields
```c#
_analyzer = new MultiFieldAnalyzerWrapper(
	defaultAnalyzer: new EnhEnglishAnalyzer(MATCH_LUCENE_VERSION, true),
	new[]
	{
		(
			// analyzer for fields: "genre", "year"
			new[] { "genre", "year" },
			Analyzer.NewAnonymous(createComponents: (fieldName, reader) =>
			{
				var source = new KeywordTokenizer(reader);
				TokenStream result = new ASCIIFoldingFilter(source);
				result = new LowerCaseFilter(MATCH_LUCENE_VERSION, result);
				return new TokenStreamComponents(source, result);
			})
		)
	});
```

* [ForEachTermDocs](/src/LuceneNet48Demo/Search/Extensions.cs) - For get documents by **Term**, only selected **fields** to work with
```c#
// II. use term & selected fields
_writer.ForEachTermDocs(new Term("year", "1194"), new[] { "title" }, d =>
 {
	 var title = d.GetField("title").GetStringValue();
	 // do something ...
 });
```

* using **SearcherManager** + `MaybeRefreshBlocking()`, `Acquire()` and `Release()`
* using **UpdateDocument()** + `new Term(keyField, ...)` (based on article: [Lucene .NET Update data](https://stackoverflow.com/questions/26094224/lucene-net-update-data))
* using **DeleteDocuments** + `MaybeRefreshBlocking()` + `new Term(keyField, ...)` (for solving issue: [C# Lucene.Net IndexWriter.DeleteDocuments not working](https://stackoverflow.com/questions/44181550/c-sharp-lucene-net-indexwriter-deletedocuments-not-working/54336227#54336227)
* to check Analyzer work use **PrintTokens()** method to log all tokens per `field`
* use example data **MovieDatabase** + `int Id` (based on code: [r15h1/lucenedemo](https://github.com/r15h1/lucenedemo))

Packages:
* NuGet: [Apache Lucene.Net 4.8.0-beta00005](https://www.nuget.org/packages/Lucene.Net/4.8.0-beta00005)
* NuGet: [Apache Lucene.Net.QueryParser 4.8.0-beta00005](https://www.nuget.org/packages/Lucene.Net.QueryParser/4.8.0-beta00005)
