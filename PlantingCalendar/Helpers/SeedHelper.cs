using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.DataAccess
{
    public class SeedHelper : ISeedHelper
    {
        public SeedHelper()
        {
        }

        public SeedDetailModel FormatSeedItem(List<SqlSeedDetailsModel> seedDetails)
        {
            var first = seedDetails.FirstOrDefault();

            if (first == null)
            {
                throw new Exception("Unexpected list length");
            }

            return new SeedDetailModel
            {
                Id = first.Id,
                Breed = first.Breed,
                PlantType = first.PlantType,
                Description = first.Description,
                SunRequirement = first.SunRequirement,
                WaterRequirement = first.WaterRequirement,
                Actions = seedDetails.Select(x => new SeedAction
                {
                    ActionId = x.Id,
                    ActionType = x.ActionType,
                    DisplayChar = x.DisplayChar.First(),
                    DisplayColour = x.DisplayColour,
                    EndDate = x.EndDate.Value,
                    StartDate = x.StartDate.Value
                }).ToList()
            };

        }

        public IOrderedEnumerable<SeedItemModel> FilterSeedItems(List<SeedItemModel> seeds, string filter)
        {
            var filteredSeeds = seeds;

            if (filter != null)
            {
                filteredSeeds = seeds.Where(x => x.Breed.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || x.PlantType.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            return filteredSeeds.OrderBy(x => x.PlantType).ThenBy(x => x.Breed);
        }
    }
}