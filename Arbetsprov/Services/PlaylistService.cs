
using Arbetsprov.Models;
using Arbetsprov.Models.Exceptions;
using Arbetsprov.Repositories;
using System.Text;

namespace Arbetsprov.Services
{
	//Interfaces is not always needed but I like having them since I can easily see what the class functionalities are but also helps a lot for Unit tests and mocking data. 
	public interface IPlaylistService
	{
		Task<List<TrackResponse>> GetPlaylistTracks(int playlistId);
		Task<bool> CreateNewPlaylist(string name);
		Task<bool> AddTrackToPlaylist(int playlistId, int trackId);
	}
	public class PlaylistService: IPlaylistService
	{
		private readonly IPlaylistRepository _playlistRepository;
		private readonly ITrackRepository _trackRepository;

		public PlaylistService(ITrackRepository trackRepository, IPlaylistRepository playlistRepository)
		{
			_playlistRepository = playlistRepository;
			_trackRepository = trackRepository;
		}

		public async Task<List<TrackResponse>> GetPlaylistTracks(int playlistId)
		{

			var tracks = await _playlistRepository.GetPlaylistTracks(playlistId).ConfigureAwait(false);
			if(tracks == null)
			{
				throw new NotFoundException($"Could not find playlist with Id {playlistId}");
			}
			return CreateResponse(tracks.ToList());
		}


		public async Task<bool> CreateNewPlaylist(string name)
		{
			//Here I would add and combine different functionality if we required it. Or if we would have solved the database issue by first fetching the highest ID to then include it in the next method. 
			return	await _playlistRepository.AddNewPlaylist(name).ConfigureAwait(false); 

		}

		//We need to be sure both the playlist and the track exist but not in the chosen playlist before we add it. 
		public async Task<bool> AddTrackToPlaylist(int playlistId, int trackId)
		{
			var playlist = await _playlistRepository.GetPlaylistTracks(playlistId);
			var track = await _trackRepository.GetTrack(trackId);

			if(playlist == null || track == null || playlist.Any(x=>x.TrackId == trackId)) 
			{
				throw new Exception($"Could not find playlist with Id {playlistId} or track already exists on playlist"); //I would normaly do this as a normal response but I also wanted to showcase the functionality with middlewares, since I like them. 
			}

			return await _playlistRepository.AddTrackToPlaylist(playlistId, trackId).ConfigureAwait(false);

		}

		private List<TrackResponse> CreateResponse(List<Track> tracks)
		{
			var response = new List<TrackResponse>();
			foreach (var track in tracks)
			{
				response.Add(new TrackResponse
				{
					TrackName = track.TrackName,
					DurationText = CreateTrackDurationText(track.Miliseconds),
					AlbumTitle = track.AlbumTitle,
					Artist = track.Artist,
					Genre = track.Genre,
				});

			}
			return response;
		}

		private string CreateTrackDurationText(int millisecond)
		{

			var time = TimeSpan.FromMilliseconds(millisecond).ToString(@"mm:ss");
			return $"({time}s)";
		}
	}

	
}
