using log4net;
using OccupancyTracker.Models;

namespace OccupancyTracker.Services
{
	public sealed class AtkinsService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(AtkinsService));
		private static AtkinsService? _instance;
		private static readonly HttpClient httpClient = new();
		private bool polling = false;
		public int Occupants { get; private set; }
		// public int MaxOccupants { get; private set; }
		public bool Open = false;

		public DateTime LastRefresh = DateTime.Now;
		public AtkinsHoursLocationData? TodayHoursData;
		private AtkinsService() { }

		public static AtkinsService GetInstance()
		{
			_instance ??= new();
			_ = _instance.StartPolling();
			return _instance;
		}

		private async Task StartPolling()
		{

			if (polling) return;
			polling = true;

			try
			{
				while (true)
				{
					await FetchOccupants();
					await FetchHours();
					LastRefresh = DateTime.Now;

					// the API refreshes every quarter hour, no point polling more often than that
					var minUntilRefresh = 15 - DateTime.Now.Minute % 15 + 1; // +1 in case system clock ahead or api clock slow
					log.Debug($"Refreshing in {minUntilRefresh} minutes");
					await Task.Delay(new TimeSpan(0, minUntilRefresh, 0));
				}
			}
			catch (Exception e)
			{
				log.Error($"An error occurred: {e}");
			}
			finally
			{
				polling = false;
				log.Warn("Polling has stopped. Retrying polling in 5000ms...");
				await Task.Delay(5000);
				_ = StartPolling();
			}
		}

		private async Task FetchOccupants()
		{
			const string url = "https://atkinsapi.charlotte.edu/occupancy/get/";
			try
			{
				using var res = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
				res.EnsureSuccessStatusCode();
				var data = (await res.Content.ReadFromJsonAsync<AtkinsOccupancyResponse>())!;
				Occupants = Parse(data.Occupancy);
				log.Debug($"Current Occupants: {Occupants}");
			}
			catch (Exception ex)
			{
				log.Error($"An error occurred while fetching occupancy data: {ex}");
			}
		}

		private async Task FetchHours()
		{
			const string url = "https://atkinsapi.charlotte.edu/hours/";
			try
			{
				var date = DateTime.Today;
				var content = new FormUrlEncodedContent(
				[
						new KeyValuePair<string, string>("view", "all"),
						new KeyValuePair<string, string>("date", date.ToString("yyyy-MM-dd"))
				]);

				using var res = await httpClient.PostAsync(url, content);
				res.EnsureSuccessStatusCode();

				AtkinsHoursResponse? data = await res.Content.ReadFromJsonAsync<AtkinsHoursResponse>();

				var _todayData = data?.Data?["1"]?.LocationData?[0];
				if (_todayData != null) TodayHoursData = _todayData;
				// log.Debug($"Today's Opening Times: {TodayHoursData?.TimeLabel ?? "Closed"}");
			}
			catch (Exception ex)
			{
				log.Error($"An error occurred while fetching hours data: {ex}");
			}
		}

		private int Parse(string occupancyStr)
		{
			if (occupancyStr == "CLØSED")
			{
				Open = false;
				return -1;
			}
			Open = true;
			return int.Parse(occupancyStr);
		}
	}
}