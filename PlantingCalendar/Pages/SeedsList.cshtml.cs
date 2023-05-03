using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Models;

namespace PlantingCalendar.Pages
{
    public class SeedsListModel : PageModel
    {

        public IOrderedEnumerable<SeedItemModel> Seeds { get; set; }

        public SeedsListModel() 
        {

        }
    }
}
