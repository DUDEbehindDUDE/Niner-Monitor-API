namespace OccupancyTracker.Models
{
// This class is used to deserialize incoming JSON, and I don't feel like
// installing a library just so that these variables can be uppercase.
#pragma warning disable IDE1006
  public class DiningSocketResponse
  {
        public int? occupants { get; set; }
        public string? action { get; set; }
  }
#pragma warning restore IDE1006
}