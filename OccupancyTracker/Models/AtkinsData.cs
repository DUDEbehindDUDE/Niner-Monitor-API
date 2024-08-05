namespace OccupancyTracker.Models
{
	public record AtkinsData(int CurrentOccupants, bool IsOpen, string TodayOpenTimes, DateTime LastUpdated);
}