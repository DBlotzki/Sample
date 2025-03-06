using Arbetsprov.Models;
using Arbetsprov.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Nodes;

namespace Arbetsprov.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlaylistsController : ControllerBase
	{
		private IPlaylistService _playlistService;

		public PlaylistsController(IPlaylistService playlistService)
		{
			_playlistService = playlistService;
		}

        //I have based this on Id since, specifications don't mention that there shouldn't be 2 or more playlists with the same name. 
        //There is also no restraint in the DB that prevents it from having a more than one playlist with same name. 
        //This is something I would have asked the Product owner about.  
        [HttpGet]
		[Route("/{id}")]
		public async Task<List<TrackResponse>> GetPlaylist(int id) 
		{
			
			var playlist = await _playlistService.GetPlaylistTracks(id).ConfigureAwait(false);

			return playlist;
		}

		//Normally I would add more information and try and respond with either the ID or the entire playlist depending on needs. 
		[HttpPost]
		public async Task<IActionResult> PostPlaylist([FromBody]string name)
		{
			
			var success = await _playlistService.CreateNewPlaylist(name).ConfigureAwait(false);
			if (success)
			{
				return Ok();
			}
			return BadRequest("Unable to create new playlist");
		}

		//Could have been a patch instead if we want to really follow correct REST standards. 
		[HttpPost]
		[Route("/{playlistId}/track")]
		public async Task<IActionResult> AddTrackToPlaylist(int playlistId, [FromBody]int trackId)
		{
			var success =  await _playlistService.AddTrackToPlaylist(playlistId, trackId).ConfigureAwait(false);
			if (success)
			{
				return Ok();
			}
			return BadRequest();
		}

	}
}
