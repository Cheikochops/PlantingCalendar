using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

namespace PlantingCalendar.Pages
{
    public class BaseLayoutPageModel : PageModel
    {
        public ICalendarDataAccess CalendarDataAccess { get; set; }
        public List<CalendarItemModel> Calendars = new List<CalendarItemModel>();

        public BaseLayoutPageModel(ICalendarDataAccess calendarDataAccess)
        {
            CalendarDataAccess = calendarDataAccess;
        }

        public async Task GetCalendars()
        {
            Calendars = await CalendarDataAccess.GetAllCalendars();
        }
    }
}
