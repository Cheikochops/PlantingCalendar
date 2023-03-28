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

        public SeedItemModel SelectedSeed { get; set; }

        public SeedsModel (ICalendarDataAccess calendarDataAccess, ISeedDataAccess seedDataAccess) : base (calendarDataAccess)
        {
            SeedDataAccess = seedDataAccess;
        }

        public async void OnGet()
        {
            await GetDropDownCalendars();
            await GetSeeds();
        }

        public async Task GetSeeds()
        {
            //Seeds = await SeedDataAccess.GetAllSeeds();

            //Temporary test models
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
                },
                new SeedItemModel
                {
                    PlantType = "Cucumber",
                    Breed = "Masterpiece",
                    SunRequirement = "1",
                    WaterRequirement = "2",
                    HarvestingDates = "1",
                    SowingDates = "2"
                },
                new SeedItemModel
                {
                    PlantType = "Corriander",
                    Breed = null,
                    SunRequirement = "1",
                    WaterRequirement = "2",
                    HarvestingDates = "1",
                    SowingDates = "2"
                },
                new SeedItemModel
                {
                    PlantType = "Asparagus",
                    Breed = null,
                    SunRequirement = "1",
                    WaterRequirement = "2",
                    HarvestingDates = "1",
                    SowingDates = "2"
                }
            };
        }
    }
}
