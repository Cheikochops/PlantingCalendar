using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;
//using System.Web.Http;

[ApiController]
[Route("api/calendar")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarHelper _calendarHelper;
    public CalendarController (ICalendarHelper calendarHelper)
    {
        _calendarHelper = calendarHelper;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetCalendar(long id)
    {
        var details = _calendarHelper.FormatCalendar(id);

        return Ok(details);
    }

    [HttpDelete("seed")]
    public async Task<ActionResult> RemoveSeedFromCalendar(long calendarId, long seedId)
    {
        await _calendarHelper.RemoveSeedFromCalendar(calendarId, seedId);
        return Ok();
    }

    [HttpPost("seed")]
    public async Task<ActionResult> AddSeedToCalendar(long calendarId, long seedId)
    {
        await _calendarHelper.AddSeedToCalendar(calendarId, seedId);
        return Ok();
    }

    [HttpPost("")]
    public async Task<ActionResult> GenerateCalendar([FromBody]GenerateCalendarModel calendar)
    {
        var calendarId = await _calendarHelper.GenerateCalendar(calendar);

        return Ok(calendarId);
    }

}