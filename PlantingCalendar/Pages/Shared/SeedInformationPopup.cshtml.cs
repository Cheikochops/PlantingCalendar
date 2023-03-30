using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

namespace PlantingCalendar.Pages
{
    public class SeedInformationPopupModel : PageModel
    {
        public SeedDetailModel Seed { get; set; } 

        public SeedInformationPopupModel() 
        {

        }

        public async void OnGet()
        {

        }
    }
}
