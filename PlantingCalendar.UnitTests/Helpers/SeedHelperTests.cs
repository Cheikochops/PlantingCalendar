using Moq;
using Newtonsoft.Json;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.UnitTests
{
    public class SeedHelperTests
    {
        [Fact]
        public async Task GetFormatedSeedItem_NoSeeds()
        {
            var seedDataAccess = new Mock<ISeedDataAccess>(MockBehavior.Strict);
            var helper = new SeedHelper(seedDataAccess.Object);

            var seedId = 1;
            var includeDeleted = false;

            seedDataAccess.Setup(x => x.GetSeedDetails(seedId, includeDeleted))
                .ReturnsAsync(new List<SqlSeedDetailsModel>())
                .Verifiable();

            await Assert.ThrowsAnyAsync<Exception>(async () => await helper.GetFormatedSeedItem(seedId, includeDeleted));
        }

        [Fact]
        public async Task GetFormatedSeedItem_NoActions()
        {
            var seedDataAccess = new Mock<ISeedDataAccess>(MockBehavior.Strict);
            var helper = new SeedHelper(seedDataAccess.Object);

            var seedId = 1;
            var includeDeleted = false;

            var sqlSeedDetails = new List<SqlSeedDetailsModel>()
            {
                new SqlSeedDetailsModel
                {
                    Id = seedId,
                    PlantType = "Tomato",
                    Breed = "Masterpiece",
                    Description = "This is a big tomato variety",
                    SunRequirement = "Full Sun",
                    WaterRequirement = "2 Inches a Week",
                    ExpiryDate = new DateTime(2022, 01, 01)
                }
            };

            seedDataAccess.Setup(x => x.GetSeedDetails(seedId, includeDeleted))
                .ReturnsAsync(sqlSeedDetails)
                .Verifiable();

            var seeds = await helper.GetFormatedSeedItem(seedId, includeDeleted);

            var first = sqlSeedDetails.First();

            Assert.Equal(seedId, seeds.Id);
            Assert.Equal(first.PlantType, seeds.PlantType);
            Assert.Equal(first.Breed, seeds.Breed);
            Assert.Equal(first.SunRequirement, seeds.SunRequirement);
            Assert.Equal(first.WaterRequirement, seeds.WaterRequirement);
            Assert.Equal(first.ExpiryDate.Value.ToString("yyyy-MM-dd"), seeds.ExpiryDate);

            Assert.Empty(seeds.Actions);
            Assert.Equal("Sow", seeds.SowAction.ActionName);
            Assert.Equal("#000000", seeds.SowAction.DisplayColour);
            Assert.Equal('S', seeds.SowAction.DisplayChar);
            Assert.Equal("Harvest", seeds.HarvestAction.ActionName);
            Assert.Equal("#000000", seeds.HarvestAction.DisplayColour);
            Assert.Equal('H', seeds.HarvestAction.DisplayChar);
        }

        [Fact]
        public async Task GetFormatedSeedItem_ExistingActions()
        {
            var seedDataAccess = new Mock<ISeedDataAccess>(MockBehavior.Strict);
            var helper = new SeedHelper(seedDataAccess.Object);

            var seedId = 1;
            var includeDeleted = false;

            var sqlSeedDetails = new List<SqlSeedDetailsModel>()
            {
                new SqlSeedDetailsModel
                {
                    Id = seedId,
                    PlantType = "Tomato",
                    Breed = "Masterpiece",
                    Description = "This is a big tomato variety",
                    SunRequirement = "Full Sun",
                    WaterRequirement = "2 Inches a Week",
                    ExpiryDate = new DateTime(2022, 01, 01),
                    ActionType = ActionTypeEnum.Sow,
                    ActionId = 1,
                    ActionName = "Sow",
                    DisplayChar = "S",
                    DisplayColour = "000121",
                    StartDate = "0106",
                    EndDate = "1508"
                },
                new SqlSeedDetailsModel
                {
                    Id = seedId,
                    PlantType = "Tomato",
                    Breed = "Masterpiece",
                    Description = "This is a big tomato variety",
                    SunRequirement = "Full Sun",
                    WaterRequirement = "2 Inches a Week",
                    ExpiryDate = new DateTime(2022, 01, 01),
                    ActionType = ActionTypeEnum.Harvest,
                    ActionId = 2,
                    ActionName = "Harvest",
                    DisplayChar = "H",
                    DisplayColour = "220112",
                    StartDate = "1010",
                    EndDate = "1012"
                },
                new SqlSeedDetailsModel
                {
                    Id = seedId,
                    PlantType = "Tomato",
                    Breed = "Masterpiece",
                    Description = "This is a big tomato variety",
                    SunRequirement = "Full Sun",
                    WaterRequirement = "2 Inches a Week",
                    ExpiryDate = new DateTime(2022, 01, 01),
                    ActionType = ActionTypeEnum.Custom,
                    ActionId = 3,
                    ActionName = "Trim Plants",
                    DisplayChar = "T",
                    DisplayColour = "333333",
                    StartDate = "0909",
                    EndDate = "0909"
                }
            };

            seedDataAccess.Setup(x => x.GetSeedDetails(seedId, includeDeleted))
                .ReturnsAsync(sqlSeedDetails)
                .Verifiable();

            var seeds = await helper.GetFormatedSeedItem(seedId, includeDeleted);

            var first = sqlSeedDetails.First();

            Assert.Equal(seedId, seeds.Id);
            Assert.Equal(first.PlantType, seeds.PlantType);
            Assert.Equal(first.Breed, seeds.Breed);
            Assert.Equal(first.SunRequirement, seeds.SunRequirement);
            Assert.Equal(first.WaterRequirement, seeds.WaterRequirement);
            Assert.Equal(first.ExpiryDate.Value.ToString("yyyy-MM-dd"), seeds.ExpiryDate);

            Assert.Single(seeds.Actions);
            Assert.Equal("Sow", seeds.SowAction.ActionName);
            Assert.Equal("#000121", seeds.SowAction.DisplayColour);
            Assert.Equal('S', seeds.SowAction.DisplayChar);
            Assert.Equal("01", seeds.SowAction.StartDateDay);
            Assert.Equal("06", seeds.SowAction.StartDateMonth);
            Assert.Equal("15", seeds.SowAction.EndDateDay);
            Assert.Equal("08", seeds.SowAction.EndDateMonth);

            Assert.Equal("Harvest", seeds.HarvestAction.ActionName);
            Assert.Equal("#220112", seeds.HarvestAction.DisplayColour);
            Assert.Equal('H', seeds.HarvestAction.DisplayChar);
            Assert.Equal("10", seeds.HarvestAction.StartDateDay);
            Assert.Equal("10", seeds.HarvestAction.StartDateMonth);
            Assert.Equal("10", seeds.HarvestAction.EndDateDay);
            Assert.Equal("12", seeds.HarvestAction.EndDateMonth);

            var trimSeed = seeds.Actions.First();
            Assert.Equal("Trim Plants", trimSeed.ActionName);
            Assert.Equal("#333333", trimSeed.DisplayColour);
            Assert.Equal('T', trimSeed.DisplayChar);
            Assert.Equal("09", trimSeed.StartDateDay);
            Assert.Equal("09", trimSeed.StartDateMonth);
            Assert.Equal("09", trimSeed.EndDateDay);
            Assert.Equal("09", trimSeed.EndDateMonth);
        }

        [Fact]
        public async Task GetSeedList()
        {
            var seedDataAccess = new Mock<ISeedDataAccess>(MockBehavior.Strict);
            var helper = new SeedHelper(seedDataAccess.Object);

            var list = new List<SeedItemModel>();

            seedDataAccess.Setup(x => x.GetAllSeeds()).ReturnsAsync(list).Verifiable();

            var items = await helper.GetSeedList();

            Assert.Equal(list, items);

            seedDataAccess.Verify();
        }

        [Fact]
        public async Task DeleteSeed()
        {
            var seedDataAccess = new Mock<ISeedDataAccess>(MockBehavior.Strict);
            var helper = new SeedHelper(seedDataAccess.Object);

            var seedId = long.MaxValue;

            seedDataAccess.Setup(x => x.DeleteSeed(seedId))
                .Returns(Task.CompletedTask)
                .Verifiable();  

            await helper.DeleteSeed(seedId);

            seedDataAccess.Verify();
        }

        [Fact]
        public async Task SaveSeedInfo_Submit()
        {
            var seedDataAccess = new Mock<ISeedDataAccess>(MockBehavior.Strict);
            var helper = new SeedHelper(seedDataAccess.Object);

            var seed = new UploadSeedDetailModel()
            {
                Id = 1,
                PlantType = "Cucumber",
                Breed = "Masterpiece",
                Description = "This is a cucumber",
                SunRequirement = "Full Sun",
                WaterRequirement = "2 Inches",
                SowAction = new UploadSeedAction()
                {
                    ActionId = 1,
                    ActionName = "Sow",
                    DisplayChar = 'S',
                    DisplayColour = "#0123456",
                    StartDateDay = "01",
                    StartDateMonth = "02",
                    EndDateDay = "03",
                    EndDateMonth = "04"
                },
                HarvestAction = new UploadSeedAction()
                {
                    ActionId = 2,
                    ActionName = "Harvest",
                    DisplayChar = 'H',
                    DisplayColour = "#098765",
                    StartDateDay = "05",
                    StartDateMonth = "06",
                    EndDateDay = "06",
                    EndDateMonth = "07"
                },
                Actions = new List<UploadSeedAction>()
                {
                    new UploadSeedAction
                    {
                        ActionId = 3,
                        ActionName = "Trim",
                        DisplayChar = 'T',
                        DisplayColour = "#000111",
                        StartDateDay = "09",
                        StartDateMonth = "09",
                        EndDateDay = "09",
                        EndDateMonth = "09"
                    }
                },
                ExpiryDate = new DateTime(2023, 07, 12)
            };

            seedDataAccess.Setup(x => x.SaveSeed(It.IsAny<string>()))
                .Callback<string>((s) =>
                {
                    var model = JsonConvert.DeserializeObject<SqlSaveSeedModel>(s);

                    Assert.Equal(3, model.Actions.Count());
                    Assert.Equal(seed.Id, model.Id);
                    Assert.Equal(seed.SunRequirement, model.SunRequirement);
                    Assert.Equal(seed.WaterRequirement, model.WaterRequirement);
                    Assert.Equal(seed.Description, model.Description);
                    Assert.Equal(seed.ExpiryDate, model.ExpiryDate);
                    Assert.Equal(seed.PlantType, model.PlantType);
                    Assert.Equal(seed.Breed, model.Breed);

                    var sow = model.Actions.First(x => x.ActionType == ActionTypeEnum.Sow);
                    var harvest = model.Actions.First(x => x.ActionType == ActionTypeEnum.Harvest);
                    var trim = model.Actions.First(x => x.ActionType == ActionTypeEnum.Custom);

                    Assert.Equal(1, sow.ActionId);
                    Assert.Equal(2, harvest.ActionId);
                    Assert.Equal(3, trim.ActionId);

                    Assert.Equal("0102", sow.StartDate);
                    Assert.Equal("0304", sow.EndDate);
                    Assert.Equal("0506", harvest.StartDate);
                    Assert.Equal("0607", harvest.EndDate);
                    Assert.Equal("0909", trim.StartDate);
                    Assert.Equal("0909", trim.EndDate);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            await helper.SaveSeedInfo(seed);
        }
    }
}