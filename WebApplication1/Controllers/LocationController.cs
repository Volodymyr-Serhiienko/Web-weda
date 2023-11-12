using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    public class LocationController : Controller
    {
        [Route("/LocationController/Location")]
        public IActionResult Location([FromBody] JsonElement jsonData)
        {
            string? latitude = jsonData.GetProperty("variable1").GetString();
            string? longitude = jsonData.GetProperty("variable2").GetString();
            string? timeZoneOffset = jsonData.GetProperty("variable3").GetString();

            HttpContext.Session.SetString("latitude", latitude!);
            HttpContext.Session.SetString("longitude", longitude!);
            HttpContext.Session.SetString("timeZoneOffset", timeZoneOffset!);

            return Ok();
        }
    }
}