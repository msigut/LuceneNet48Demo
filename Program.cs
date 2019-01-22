using System;
using System.IO;

namespace LuceneNet48Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			var query = "t:term s:cyborg";
			var search = new MySearch(Path.Combine(Environment.CurrentDirectory, "index"));

			// 1. index first
			search.Index();

			// 2. search
			var result = search.Search(query);

			// 3. show results
			Console.WriteLine($"Query: '{query}' in {result.Time}, total: {result.TotalHits}");
			foreach (var item in result.Hits)
			{
				Console.WriteLine($"{item.Title} ({item.Year})");
			}
		}
	}
}
