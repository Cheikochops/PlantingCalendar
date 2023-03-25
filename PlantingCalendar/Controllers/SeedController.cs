using Microsoft.AspNetCore.Mvc;

namespace PlantingCalendar.Controllers;

public class SeedController : Controller
{
    public SeedController()
    {
    }

    public IActionResult GetAll()
    {
        return Ok();
    }
}