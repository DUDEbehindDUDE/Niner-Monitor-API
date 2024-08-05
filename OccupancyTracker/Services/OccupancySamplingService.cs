using log4net;
using OccupancyTracker.Models;

namespace OccupancyTracker.Services
{
	public sealed class OccupancySamplingService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(OccupancySamplingService));
		private static OccupancySamplingService? _instance;
		public List<HistoricalData<DiningData>> DiningDatas { get; private set; } = [];
		public List<HistoricalData<List<Lot>>> ParkingDatas { get; private set; } = [];
		public List<HistoricalData<AtkinsData>> AtkinsDatas { get; private set; } = [];

		private OccupancySamplingService()
		{
			_ = ExecuteAsync();
		}

		public static OccupancySamplingService GetInstance()
		{
			_instance ??= new OccupancySamplingService();

			return _instance;
		}

		private async Task ExecuteAsync()
		{
			var service704 = await Social704DataService.GetInstance();
			var serviceSoVi = await SoViDataService.GetInstance();
			var serviceParking = await ParkingDataService.GetInstance();
			var serviceAtkins = AtkinsService.GetInstance();

			var oneMinTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
			while (await oneMinTimer.WaitForNextTickAsync())
			{
				var diningData = new DiningData(serviceSoVi.Occupants, serviceSoVi.MaxOccupants, service704.Occupants, service704.MaxOccupants);
				var atkinsOpenLabel = serviceAtkins.TodayHoursData?.TimeLabel ?? "Closed Today";
				var atkinsData = new AtkinsData
				(
					serviceAtkins.Occupants,
					serviceAtkins.Open,
					atkinsOpenLabel,
					serviceAtkins.LastRefresh
				);
				DiningDatas.Add(new(DateTime.Now, diningData));
				ParkingDatas.Add(new(DateTime.Now, serviceParking.ParkingData));
				AtkinsDatas.Add(new(DateTime.Now, atkinsData));
				// log.Debug(DiningDatas);
				// log.Debug(ParkingDatas);
				// log.Debug(AtkinsDatas);
			}
		}
	}
}