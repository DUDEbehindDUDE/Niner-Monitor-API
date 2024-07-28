using System.Text.Json;
using log4net;
using OccupancyTracker.Models;
using SocketIO.Core;

namespace OccupancyTracker.Services
{
	public sealed class Social704DataService : DiningService
	{
		private static Social704DataService? _instance;
		protected override ILog Log => LogManager.GetLogger(typeof(Social704DataService));
		protected override string SpaceId => "7a9c0a24-920c-42c7-872d-74f1e98bcf01";
		protected override int InitialMaxOccupants => 463;

		private Social704DataService() { }
		public static async Task<Social704DataService> GetInstance()
		{
			if (_instance == null)
			{
				_instance = new Social704DataService();
				await _instance.Connect();
			}
			return _instance;
		}
	}
}