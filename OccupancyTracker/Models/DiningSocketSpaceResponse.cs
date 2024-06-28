namespace OccupancyTracker.Models
{
// This class is used to deserialize incoming JSON, and I don't feel like
// installing a library just so that these variables can be uppercase.
#pragma warning disable IDE1006
  public class DiningSocketSpaceResponse
  {
        public int? occupants { get; set; }
  }
#pragma warning restore IDE1006
}