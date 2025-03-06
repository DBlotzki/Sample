namespace Arbetsprov.Models
{
	public class Artist
	{
		public string ArtistName { get; set; }
		public List<string> UniqueGenres { get; set; }
	}

	//Will allow me to do fewer calls to the DB and save some time. In larger projects I would argue to split models in more subfolders, like for example DataModels and maybe Response Models. 
	public class ArtistGenre
	{
		public int ArtistId { get; set; }
		public string ArtistName { get; set; }
		public string GenreName { get; set; }
	}
}
