using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.DataAccess
{
    public class SeedHelper : ISeedHelper
    {
        private ISeedDataAccess SeedDataAccess { get; set; }

        public SeedHelper(ISeedDataAccess seedDataAccess)
        {
            SeedDataAccess = seedDataAccess;
        }

        public async Task<SeedDetailModel> GetFormatedSeedItem(long seedId)
        {
            var seedDetails = await SeedDataAccess.GetSeedDetails(seedId).ConfigureAwait(false);

            var first = seedDetails.FirstOrDefault();

            if (first == null)
            {
                throw new Exception("Unexpected list length");
            }

            var model = new SeedDetailModel
            {
                Id = first.Id,
                Breed = first.Breed,
                PlantType = first.PlantType,
                Description = first.Description,
                ExpiryDate = first.ExpiryDate,
                SunRequirement = first.SunRequirement,
                WaterRequirement = first.WaterRequirement
            };

            if (seedDetails.Any(x => x.ActionId != null))
            {
                model.Actions = seedDetails.Select(x => new SeedAction
                {
                    ActionId = x.Id,
                    ActionType = x.ActionType,
                    DisplayChar = x.DisplayChar.First(),
                    DisplayColour = x.DisplayColour,
                    EndDate = x.EndDate.Value,
                    StartDate = x.StartDate.Value
                }).ToList();
            }

            return model;

        }

        public async Task<IOrderedEnumerable<SeedItemModel>> GetFilteredSeedItems(string? filter, int? orderBy)
        {
            //orderBy
            // 1 = PlantType, 2 = Breed, 3 = SunRequirement, 4 = WaterRequirement

            var seeds = await SeedDataAccess.GetAllSeeds();

            var filteredSeeds = seeds;

            if (filter != null)
            {
                filteredSeeds = seeds.Where(x => x.Breed.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || x.PlantType.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            switch (orderBy)
            {
                case 2:
                    return filteredSeeds.OrderBy(x => x.Breed);
                case 3:
                    return filteredSeeds.OrderBy(x => x.SunRequirement);
                case 4:
                    return filteredSeeds.OrderBy(x => x.WaterRequirement);
                default:
                    return filteredSeeds.OrderBy(x => x.PlantType).ThenBy(x => x.Breed);
            }
        }

        public async Task SaveSeedInfo(SeedItemModel seed)
        {

        }

        public async Task DeleteSeed(long seedId)
        {
            await SeedDataAccess.DeleteSeed(seedId);
        }
    }
}