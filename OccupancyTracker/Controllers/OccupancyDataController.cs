using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using OccupancyTracker.Models;
using OccupancyTracker.Services;

namespace OccupancyTracker.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OccupancyDataController : ControllerBase
  {

    [HttpGet("CurrentOccupancyData")]
    public async Task<OccupancyData> GetOccupancyData()
    {
      var service704 = await Social704DataService.GetInstance();
      var serviceSoVi = await SoViDataService.GetInstance();

      var dining = new DiningData
      (
          serviceSoVi.Occupants,
          serviceSoVi.MaxOccupants,
          service704.Occupants,
          service704.MaxOccupants
      );

      OccupancyData data = new(DateTime.Now, new LocationData(dining));
      return data;
    }
  }
}