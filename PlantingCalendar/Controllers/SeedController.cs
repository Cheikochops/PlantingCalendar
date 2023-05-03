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
        var seedDetails = new SeedDetailModel();
        if (seedId != null)
        {
            seedDetails = await _dataAccess.GetSeedDetails(seedId.Value);
        }

        return PartialView("SeedInformationPopup", seedDetails);
    }

    public async Task<ActionResult> SeedsList(string? filter)
    {
        var seeds = await _dataAccess.GetAllSeeds();

        var orderedSeeds = _seedHelper.FilterSeedItems(seeds, filter);

        var seedDetails = new SeedsListModel();
        seedDetails.Seeds = orderedSeeds;

        return PartialView("SeedsList", seedDetails);
    }

    [HttpPost]
    public ActionResult SeedInfo(SeedDetailModel seed)
    {
        //Update to return an OK then display a notification on the page
        return View("Seeds", new SeedsModel(_calendarDataAccess, _dataAccess));
    }

    [HttpDelete]
    public ActionResult SeedInfo(long seedId)
    {
        //Update to actualy delete the seed from database
        //Add an are you sure confirmation popup to the seedinfo popup
        //_dataAccess.DeleteSeed(seedId);

        return Ok();
    }
}