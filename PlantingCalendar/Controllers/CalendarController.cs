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
        var details = await _calendarHelper.FormatCalendar(id);

        return Ok(details);
    }

    [HttpPost("seeds")]
    public async Task<ActionResult> UpdateCalendarSeeds(long calendarId, [FromBody]long[] seedIds)
    {
        await _calendarHelper.UpdateCalendarSeeds(calendarId, seedIds.ToList());
        return Ok();
    }

    [HttpPost("")]
    public async Task<ActionResult> GenerateCalendar([FromBody]GenerateCalendarModel calendar)
    {
        var calendarId = await _calendarHelper.GenerateCalendar(calendar);

        return Ok(calendarId);
    }

}