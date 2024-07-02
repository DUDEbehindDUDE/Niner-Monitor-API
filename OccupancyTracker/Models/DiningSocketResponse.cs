using System.Text.Json.Serialization;

namespace OccupancyTracker.Models
{
  public class DiningSocketResponse
  {
    [JsonPropertyName("occupants")]
    public int? Occupants { get; set; }

    [JsonPropertyName("action")]
    public string? Action { get; set; }
  }
}