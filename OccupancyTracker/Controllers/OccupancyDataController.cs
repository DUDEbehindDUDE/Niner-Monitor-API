using Microsoft.AspNetCore.Mvc;
using OccupancyTracker.Models;

namespace OccupancyTracker.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OccupancyDataController : ControllerBase
  {
    [HttpGet("currentoccupancydata")]
    public OccupancyData GetOccupancyData()
    {
      int currentSoVi = Random.Shared.Next(400);
      int current704 = Random.Shared.Next(400);

      var data = new OccupancyData(DateTime.Now, new LocationData
      (
          new DiningData(currentSoVi, 400, current704, 400)
      ));

      return data;
    }
  }
}