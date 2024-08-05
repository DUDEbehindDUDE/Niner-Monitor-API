using Microsoft.OpenApi.Any;

namespace OccupancyTracker.Models
{
	public record HistoricalData<T>(DateTime Time, T Data);
}