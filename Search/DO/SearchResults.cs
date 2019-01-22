using System.Collections.Generic;

namespace LuceneNet48Demo.DO
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
}
