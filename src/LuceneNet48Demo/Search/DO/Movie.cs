using System.Collections.Generic;

namespace LuceneNet48Demo.DO
{
	/// <summary>
	/// movie
	/// </summary>
	public class Movie
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public List<string> Actors { get; set; }
		public int Year { get; set; }
		public string Genre { get; internal set; }
	}
}
