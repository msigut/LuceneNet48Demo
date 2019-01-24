using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using LuceneNet48Demo.DO;

namespace LuceneNet48Demo
{
	/// <summary>
	/// lucene.net based search
	/// </summary>
	public class MySearch
	{
		private const LuceneVersion MATCH_LUCENE_VERSION = LuceneVersion.LUCENE_48;

		private readonly IndexWriter _writer;
		private readonly SearcherManager _searchManager;
		private readonly Analyzer _analyzer;
		private readonly QueryParser _queryParser;

		public MySearch(string indexPath)
		{
			_analyzer = new EnhEnglishAnalyzer(MATCH_LUCENE_VERSION);
			_writer = new IndexWriter(FSDirectory.Open(indexPath), new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer));

			_searchManager = new SearcherManager(_writer, true, null);

			// make alias for fields: "t" -> "title"; "s" -> "summary"
			_queryParser = new AliasMultiFieldQueryParser(MATCH_LUCENE_VERSION, new[] { "title", "sumary" }, _analyzer,
				new Dictionary<string, string>()
				{
					{ "t", "title" },
					{ "s", "summary" }
				});
		}

		/// <summary>
		/// create index
		/// </summary>
		public void Index()
		{
			const string keyField = "id";

			foreach (var movie in MovieDatabase.Database)
			{
				// prapare new document
				var doc = new Document
				{
					new StringField(keyField, $"{movie.Id}", Field.Store.YES),
					new TextField("title", movie.Title, Field.Store.YES),
					new TextField("summary", movie.Summary, Field.Store.YES),
					new StringField("genre", movie.Genre, Field.Store.YES),
					new Int32Field("year", movie.Year, Field.Store.YES),
				};
				foreach (var actor in movie.Actors)
				{
					doc.Add(new TextField("actor", actor, Field.Store.YES));
				}

				// https://stackoverflow.com/questions/26094224/lucene-net-update-data
				// Where in Term constructor field "ID" is my unique field with no index flag and Value
				// is text of old value field "ID" in old document in index.
				_writer.UpdateDocument(new Term(keyField, doc.GetField(keyField).GetStringValue()), doc);

				// for debug Analyzer work use this ...
				// 
				PrintTokens(_writer.Analyzer, "title", movie.Title);
			}

			// https://stackoverflow.com/questions/44181550/c-sharp-lucene-net-indexwriter-deletedocuments-not-working/54336227#54336227
			// Delete last movie, check DeleteDocuments() principle
			_writer.DeleteDocuments(new Term(keyField, "7"));
			
			_writer.Flush(true, true);
			_writer.Commit();
		}

		/// <summary>
		/// search index
		/// </summary>
		public SearchResults Search(string query)
		{
			// Execute the search with a fresh indexSearcher
			_searchManager.MaybeRefreshBlocking();

			var searcher = _searchManager.Acquire();
			try
			{
				var q = _queryParser.Parse(query);

				var topDocs = searcher.Search(q, 10);

				var result = new SearchResults()
				{
					Query = query,
					TotalHits = topDocs.TotalHits,
				};

				foreach (var scoreDoc in topDocs.ScoreDocs)
				{
					var document = searcher.Doc(scoreDoc.Doc);

					var hit = new MovieHit
					{
						Id = int.Parse(document.GetField("id")?.GetStringValue()),
						Title = document.GetField("title")?.GetStringValue(),
						Summary = document.GetField("summary")?.GetStringValue(),
						Genre = document.GetField("genre")?.GetStringValue(),
						Year = document.GetField("year")?.GetInt32Value(),

						// Results are automatically sorted by relevance
						Score = scoreDoc.Score,
					};

					result.Hits.Add(hit);
				}

				return result;
			}
			finally
			{
				_searchManager.Release(searcher);
				searcher = null;
			}
		}

		/// <summary>
		/// reading index document by document
		/// </summary>
		public void WorkOnIndex()
		{
			var reader = _writer.GetReader(true);

			for (var x = 0; x < reader.NumDocs; x++)
			{
				var doc = reader.Document(x);
				// do something ...
			}
		}

		/// <summary>
		/// print tokens after field analyze
		/// </summary>
		protected void PrintTokens(Analyzer analyzer, string fieldName, string text)
		{
			if (analyzer == null)
				throw new ArgumentNullException(nameof(analyzer));

			var tokenStream = analyzer.GetTokenStream(fieldName, text);
			var termAttr = tokenStream.GetAttribute<ICharTermAttribute>();

			tokenStream.Reset();

			var str = new StringBuilder();
			while (tokenStream.IncrementToken())
			{
				if (str.Length > 0)
				{
					str.Append(" ");
				}
				str.Append(termAttr.ToString());
			}
			Console.WriteLine($"field: {fieldName} '{text}' -> '{str.ToString()}'");

			tokenStream.Reset();
		}
	}
}
