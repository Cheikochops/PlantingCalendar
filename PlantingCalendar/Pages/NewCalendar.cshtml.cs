using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using System.Web;

namespace PlantingCalendar.Pages
{
    public class NewCalendarModel : BaseLayoutPageModel
    {
        public NewCalendarModel(ICalendarDataAccess calendarDataAccess) : base(calendarDataAccess)
        {

        }

        public async void OnGet()
        {
            await GetDropDownCalendars();
        }
    }
}
