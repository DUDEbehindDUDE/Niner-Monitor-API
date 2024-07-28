namespace OccupancyTracker.Models
{
	public record LocationData(DiningData Dining, int AdkinsOccupancy, List<Lot> Parking);
}