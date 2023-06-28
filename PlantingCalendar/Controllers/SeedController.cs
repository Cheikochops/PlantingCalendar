using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;

namespace PlantingCalendar.Controllers;

[ApiController]
[Route("api/seeds")]
public class SeedController : ControllerBase
{
    private ISeedHelper SeedHelper { get; set; }

    public SeedController(ISeedHelper seedHelper)
    {
        SeedHelper = seedHelper;
    }

    [HttpGet("")]
    public async Task<ActionResult> SeedInfo(long? seedId)
    {
        var seedModel = new SeedDetailModel();
        if (seedId != null)
        {
            seedModel = await SeedHelper.GetFormatedSeedItem(seedId.Value);
        }

        return Ok(seedModel);
    }

    [HttpGet("list")]
    public async Task<ActionResult> SeedsList()
    {
        var seeds = await SeedHelper.GetSeedList();

        return Ok(seeds);
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