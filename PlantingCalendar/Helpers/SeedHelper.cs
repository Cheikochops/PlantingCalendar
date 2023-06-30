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
                ExpiryDate = first.ExpiryDate?.ToString("yyyy/MM/dd"),
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

        public async Task SaveSeedInfo(UploadSeedDetailModel seed)
        {
            //need to convert to json correctly for sql to process it

            if (!DateTime.TryParseExact(seed.ExpiryDate, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.AssumeUniversal, out DateTime date)) {
                throw new Exception("Invalid ExpiryDate format");
            }

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
                    ActionType = ActionType.Custom,
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
                ActionType = ActionType.Sow,
                DisplayChar = seed.SowAction.DisplayChar,
                DisplayColour = seed.SowAction.DisplayColour.TrimStart('#'),
                StartDate = seed.SowAction.StartDateDay + seed.SowAction.StartDateMonth,
                EndDate = seed.SowAction.EndDateDay + seed.SowAction.EndDateMonth
            });

            saveModel.Actions.Add(new SqlSaveSeedAction
            {
                ActionId = seed.HarvestAction.ActionId,
                ActionName = seed.HarvestAction.ActionName,
                ActionType = ActionType.Harvest,
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