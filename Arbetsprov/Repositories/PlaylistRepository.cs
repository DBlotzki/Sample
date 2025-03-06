using Arbetsprov.Models;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Arbetsprov.Repositories
{
	public interface IPlaylistRepository
	{
		Task<bool> AddNewPlaylist(string name);
		Task<List<Track>> GetPlaylistTracks(int playlistId);
		Task<bool> AddTrackToPlaylist(int playlistId, int trackId);

	}
	public class PlaylistRepository: IPlaylistRepository
	{
		private IConfiguration Configuration { get; }
        public PlaylistRepository(IConfiguration configuration)
        {
           Configuration = configuration;
        }

		//Here we have the issue with PlaylistId not being an Identity in the SQL Database, we actually need to make sure to also use a select in the insert and expose us for a risk of concurrency issue. 
		//We could use lock etc but since it's just a time consuming sample work I will not be adding this at this time. 
		//Quickest and best solution would have been using Identity property on the PlaylistId in the DB. 
        public async Task<bool> AddNewPlaylist(string name)
		{
			using(var connection = new SqlConnection(Configuration["ConnectionStrings:MusicDatabase"]))
			{
				var sqlCommand = @"INSERT INTO Playlist (PlaylistId, Name)  SELECT MAX(PlaylistId)+1, @name FROM Playlist";

				var parameters = new DynamicParameters();
				parameters.Add("name", name);

				var result = await connection.ExecuteAsync(sqlCommand, parameters, commandType: CommandType.Text).ConfigureAwait(false);
				//We just want it to insert 1 row and nothing else. 
				if(result == 1)
				{
					return true;
				}
				return false;
			}
		}


		//Not always the most efficient but time effective to make fewer queries and repository methods. 
		public async Task<List<Track>> GetPlaylistTracks(int playlistId)
		{
		
				using (var connection = new SqlConnection(Configuration["ConnectionStrings:MusicDatabase"]))
				{
					var query = @" Select 
									track.TrackId AS TrackId,
									track.Name AS TrackName,
									Album.Title AS AlbumTitle,
									MediaType.Name AS MediaType,
									Genre.Name AS Ganre,
									Artist.Name AS Artist,
									track.Composer AS Composer,
									track.Milliseconds AS Milliseconds,
									track.Bytes AS Bytes
									
									 from Track 
									inner join Album on Album.AlbumId = track.AlbumId
									inner join Artist on Artist.ArtistId = Album.AlbumId
									inner join Genre on Genre.GenreId = track.GenreId
									inner join MediaType on MediaType.MediaTypeId = track.MediaTypeId
									inner join PlaylistTrack on PlaylistTrack.TrackId = track.TrackId
									where PlaylistTrack.PlaylistId = @playlistId";

					var parameters = new DynamicParameters();
					parameters.Add("playlistId", playlistId);
			
					return (await connection.QueryAsync<Track>(query, parameters).ConfigureAwait(false)).ToList();
				

					
				}
			
		}


		public async Task<bool> AddTrackToPlaylist(int playlistId,int trackId)
		{
			using (var connection = new SqlConnection(Configuration["ConnectionStrings:MusicDatabase"]))
			{

				var sqlCommand = @"  insert into PlaylistTrack (PlaylistId, TrackId) @playlistId, @trackId";

				var parameters = new DynamicParameters();
				parameters.Add("playlistId", playlistId);
				parameters.Add("trackId", trackId);

				var result = await connection.ExecuteAsync(sqlCommand, parameters, commandType: CommandType.Text).ConfigureAwait(false);
				//We just want it to insert 1 row and nothing else. 
				if (result == 1)
				{
					return true;
				}
				return false;
			}

		}


	}

}
