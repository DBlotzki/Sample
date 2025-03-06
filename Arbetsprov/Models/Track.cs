namespace Arbetsprov.Models
{
	public class Track
	{
        public int TrackId { get; set; }
		public string TrackName { get; set; }
		public string AlbumTitle { get; set; }
		public string MediaType { get; set; }
		public string Genre { get; set; }
		public string Artist { get; set; }
		public string Composer {  get; set; }
		public int Miliseconds { get; set; }
		public int Bytes {  get; set; }
    }
	public class TrackResponse
	{
		public string TrackName { get; set; }
		public string DurationText { get; set; } // Here I would have both mentioned that this is not the optimal thing to return and that presentational parts should be preferably done in the view layer. 
		public string AlbumTitle { get; set; }
		public string Artist { get; set; }
		public string Genre { get; set; }
		
	}
	
}
