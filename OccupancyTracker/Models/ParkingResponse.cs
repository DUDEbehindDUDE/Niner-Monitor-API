using System.Text.Json.Serialization;

namespace OccupancyTracker.Models
{

  public class Lot
  {
    [JsonPropertyName("lotCode")]
    public required string LotCode { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    // [JsonPropertyName("style")]
    // public string? Style { get; set; }

    [JsonPropertyName("mapLink")]
    public string? MapLink { get; set; }

    [JsonPropertyName("percentAvailable")]
    public double PercentAvailable { get; set; }
  }
}