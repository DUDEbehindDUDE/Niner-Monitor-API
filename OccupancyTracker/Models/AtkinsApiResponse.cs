using System.Text.Json.Serialization;

namespace OccupancyTracker.Models
{
	public class AtkinsOccupancyResponse
	{
		[JsonPropertyName("atkins_current_occupancy")]
		public required string Occupancy { get; set; }
	}

	public class AtkinsHoursResponse
	{
		[JsonPropertyName("success")]
		public bool Success { get; set; }

		[JsonPropertyName("data")]
		public Dictionary<string, AtkinsHoursResponseData>? Data { get; set; }
	}

	public class AtkinsHoursResponseData
	{
		[JsonPropertyName("location")]
		public string? Location { get; set; }

		[JsonPropertyName("id")]
		public string? LocationId { get; set; }

		[JsonPropertyName("data")]
		public AtkinsHoursLocationData[]? LocationData { get; set; }
	}

	public class AtkinsHoursLocationData
	{
		private string? _timeLabel;

		[JsonPropertyName("date")]
		public string? Date { get; set; }

		[JsonPropertyName("date_short")]
		public string? DateShort { get; set; }

		[JsonPropertyName("label")]
		public string? TimeLabel
		{
			get => _timeLabel!;
			set => _timeLabel = value?.Replace(" - ", "–"); // replace with en dash
		}

		[JsonPropertyName("open")]
		public string? OpenTime { get; set; }

		[JsonPropertyName("close")]
		public string? CloseTime { get; set; }

		[JsonPropertyName("isClosed")]
		public string? IsClosed { get; set; }

		[JsonPropertyName("byAppointment")]
		public string? ByAppointment { get; set; }
	}
}
