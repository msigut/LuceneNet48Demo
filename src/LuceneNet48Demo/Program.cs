using System;
using System.IO;

namespace LuceneNet48Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			var search = new MySearch(Path.Combine(Environment.CurrentDirectory, "index"));

			// 1. index first
			search.Index();

			// 2. search (query: title:term summary:cyborg)
			var result = search.Search("t:term s:cyborg");

			// 3. show results
			Console.WriteLine($"Query: '{result.Query}' in {result.Time}, total: {result.TotalHits}");
			foreach (var item in result.Hits)
			{
				Console.WriteLine($"#{item.Id}: {item.Title} ({item.Year}) {item.Score}");
			}

			// 4. work on index ...
			search.WorkOnIndex();
		}
	}
}
