using System.Text.Json;
using log4net;
using OccupancyTracker.Models;

namespace OccupancyTracker.Services
{
	public sealed class ParkingDataService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ParkingDataService));
		private static ParkingDataService? _instance;
		private static readonly HttpClient httpClient = new();
		private const string url = "https://parkingavailability.charlotte.edu/decks/stream";

		private bool listeningToStream = false;
		public List<Lot> ParkingData { get; private set; }

		private ParkingDataService()
		{
			ParkingData = [];
		}

		public static async Task<ParkingDataService> GetInstance()
		{
			_instance ??= new ParkingDataService();
			_ = _instance.StartListening();

			if (_instance.ParkingData.Count == 0)
			{
				// wait up to 5s to get initial value
				for (int i = 0; i < 50; i++)
				{
					if (_instance.ParkingData.Count != 0) break;
					await Task.Delay(100);
				}
			}

			return _instance;
		}

		private async Task StartListening()
		{
			if (listeningToStream) return;
			const int retryDelay = 5000; // ms between retries if the 
			listeningToStream = true;

			while (true)
			{
				await ReadStream(retryDelay);
				await Task.Delay(retryDelay);
			}
		}

		private async Task ReadStream(int retryDelay)
		{
			try
			{
				using var res = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
				res.EnsureSuccessStatusCode();

				using var stream = await res.Content.ReadAsStreamAsync();
				using var reader = new StreamReader(stream);
				log.Info("Connected to stream.");

				while (!reader.EndOfStream)
				{
					string data = (await reader.ReadLineAsync())!;
					data = data.StartsWith("data:") ? data[5..] : data;
					if (data == "") continue;

					ParkingData = JsonSerializer.Deserialize<List<Lot>>(data)!;
				}
			}
			catch (Exception ex)
			{
				log.Error($"An error occurred while reading parking data stream: {ex.Message}.");
			}
			finally
			{
				log.Warn($"Stream ended. Attempting to reconnect in {retryDelay}ms...");
			}
		}
	}
}