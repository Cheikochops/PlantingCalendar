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

        public async Task<SeedDetailModel> GetFormatedSeedItem(long seedId, bool includeDeleted)
        {
            var seedDetails = await SeedDataAccess.GetSeedDetails(seedId, includeDeleted).ConfigureAwait(false);

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
                ExpiryDate = first.ExpiryDate != null ? first.ExpiryDate.Value.ToString("yyyy-MM-dd") : null,
                SunRequirement = first.SunRequirement,
                WaterRequirement = first.WaterRequirement
            };

            model = AddManadatoryActions(seedDetails, model);

            if (seedDetails.Any(x => x.ActionId != null))
            {
                model.Actions = seedDetails.Where(x => x.ActionType == ActionTypeEnum.Custom).Select(x => new SeedAction
                {
                    ActionId = x.Id,
                    ActionName = x.ActionName,
                    ActionDescription = x.ActionDescription,
                    ActionType = x.ActionType,
                    IsDisplay = x.IsDisplay,
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
            var sowAction = seedDetails.FirstOrDefault(x => x.ActionType == ActionTypeEnum.Sow);

            if (sowAction != null)
            {
                model.SowAction = new SeedAction
                {
                    ActionId = sowAction.ActionId,
                    ActionName = sowAction.ActionName,
                    ActionDescription = sowAction.ActionDescription,
                    ActionType = sowAction.ActionType,
                    DisplayChar = sowAction.DisplayChar != null ? sowAction.DisplayChar.First() : 'S',
                    IsDisplay = true,
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
                    ActionDescription = "Sow the seed",
                    ActionType = ActionTypeEnum.Sow,
                    IsDisplay = true,
                    DisplayChar = 'S',
                    DisplayColour = "#000000"
                };
            }

            var harvestAction = seedDetails.FirstOrDefault(x => x.ActionType == ActionTypeEnum.Harvest);

            if (harvestAction != null)
            {
                model.HarvestAction = new SeedAction
                {
                    ActionId = harvestAction.ActionId,
                    ActionName = harvestAction.ActionName,
                    ActionDescription = harvestAction.ActionDescription,
                    ActionType = harvestAction.ActionType,
                    IsDisplay = true,
                    DisplayChar = harvestAction.DisplayChar != null ? harvestAction.DisplayChar.First() : 'H',
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
                    ActionDescription = "Harvest the crop",
                    ActionType = ActionTypeEnum.Harvest,
                    IsDisplay = true,
                    DisplayChar = 'H',
                    DisplayColour = "#000000"
                };
            }

            return model;
        }

        public async Task<IEnumerable<SeedItemModel>> GetSeedList()
        {
            var seeds = await SeedDataAccess.GetAllSeeds();

            return seeds;
        }

        public async Task SaveSeedInfo(UploadSeedDetailModel seed)
        {
            var saveModel = new SqlSaveSeedModel
            {
                Id = seed.Id,
                PlantType = seed.PlantType,
                Breed = seed.Breed,
                SunRequirement = seed.SunRequirement,
                WaterRequirement = seed.WaterRequirement,
                Description = seed.Description,
                ExpiryDate = seed.ExpiryDate,
                Actions = seed.Actions.Select(x => new SqlSaveSeedAction
                {
                    ActionId = x.ActionId,
                    ActionName = x.ActionName,
                    ActionDescription = x.ActionDescription,
                    ActionType = ActionTypeEnum.Custom,
                    IsDisplay = x.IsDisplay,
                    DisplayChar = x.DisplayChar,
                    DisplayColour = x.DisplayColour.TrimStart('#'),
                    StartDate = x.StartDateDay + x.StartDateMonth,
                    EndDate = x.EndDateDay + x.EndDateMonth
                }).ToList()
            };

            saveModel.Actions.Add(new SqlSaveSeedAction
            {
                ActionId = seed.SowAction.ActionId,
                ActionName = seed.SowAction.ActionName,
                ActionDescription = seed.SowAction.ActionDescription,
                ActionType = ActionTypeEnum.Sow,
                IsDisplay = seed.SowAction.IsDisplay,
                DisplayChar = seed.SowAction.DisplayChar,
                DisplayColour = seed.SowAction.DisplayColour.TrimStart('#'),
                StartDate = seed.SowAction.StartDateDay + seed.SowAction.StartDateMonth,
                EndDate = seed.SowAction.EndDateDay + seed.SowAction.EndDateMonth
            });

            saveModel.Actions.Add(new SqlSaveSeedAction
            {
                ActionId = seed.HarvestAction.ActionId,
                ActionName = seed.HarvestAction.ActionName,
                ActionDescription = seed.HarvestAction.ActionDescription,
                ActionType = ActionTypeEnum.Harvest,
                IsDisplay = seed.HarvestAction.IsDisplay,
                DisplayChar = seed.HarvestAction.DisplayChar,
                DisplayColour = seed.HarvestAction.DisplayColour.TrimStart('#'),
                StartDate = seed.HarvestAction.StartDateDay + seed.HarvestAction.StartDateMonth,
                EndDate = seed.HarvestAction.EndDateDay + seed.HarvestAction.EndDateMonth
            });


            await SeedDataAccess.SaveSeed(JsonConvert.SerializeObject(saveModel));
        }

        public async Task DeleteSeed(long seedId)
        {
            await SeedDataAccess.DeleteSeed(seedId);
        }
    }
}