namespace OccupancyTracker.Models
{
	public record LocationData(DiningData Dining, AtkinsData Adkins, List<Lot> Parking);
}