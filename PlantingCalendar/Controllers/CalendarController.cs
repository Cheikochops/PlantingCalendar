using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
//using System.Web.Http;

public class CalendarController : ControllerBase
{
    private readonly ICalendarDataAccess _dataAccess;
    public CalendarController (ICalendarDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
}