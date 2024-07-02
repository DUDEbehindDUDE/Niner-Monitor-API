using System.Text.Json.Serialization;

namespace OccupancyTracker.Models
{
  public class ResetTime
  {
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }
  }

  public class ResetConfig
  {
    [JsonPropertyName("resetDaily")]
    public bool ResetDaily { get; set; }

    [JsonPropertyName("resetDailyAtTime")]
    public bool ResetDailyAtTime { get; set; }

    [JsonPropertyName("resetTimes")]
    public List<ResetTime>? ResetTimes { get; set; }
  }

  public class Space
  {
    [JsonPropertyName("spaceId")]
    public string? SpaceId { get; set; }

    [JsonPropertyName("orgId")]
    public string? OrgId { get; set; }

    [JsonPropertyName("locationId")]
    public string? LocationId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("maxCapacity")]
    public int MaxCapacity { get; set; }

    [JsonPropertyName("targetOccupancy")]
    public int TargetOccupancy { get; set; }

    [JsonPropertyName("resetConfig")]
    public ResetConfig? ResetConfig { get; set; }

    [JsonPropertyName("isSafeSpace")]
    public bool IsSafeSpace { get; set; }

    [JsonPropertyName("disabled")]
    public bool Disabled { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
  }

  public class SpaceResponse
  {
    [JsonPropertyName("space")]
    public Space? Space { get; set; }
  }
}