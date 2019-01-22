namespace LuceneNet48Demo.DO
{
	/// <summary>
	/// a representation of a movie in results
	/// </summary>
	public class MovieHit : Movie
	{
		public new int? Year { get; set; }
		public float Score { get; set; }
	}
}
