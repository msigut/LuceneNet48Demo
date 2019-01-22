using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.En;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace LuceneNet48Demo
{
	/// <summary>
	/// results after executing a search on the movie index
	/// </summary>
	public class SearchResults
	{
		public string Time { get; set; }
		public int TotalHits { get; set; }
		public IList<MovieHit> Hits { get; set; }
		public SearchResults() => Hits = new List<MovieHit>();
	}

	/// <summary>
	/// a representation of a movie item
	/// </summary>
	public class MovieHit : Movie
	{
		public new int? Year { get; set; }
		public float Score { get; set; }
	}

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
			_analyzer = new EnhancedEnglishAnalyzer(MATCH_LUCENE_VERSION, EnglishAnalyzer.DefaultStopSet);
			_writer = new IndexWriter(FSDirectory.Open(indexPath), new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer)
			{
				OpenMode = OpenMode.CREATE
			});

			_searchManager = new SearcherManager(_writer, true, null);
			_queryParser = new AliasMultiFieldQueryParser(MATCH_LUCENE_VERSION, null, _analyzer,
				new Dictionary<string, string>()
				{
					{ "t", "title" },
					{ "s", "summary" }
				});
		}

		public void Index()
		{
			foreach (var movie in MovieDatabase.Database)
			{
				_writer.UpdateDocument(new Term("url", $"http://movies.com/{movie.Title.Replace(" ", "-")}"), GetDocument(movie));
			}

			_writer.Flush(true, true);
			_writer.Commit();
		}

		private Document GetDocument(Movie movie)
		{
			var document = new Document
			{
				new TextField("title", movie.Title, Field.Store.YES),
				new TextField("summary", movie.Summary, Field.Store.YES),
				new StringField("genre", movie.Genre, Field.Store.YES),
				new Int32Field("year", movie.Year, Field.Store.YES),
			};

			foreach (var actor in movie.Actors)
			{
				document.Add(new TextField("actor", actor, Field.Store.YES));
			}

			return document;
		}

		public SearchResults Search(string query)
		{
			// Execute the search with a fresh indexSearcher
			_searchManager.MaybeRefreshBlocking();

			var searcher = _searchManager.Acquire();
			try
			{
				var q = _queryParser.Parse(query);

				var topdDocs = searcher.Search(q, 10);

				var result = new SearchResults()
				{
					TotalHits = topdDocs.TotalHits
				};

				foreach (var scoreDoc in topdDocs.ScoreDocs)
				{
					var document = searcher.Doc(scoreDoc.Doc);

					var hit = new MovieHit
					{
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
	}
}
