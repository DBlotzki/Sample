using Arbetsprov.Models;
using Arbetsprov.Repositories;

namespace Arbetsprov.Services
{
	public interface IArtistService
	{
		Task<List<Artist>> GetArtists();
	}
	public class ArtistService : IArtistService
	{
		private readonly IArtistRepository _artistRepository;

		public ArtistService(IArtistRepository artistRepository)
		{
			_artistRepository = artistRepository;
		}


		//Could have been done on fewer lines or itterations but this kind of helps with readability too. 
		public async Task<List<Artist>> GetArtists()
		{
			var artistList = new List<Artist>();
			
			var artistGenre = await _artistRepository.GetAllArtistsGenres().ConfigureAwait(false);

			var artistIds = artistGenre.Select(x => x.ArtistId).ToList();

			foreach (var artistId in artistIds)
			{
				var artist = new Artist()
				{
					ArtistName = artistGenre.Where(x=>x.ArtistId == artistId).First().ArtistName,
					UniqueGenres = artistGenre.Where(x=>x.ArtistId == artistId).Select(x=>x.GenreName).ToList(),
				};
				artistList.Add(artist);
			}

			return artistList;
		}

	}
}
