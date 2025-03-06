using Arbetsprov.Models;
using Arbetsprov.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Arbetsprov.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArtistsController : ControllerBase
	{
		private IArtistService _artistService;

		public ArtistsController(IArtistService artistService)
		{
			_artistService = artistService;
		}

		[HttpGet]
		public async Task<List<Artist>> GetArtists()
		{
			return await _artistService.GetArtists().ConfigureAwait(false);
			
		}

	}
}
