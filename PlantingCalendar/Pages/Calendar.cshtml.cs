using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using System.Web;

namespace PlantingCalendar.Pages
{
    public class CalendarModel : BaseLayoutPageModel
    {
        public CalendarDetailsModel CurrentCalendar { get; set; }

        public CalendarModel(ICalendarDataAccess calendarDataAccess) : base(calendarDataAccess)
        {

        }

        public async void OnGet()
        {
            var calendarId = HttpUtility.UrlDecode(Request.Query["calendarId"].FirstOrDefault()) ?? null;

            await GetDropDownCalendars();

            if (calendarId != null)
            {
                var CurrentCalendar = await CalendarDataAccess.GetCalendar(long.Parse(calendarId));
            }
            else
            {
                
            }
        }
    }
}
