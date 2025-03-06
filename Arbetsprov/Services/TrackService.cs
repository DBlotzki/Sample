using Arbetsprov.Repositories;
using CsvHelper;
using System.Globalization;

namespace Arbetsprov.Services
{
	public interface ITrackService
	{
		Task<bool> ExportTracksAsCSV();
	}
	public class TrackService:ITrackService
	{
		private readonly ITrackRepository _trackRepository;

		public TrackService(ITrackRepository trackRepository)
		{
			_trackRepository = trackRepository;
		}

		public async Task<bool> ExportTracksAsCSV()
		{
			try
			{
				var tracks = await _trackRepository.GetTracks().ConfigureAwait(false);
				var fileName = $"./music-{DateTime.Now.ToString("yyyy-MM-dd-HHmm")}.csv";

				using (var writer = new StreamWriter(fileName))
				using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
				{
					csv.WriteRecords(tracks.Select(x => new { Name = x.TrackName, x.Composer, x.Miliseconds, x.Bytes, Album = x.AlbumTitle, x.Artist, x.Genre, x.MediaType }));
				}

				return true;
			}
			catch (Exception ex) 
			{ 
				return false;
			}
		
			
		}
	}
}
