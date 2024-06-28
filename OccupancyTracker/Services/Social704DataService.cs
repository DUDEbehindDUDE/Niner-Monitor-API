using System.Text.Json;
using OccupancyTracker.Models;
using SocketIO.Core;

namespace OccupancyTracker.Services
{
  public sealed class Social704DataService
  {
    private static Social704DataService? _instance;
    private readonly SocketIOClient.SocketIO _socket;
    public int Occupants { get; private set; }
    public int MaxOccupants { get; private set; }

    private Social704DataService() {
      Occupants = 0;
      MaxOccupants = 463;
      var url = "https://app.safespace.io/";
      var socketOptions = new SocketIOClient.SocketIOOptions
      {
        Path = "/veart/socket.io",
        EIO = EngineIO.V3
      };
      _socket = new SocketIOClient.SocketIO(url, socketOptions);
    }
    public static async Task<Social704DataService> GetInstance()
    {
      if (_instance == null) {
        _instance = new Social704DataService();
        await _instance.Connect();
      }
      return _instance;
    }


    private async Task Connect()
    {
      _socket.OnConnected += (sender, e) => Console.WriteLine("Connected to the Socket.IO server.");
      _socket.OnDisconnected += (sender, e) => Console.WriteLine("Disconnected from the Socket.IO server.");
      _socket.OnReconnectAttempt += (sender, e) => Console.WriteLine("Attempting to reconnect...");
      _socket.OnError += (sender, e) => Console.WriteLine($"Socket.IO error: {e}");
      
      _socket.On("manualoccupancy:data", response =>
      {
        string data = response.GetValue<object>().ToString()!;
        var formattedData = JsonSerializer.Deserialize<DiningSocketResponse>(data);
        if (formattedData?.occupants != null) {
          Occupants = (int)formattedData.occupants;
          Console.WriteLine("Current Social 704 Occupants: " + Occupants);
        };
      });
      _socket.On("manualoccupancy:spaceupdate", response =>
      {
        Console.WriteLine("704 Size: " + response);
        // string data = response.GetValue<object>().ToString()!;
        // var formattedData = JsonSerializer.Deserialize<DiningSocketResponse>(data);
        // if (formattedData?.occupants != null) {
        //   Occupants = (int)formattedData.occupants;
        //   Console.WriteLine("Current Social 704 Size: " + MaxOccupants);
        // };
      });

      _socket.OnConnected += async (sender, e) =>
      {
        string spaceId = "7a9c0a24-920c-42c7-872d-74f1e98bcf01";
        await _socket.EmitAsync("manualoccupancy:subscribe", spaceId);
        await _socket.EmitAsync("manualoccupancy:spaceupdate-subscribe", spaceId);
      };

      await _socket.ConnectAsync();
    }
  }
}