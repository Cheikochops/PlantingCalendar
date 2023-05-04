using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;

namespace PlantingCalendar.Controllers;

public class SeedController : Controller
{
    private ICalendarDataAccess _calendarDataAccess { get; set; }

    private ISeedDataAccess _dataAccess { get; set; }

    private ISeedHelper _seedHelper { get; set; }

    public SeedController(ICalendarDataAccess calendarDataAccess, ISeedDataAccess dataAccess, ISeedHelper seedHelper)
    {
        _dataAccess = dataAccess;
        _calendarDataAccess = calendarDataAccess;
        _seedHelper = seedHelper;
    }

    public async Task<ActionResult> SeedInfo(long? seedId)
    {
        var seedModel = new SeedDetailModel();
        if (seedId != null)
        {
            var sqlSeedDetails = await _dataAccess.GetSeedDetails(seedId.Value);
            seedModel = _seedHelper.FormatSeedItem(sqlSeedDetails);
        }

        return PartialView("SeedInformationPopup", seedModel);
    }

    public async Task<ActionResult> SeedsList(string? filter, int? orderBy)
    {
        var seeds = await _dataAccess.GetAllSeeds();
        var orderedSeeds = _seedHelper.FilterSeedItems(seeds, filter, orderBy);

        var seedDetails = new SeedsListModel();
        seedDetails.Seeds = orderedSeeds;

        return PartialView("SeedsList", seedDetails);
    }

    [HttpPost]
    public ActionResult SeedInfo(SeedDetailModel seed)
    {
        //Update to save updates and return an OK then display a notification on the page
        return Ok();
    }

    [HttpDelete]
    public ActionResult SeedInfo(long seedId)
    {
        _dataAccess.DeleteSeed(seedId);

        return Ok();
    }
}