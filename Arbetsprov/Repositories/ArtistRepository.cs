using Arbetsprov.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Arbetsprov.Repositories
{
	public interface IArtistRepository
	{
		Task<List<ArtistGenre>> GetAllArtistsGenres();
	}

	public class ArtistRepository : IArtistRepository
	{
		private IConfiguration Configuration { get; }

		public ArtistRepository(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public async Task<List<ArtistGenre>> GetAllArtistsGenres()
		{
			using (var connection = new SqlConnection(Configuration["ConnectionStrings:MusicDatabase"]))
			{
				var query = @"SELECT 
									Artist.ArtistId AS ArtistId, 
									Artist.Name AS ArtistName, 
									Genre.Name AS GenreName 
								FROM Artist
									INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
									INNER JOIN Track ON Track.AlbumId = Album.AlbumId
									INNER JOIN Genre ON Genre.GenreId = Track.GenreId
								GROUP BY  Artist.Name, Genre.Name, Artist.ArtistId 
								ORDER BY Artist.ArtistId  ";

				return (await connection.QueryAsync<ArtistGenre>(query).ConfigureAwait(false)).ToList();

			}
		}

	}
}
