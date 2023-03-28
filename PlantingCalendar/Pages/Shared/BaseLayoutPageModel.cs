using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

namespace PlantingCalendar.Pages
{
    public class BaseLayoutPageModel : PageModel
    {
        public ICalendarDataAccess CalendarDataAccess { get; set; }
        public List<CalendarItemBasicModel> Calendars = new List<CalendarItemBasicModel>();

        public BaseLayoutPageModel(ICalendarDataAccess calendarDataAccess)
        {
            CalendarDataAccess = calendarDataAccess;
        }

        public async Task GetDropDownCalendars()
        {
            Calendars = await CalendarDataAccess.GetBasicCalendars();
        }
    }
}
