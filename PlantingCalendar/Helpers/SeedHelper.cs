using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Linq;

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
                ExpiryDate = first.ExpiryDate?.ToString("yyyy-MM-dd"),
                SunRequirement = first.SunRequirement,
                WaterRequirement = first.WaterRequirement,
                FrontImageUrl = first.FrontImageUrl,
                BackImageUrl = first.BackImageUrl
            };

            model = AddManadatoryActions(seedDetails, model);

            if (seedDetails.Any(x => x.ActionId != null))
            {
                model.Actions = seedDetails.Where(x => x.ActionType == ActionType.Custom).Select(x => new SeedAction
                {
                    ActionId = x.Id,
                    ActionName = x.ActionName,
                    ActionType = x.ActionType,
                    DisplayChar = x.DisplayChar != null ? x.DisplayChar.First() : null,
                    DisplayColour = "#" + (x.DisplayColour ?? "000000"),
                    EndDateMonth = new string(x.EndDate.TakeLast(2).ToArray()),
                    EndDateDay = new string(x.EndDate.Take(2).ToArray()),
                    StartDateMonth = new string(x.StartDate.TakeLast(2).ToArray()),
                    StartDateDay = new string(x.StartDate.Take(2).ToArray()),
                }).ToList();
            }

            return model;

        }

        private SeedDetailModel AddManadatoryActions(List<SqlSeedDetailsModel> seedDetails, SeedDetailModel model)
        {
            var sowAction = seedDetails.FirstOrDefault(x => x.ActionType == ActionType.Sow);

            if (sowAction != null)
            {
                model.SowAction = new SeedAction
                {
                    ActionId = sowAction.Id,
                    ActionName = sowAction.ActionName,
                    ActionType = sowAction.ActionType,
                    DisplayChar = sowAction.DisplayChar != null ? sowAction.DisplayChar.First() : 'S',
                    DisplayColour = "#" + (sowAction.DisplayColour ?? "000000"),
                    EndDateMonth = new string(sowAction.EndDate.TakeLast(2).ToArray()),
                    EndDateDay = new string(sowAction.EndDate.Take(2).ToArray()),
                    StartDateMonth = new string(sowAction.StartDate.TakeLast(2).ToArray()),
                    StartDateDay = new string(sowAction.StartDate.Take(2).ToArray()),
                };
            }
            else
            {
                model.SowAction = new SeedAction
                {
                    ActionName = "Sow",
                    ActionType = ActionType.Sow,
                    DisplayChar = 'S',
                };
            }

            var harvestAction = seedDetails.FirstOrDefault(x => x.ActionType == ActionType.Harvest);

            if (harvestAction != null)
            {
                model.HarvestAction = new SeedAction
                {
                    ActionId = harvestAction.Id,
                    ActionName = harvestAction.ActionName,
                    ActionType = harvestAction.ActionType,
                    DisplayChar = harvestAction.DisplayChar != null ? sowAction.DisplayChar.First() : 'H',
                    DisplayColour = "#" + (harvestAction.DisplayColour ?? "000000"),
                    EndDateMonth = new string(harvestAction.EndDate.TakeLast(2).ToArray()),
                    EndDateDay = new string(harvestAction.EndDate.Take(2).ToArray()),
                    StartDateMonth = new string(harvestAction.StartDate.TakeLast(2).ToArray()),
                    StartDateDay = new string(harvestAction.StartDate.Take(2).ToArray())
                };
            }
            else
            {
                model.HarvestAction = new SeedAction
                {
                    ActionName = "Harvest",
                    ActionType = ActionType.Harvest,
                    DisplayChar = 'H'
                };
            }

            return model;
        }

        public async Task<IEnumerable<SeedItemModel>> GetSeedList()
        {
            var seeds = await SeedDataAccess.GetAllSeeds();

            return seeds;
        }

        public async Task SaveSeedInfo(SeedDetailModel seed)
        {
            //need to convert to json correctly for sql to process it
            //need to update start and end date to not be year based
            await SeedDataAccess.SaveSeed(JsonConvert.SerializeObject(seed));
        }

        public async Task DeleteSeed(long seedId)
        {
            await SeedDataAccess.DeleteSeed(seedId);
        }
    }
}