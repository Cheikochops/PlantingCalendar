using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;

namespace PlantingCalendar.Controllers;

public class SeedController : Controller
{
    private ISeedHelper SeedHelper { get; set; }

    public SeedController(ISeedHelper seedHelper)
    {
        SeedHelper = seedHelper;
    }

    public async Task<ActionResult> SeedInfo(long? seedId)
    {
        var seedModel = new SeedDetailModel();
        if (seedId != null)
        {
            seedModel = await SeedHelper.GetFormatedSeedItem(seedId.Value);
        }

        return PartialView("SeedInformationPopup", seedModel);
    }

    public async Task<ActionResult> SeedsList(string? filter, int? orderBy)
    {
        var orderedSeeds = await SeedHelper.GetFilteredSeedItems(filter, orderBy);

        var seedDetails = new SeedsListModel()
        {
            Seeds = orderedSeeds
        };

        return PartialView("SeedsList", seedDetails);
    }

    [HttpPost]
    public async Task<ActionResult> SeedInfo(SeedDetailModel seed)
    {
        try
        {
            await SeedHelper.SaveSeedInfo(seed);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<ActionResult> SeedInfo(long seedId)
    {
        try
        {
            await SeedHelper.DeleteSeed(seedId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}