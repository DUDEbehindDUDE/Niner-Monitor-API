using System.Text.Json.Serialization;

namespace OccupancyTracker.Models
{
	public class AdkinsOccupancyResponse
	{
		[JsonPropertyName("atkins_current_occupancy")]
		public required string Occupancy { get; set; }
	}
}
