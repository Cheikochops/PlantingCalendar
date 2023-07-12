using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;
using System.Text.Json;

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
    public async Task<ActionResult> GetSeedInfo(long seedId)
    {
        var seedModel = await SeedHelper.GetFormatedSeedItem(seedId, true);

        return Ok(seedModel);
    }

    [HttpGet("list")]
    public async Task<ActionResult> GetSeedsList()
    {
        var seeds = await SeedHelper.GetSeedList();

        return Ok(seeds);
    }

    [HttpPost("")]
    public async Task<ActionResult> UpdateSeedInfo([FromBody] UploadSeedDetailModel seedItem)
    {
        try
        {
            await SeedHelper.SaveSeedInfo(seedItem);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [HttpDelete("")]
    public async Task<ActionResult> DeleteSeedInfo(long seedId)
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