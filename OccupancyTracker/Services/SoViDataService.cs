using System.Text.Json;
using OccupancyTracker.Models;
using SocketIO.Core;

namespace OccupancyTracker.Services
{
  public sealed class SoViDataService
  {
    private static SoViDataService? _instance;
    private readonly SocketIOClient.SocketIO _socket;
    public int Occupants { get; private set; }
    public int MaxOccupants { get; private set; }

    private SoViDataService()
    {
      Occupants = 0;
      MaxOccupants = 896;
      var url = "https://app.safespace.io/";
      var socketOptions = new SocketIOClient.SocketIOOptions
      {
        Path = "/veart/socket.io",
        EIO = EngineIO.V3
      };
      _socket = new SocketIOClient.SocketIO(url, socketOptions);
    }
    public static async Task<SoViDataService> GetInstance()
    {
      if (_instance == null)
      {
        _instance = new SoViDataService();
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
        if (formattedData?.occupants != null)
        {
          Occupants = (int)formattedData.occupants;
          Console.WriteLine("Current SoVi Occupants: " + Occupants);
        };
      });
      _socket.On("manualoccupancy:spaceupdate", response =>
      {
        Console.WriteLine("SoVi Size: " + response);
        // string data = response.GetValue<object>().ToString()!;
        // var formattedData = JsonSerializer.Deserialize<DiningSocketResponse>(data);
        // if (formattedData?.occupants != null) {
        //   Occupants = (int)formattedData.occupants;
        //   Console.WriteLine("Current Social 704 Size: " + MaxOccupants);
        // };
      });


      _socket.OnConnected += async (sender, e) =>
      {
        string spaceId = "15da3cfa-044a-4ef7-92d9-f77f54c9fc3b";
        await _socket.EmitAsync("manualoccupancy:subscribe", spaceId);
        await _socket.EmitAsync("manualoccupancy:spaceupdate-subscribe", spaceId);
      };

      await _socket.ConnectAsync();
    }
  }
}