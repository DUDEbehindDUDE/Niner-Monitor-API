using System.Text.Json;
using log4net;
using OccupancyTracker.Models;
using SocketIO.Core;

namespace OccupancyTracker.Services
{
	public sealed class SoViDataService : DiningService
	{
		private static SoViDataService? _instance;
		protected override ILog Log => LogManager.GetLogger(typeof(SoViDataService));
		protected override string SpaceId => "15da3cfa-044a-4ef7-92d9-f77f54c9fc3b";
		protected override int InitialMaxOccupants => 896;

		private SoViDataService() { }
		public static async Task<SoViDataService> GetInstance()
		{
			if (_instance == null)
			{
				_instance = new SoViDataService();
				await _instance.Connect();
			}
			return _instance;
		}
	}
}