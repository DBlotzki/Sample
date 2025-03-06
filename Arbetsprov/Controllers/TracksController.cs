using Arbetsprov.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arbetsprov.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TracksController : ControllerBase
	{
		private ITrackService _trackService;

		public TracksController(ITrackService trackService)
		{
			_trackService = trackService;
		}

		[HttpGet]
		[Route("/Export")]
		public async Task<IActionResult> ExportTracks()
		{
			var success = await _trackService.ExportTracksAsCSV().ConfigureAwait(false);
			if (success)
			{
				return Ok();
			}
			return BadRequest("Unable to save csv file"); //If I had time I would have worked more on the respone side here.

		}
	}
}
