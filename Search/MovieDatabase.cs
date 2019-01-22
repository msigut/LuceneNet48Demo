using System.Collections.Generic;

namespace LuceneNet48Demo
{
	public class Movie
	{
		public string Title { get; set; }
		public string Summary { get; set; }
		public List<string> Actors { get; set; }
		public int Year { get; set; }
		public string Genre { get; internal set; }
	}

	public static class MovieDatabase
	{
		public static List<Movie> Database { get; private set; } = new List<Movie>();

		static MovieDatabase()
		{
			Database.Add(new Movie
			{
				Actors = new List<string> { "Laurence Fishburne", "Keanu Reeves", "Carrie-Anne Moss" },
				Summary = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
				Title = "The Matrix",
				Year = 1999,
				Genre = "scifi"
			});

			Database.Add(new Movie
			{
				Actors = new List<string> { "Arnold Schwarzenegger", "Linda Hamilton", "Michael Biehn" },
				Summary = "A human-looking indestructible cyborg from the future is sent in the past to assassinate a waitress, whose unborn son will lead humanity in a war against the machines, while a soldier from that war is sent to protect her at all costs.",
				Title = "The Terminator",
				Year = 1984,
				Genre = "scifi"
			});

			Database.Add(new Movie
			{
				Actors = new List<string> { "Tom Hanks", "Robin Wright" },
				Summary = "Forrest Gump, while not intelligent, has accidentally been present at many historic moments, but his true love, Jenny Curran, eludes him.",
				Title = "Forrest Gump",
				Year = 1994,
				Genre = "comedy"
			});

			Database.Add(new Movie
			{
				Actors = new List<string> { "Russel Crowe", "Joaquin Phoenix", "Connie Nielsen" },
				Summary = "When a Roman general is betrayed and his family murdered by an emperor's corrupt son, he comes to Rome as a gladiator to seek revenge.",
				Title = "Gladiator",
				Year = 2000,
				Genre = "action"
			});

			Database.Add(new Movie
			{
				Actors = new List<string> { "Arnold Schwarzenegger", "Danny DeVito", "Kelly Preston" },
				Summary = "A physically perfect but innocent man goes in search of his long-lost twin brother, who is a short small-time crook.",
				Title = "Twins",
				Year = 1988,
				Genre = "comedy"
			});

			Database.Add(new Movie
			{
				Actors = new List<string> { "Leonardo DiCaprio", "Jonah Hill", "Margot Robbie" },
				Summary = "Based on the true story of Jordan Belfort, from his rise to a wealthy stock-broker living the high life to his fall involving crime, corruption and the federal government.",
				Title = "The Wolf of Wall Street",
				Year = 2013,
				Genre = "comedy"
			});

			Database.Add(new Movie
			{
				Actors = new List<string> { "Arnold Schwarzenegger", "Jason Clarke", "Emilia Clarke" },
				Summary = "When John Connor, leader of the human resistance, sends Sgt. Kyle Reese back to 1984 to protect Sarah Connor and safeguard the future, an unexpected turn of events creates a fractured timeline.",
				Title = "Terminator Genisys",
				Year = 2015,
				Genre = "action"
			});
		}
	}
}
