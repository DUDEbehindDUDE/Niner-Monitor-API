using System.Text.Json;
using log4net;
using OccupancyTracker.Models;
using SocketIO.Core;

namespace OccupancyTracker.Services
{
	public abstract class DiningService
	{
		private readonly SocketIOClient.SocketIO _socket;
		public int Occupants { get; protected set; }
		public int MaxOccupants { get; protected set; }

		protected abstract ILog Log { get; }
		protected abstract string SpaceId { get; }
		protected abstract int InitialMaxOccupants { get; }

		protected DiningService()
		{
			Occupants = 0;
			MaxOccupants = InitialMaxOccupants;
			var url = "https://app.safespace.io/";
			var socketOptions = new SocketIOClient.SocketIOOptions
			{
				Path = "/veart/socket.io",
				EIO = EngineIO.V3
			};
			_socket = new SocketIOClient.SocketIO(url, socketOptions);
		}

		public async Task Connect()
		{
			_socket.OnConnected += (sender, e) => Log.Info("Connected to the Socket.IO server.");
			_socket.OnDisconnected += (sender, e) => Log.Warn("Disconnected from the Socket.IO server.");
			_socket.OnReconnectAttempt += (sender, e) => Log.Warn("Attempting to reconnect to the Socket.IO Server...");
			_socket.OnError += (sender, e) => Log.Error($"Socket.IO error: {e}");

			_socket.OnConnected += async (sender, e) =>
			{
				await _socket.EmitAsync("manualoccupancy:subscribe", SpaceId);
				await _socket.EmitAsync("manualoccupancy:spaceupdate-subscribe", SpaceId);
			};

			_socket.On("manualoccupancy:data", response =>
			{
				string data = response.GetValue<object>().ToString()!;
				var deserialized = JsonSerializer.Deserialize<DiningSocketResponse>(data);
				if (deserialized?.Occupants != null)
				{
					Occupants = (int)deserialized.Occupants;
					Log.Debug($"Current Occupants: {Occupants}");
				}
			});

			_socket.On("manualoccupancy:spaceupdate", response =>
			{
				string data = response.GetValue<object>().ToString()!;
				var deserialized = JsonSerializer.Deserialize<SpaceResponse>(data);
				if (deserialized?.Space?.MaxCapacity != null)
				{
					MaxOccupants = deserialized.Space.MaxCapacity;
					Log.Debug($"Max Occupants: {MaxOccupants}");
				}
			});

			await _socket.ConnectAsync();
		}
	}
}