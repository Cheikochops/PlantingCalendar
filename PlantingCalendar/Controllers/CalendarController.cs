using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;
//using System.Web.Http;

public class CalendarController : ControllerBase
{
    private readonly ICalendarDataAccess _dataAccess;
    private readonly ICalendarHelper _calendarHelper;
    public CalendarController (ICalendarDataAccess dataAccess, ICalendarHelper calendarHelper)
    {
        _dataAccess = dataAccess;
        _calendarHelper = calendarHelper;
    }

    [HttpGet]
    public async Task<ActionResult> Calendar(int id)
    {
        var calendar = await _dataAccess.GetCalendar(id);

        return Ok(calendar);
    }

}