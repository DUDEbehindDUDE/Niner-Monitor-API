namespace OccupancyTracker.Models
{
  public record DiningData(int CurrentSoVi, int MaxSoVi, int Current704, int Max704)
  {
    // percent occupancy at SoVi
    public double PercentSoVi => Math.Round((double)CurrentSoVi / MaxSoVi * 100, 2);

    // percent occupancy at Social 704
    public double Percent704 => Math.Round((double)Current704 / Max704 * 100, 2);
  };
}