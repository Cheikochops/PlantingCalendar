using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

namespace PlantingCalendar.Pages
{
    public class SeedsModel : BaseLayoutPageModel
    {
        private ISeedDataAccess SeedDataAccess { get; set; }

        public List<SeedItemModel> Seeds { get; set; }

        public SeedsModel (ICalendarDataAccess calendarDataAccess, ISeedDataAccess seedDataAccess) : base (calendarDataAccess)
        {
            SeedDataAccess = seedDataAccess;
        }

        public async void OnGet()
        {
            await GetCalendars();
            await GetSeeds();
        }

        public async Task GetSeeds()
        {
            //Seeds = await SeedDataAccess.GetAllSeeds();
            Seeds = new List<SeedItemModel>()
            {
                new SeedItemModel
                {
                    PlantType = "Tomato",
                    Breed = "Gardener's Delight",
                    SunRequirement = "1",
                    WaterRequirement = "2",
                    HarvestingDates = "1",
                    SowingDates = "2"

                }
            };
        }
    }
}
