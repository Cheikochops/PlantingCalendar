using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Interfaces;

namespace PlantingCalendar.Pages
{
    public class HomeModel : BaseLayoutPageModel
    {
        private readonly ILogger<HomeModel> _logger;

        public HomeModel(ILogger<HomeModel> logger, ICalendarDataAccess calendarDataAccess) : base(calendarDataAccess)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            await GetCalendars();
        }
    }
}