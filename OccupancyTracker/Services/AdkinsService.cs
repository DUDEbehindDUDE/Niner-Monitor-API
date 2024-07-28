using System.Diagnostics.Tracing;
using System.Text.Json;
using log4net;
using OccupancyTracker.Models;

namespace OccupancyTracker.Services
{
	public sealed class AdkinsService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ParkingDataService));
		private static AdkinsService? _instance;
		private static readonly HttpClient httpClient = new();
		private const string url = "https://atkinsapi.charlotte.edu/occupancy/get/";
		private bool polling = false;
		public int Occupants { get; private set; }
		// public int MaxOccupants { get; private set; }
		private AdkinsService() { }

		public static AdkinsService GetInstance()
		{
			_instance ??= new();
			_ = _instance.StartPolling();
			return _instance;
		}

		private async Task StartPolling()
		{
			if (polling) return;
			polling = true;
			const int pollingRate = 5000; // ms

			while (true)
			{
				await Fetch();
				await Task.Delay(pollingRate);
			}
		}

		private async Task Fetch()
		{
			try
			{
				using var res = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
				res.EnsureSuccessStatusCode();
				string rawData = (await res.Content.ReadAsStringAsync())!;
				AdkinsOccupancyResponse data = JsonSerializer.Deserialize<AdkinsOccupancyResponse>(rawData)!;
				// log.Debug($"Adkins Occupancy: {Parse(data.Occupancy)}");
				Occupants = Parse(data.Occupancy);
			}
			catch (HttpRequestException ex)
			{
				log.Error($"An error occurred while fetching data: {ex}");
			}
		}

		private static int Parse(string occupancyStr)
		{
			if (occupancyStr == "CLØSED") return 0;
			return int.Parse(occupancyStr);
		}
	}
}