using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using OccupancyTracker.Models;
using OccupancyTracker.Services;

namespace OccupancyTracker.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OccupancyDataController : ControllerBase
	{

		[HttpGet("CurrentOccupancyData")]
		public async Task<OccupancyData> GetOccupancyData()
		{
			var service704 = await Social704DataService.GetInstance();
			var serviceSoVi = await SoViDataService.GetInstance();
			var serviceParking = await ParkingDataService.GetInstance();
			var serviceAtkins = AtkinsService.GetInstance();

			var dining = new DiningData
			(
					serviceSoVi.Occupants,
					serviceSoVi.MaxOccupants,
					service704.Occupants,
					service704.MaxOccupants
			);

			var openLabel = serviceAtkins.TodayHoursData?.TimeLabel ?? "Closed Today";
			var atkinsData = new AtkinsData
			(
				serviceAtkins.Occupants,
				serviceAtkins.Open,
				openLabel,
				serviceAtkins.LastRefresh
			);
			var parking = serviceParking.ParkingData;

			OccupancyData data = new(DateTime.Now, new LocationData(dining, atkinsData, parking));
			return data;
		}

		[HttpGet("HistoricalOccupancyData")]
		public ActionResult<List<HistoricalData<object>>> GetHistoricalOccupancyData([FromQuery] string item)
		{
			var sampleService = OccupancySamplingService.GetInstance();

			return item.ToLower() switch
			{
				"dining" => Ok(sampleService.DiningDatas),
				"atkins" => Ok(sampleService.AtkinsDatas),
				"parking" => Ok(sampleService.ParkingDatas),
				_ => NotFound("No data exists for this item!")
			};
		}
	}
}