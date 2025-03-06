using Arbetsprov.Models;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Arbetsprov.Repositories
{
	public interface ITrackRepository
	{
		Task<Track?> GetTrack(int trackId);
		Task<IEnumerable<Track>> GetTracks();
	}
	public class TrackRepository : ITrackRepository
	{
		private IConfiguration Configuration { get; }
		public TrackRepository(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public async Task<Track?> GetTrack(int trackId)
		{
			using (var connection = new SqlConnection(Configuration["ConnectionStrings:MusicDatabase"]))
			{
				var query = @"select * from track where trackId = @trackId";

				var parameters = new DynamicParameters();
				parameters.Add("trackId", trackId);

				return await connection.QueryFirstOrDefaultAsync<Track>(query, parameters, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}
		public async Task<IEnumerable<Track>> GetTracks()
		{
			using (var connection = new SqlConnection(Configuration["ConnectionStrings:MusicDatabase"]))
			{
				var query = @"SELECT 
									track.TrackId AS TrackId,
									track.Name AS TrackName,
									Album.Title AS AlbumTitle,
									MediaType.Name AS MediaType,
									Genre.Name AS Genre,
									Artist.Name AS Artist,
									track.Composer AS Composer,
									track.Milliseconds AS Milliseconds,
									track.Bytes AS Bytes
								FROM Track 
									INNER JOIN Album on Album.AlbumId = track.AlbumId
									INNER JOIN Artist on Artist.ArtistId = Album.AlbumId
									INNER JOIN Genre on Genre.GenreId = track.GenreId
									INNER JOIN MediaType on MediaType.MediaTypeId = track.MediaTypeId";

				return await connection.QueryAsync<Track>(query, commandType: CommandType.Text).ConfigureAwait(false);
			}

		}
	
	}
}