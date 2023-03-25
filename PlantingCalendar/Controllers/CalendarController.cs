using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Interfaces;
using System.Globalization;
using System.Text.Encodings.Web;

namespace PlantingCalendar.Controllers;

public class CalendarController : Controller
{
    private readonly ICalendarDataAccess _dataAccess;
    public CalendarController (ICalendarDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<ActionResult<List<Calendar>>> Index()
    {
        var calendars = await GetAll();
        ViewBag.Calendars = calendars;

        return View();

    }

    public async Task<ActionResult<List<Calendar>>> GetAll()
    {
        try
        {
            var calendars = await _dataAccess.GetAllCalendars();
            return Ok(calendars);
        }
        catch (Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }
}