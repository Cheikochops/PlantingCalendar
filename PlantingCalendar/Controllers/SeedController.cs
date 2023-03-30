using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;

namespace PlantingCalendar.Controllers;

public class SeedController : Controller
{
    private ISeedDataAccess _dataAccess { get; set; }

    public SeedController(ISeedDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<ActionResult> GetSeedInfo(long seedId)
    {
        //Uncomment once implemented
        //var seedDetail = await _dataAccess.GetSeedDetails(seedId);

        return PartialView("SeedInformationPopup", new SeedDetailModel
            {
                Id = seedId,
                Breed = "a",
                Description = "adadada",
                PlantType = "WOO",
                SunRequirement = "1",
                WaterRequirement = "2",
                Actions = new List<SeedAction>
                {
                    new SeedAction
                    {
                        ActionName = "Sow",
                        MaxDate = new DateOnly(2023, 03, 30),
                        MinDate = new DateOnly(2023, 01, 01)
                    }
                }
        });
    }

    [HttpPost]
    public void SaveSeed(SeedDetailModel seed)
    {
        var a = 1;

    }
}