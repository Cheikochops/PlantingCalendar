using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlantingCalendar.Pages
{
    public class LayoutModel : PageModel
    {
        public List<CalendarModel> _calendars = new List<CalendarModel>();

        public void OnGet()
        {


        }
    }
}
