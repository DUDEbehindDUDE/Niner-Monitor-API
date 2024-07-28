using log4net;
using OccupancyTracker.Services;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

internal class Program
{
	private static readonly ILog log = LogManager.GetLogger(typeof(Program));
	private static async Task Main(string[] args)
	{

		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSingleton(serviceProvider => Social704DataService.GetInstance().Result);

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddSwaggerGen();
		builder.Services.AddControllers();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers();

		await InitializeServices();
		app.Run();
	}

	static async Task InitializeServices()
	{
		log.Debug("Initializing services...");
		await Social704DataService.GetInstance();
		await SoViDataService.GetInstance();
		await ParkingDataService.GetInstance();
		log.Debug("Done initializing services!");
	}
}