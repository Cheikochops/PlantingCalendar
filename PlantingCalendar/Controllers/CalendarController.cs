﻿using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;
//using System.Web.Http;

[ApiController]
[Route("api/calendar")]
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

    [HttpPost]
    public async Task<ActionResult> Calendar([FromBody]GenerateCalendarModel calendar)
    {
        var calendarId = await _calendarHelper.GenerateCalendar(calendar);

        return Ok(calendar);
    }

}