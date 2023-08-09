using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Models;
using Xunit;

namespace PlantingCalendar.UnitTests
{
    public class SeedTests
    {
        private readonly IntegrationFixture _integrationFixture;

        public SeedTests()
        {
            _integrationFixture = new IntegrationFixture();
        }

        [Fact]
        public async Task RunIntegrationSeedTests()
        {
            try
            {
                await _integrationFixture.TestDataAccess.SetupTestData();

                var seedId = _integrationFixture.TestDataAccess.SeedIds.First();

                var result = await _integrationFixture.SeedController.GetSeedInfo(seedId);

                Assert.Equal(typeof(OkObjectResult), result.GetType());
                var seed = (result as OkObjectResult).Value as SeedDetailModel;

                Assert.Equal(seedId, seed.Id);
                Assert.Equal("IntegrationTest: Tomato", seed.PlantType);
                Assert.Equal("2023-05-10", seed.ExpiryDate);

                result = await _integrationFixture.SeedController.GetSeedsList();
                Assert.Equal(typeof(OkObjectResult), result.GetType());
                var seedList = (result as OkObjectResult).Value as List<SeedItemModel>;

                Assert.True(seedList.Count() >= 2);
                Assert.True(seedList.Any(x => x.Id == seedId));

                var editSeed = new UploadSeedDetailModel
                {
                    Id = seedId,
                    PlantType = "IntegrationTest: Tomato Updated",
                    Breed = "Test Breed",
                    ExpiryDate = null,
                    SunRequirement = "Test",
                    WaterRequirement = "Test",
                    SowAction = new UploadSeedAction
                    {
                        ActionName = "Sow",
                        StartDateDay = "05",
                        StartDateMonth = "05",
                        EndDateDay = "06",
                        EndDateMonth = "06"
                    },
                    HarvestAction = new UploadSeedAction
                    {
                        ActionName = "Harvest",
                        StartDateDay = "07",
                        StartDateMonth = "08",
                        EndDateDay = "08",
                        EndDateMonth = "09"
                    }
                };

                result = await _integrationFixture.SeedController.UpdateSeedInfo(editSeed);
                Assert.Equal(typeof(OkResult), result.GetType());

                var newSeed = new UploadSeedDetailModel
                {
                    Id = null,
                    PlantType = "IntegrationTest: New Seed",
                    Breed = "Test Breed",
                    ExpiryDate = null,
                    SunRequirement = "Test",
                    WaterRequirement = "Test",
                    SowAction = new UploadSeedAction
                    {
                        ActionName = "Sow",
                        StartDateDay = "05",
                        StartDateMonth = "05",
                        EndDateDay = "06",
                        EndDateMonth = "06"
                    },
                    HarvestAction = new UploadSeedAction
                    {
                        ActionName = "Harvest",
                        StartDateDay = "07",
                        StartDateMonth = "08",
                        EndDateDay = "08",
                        EndDateMonth = "09"
                    }
                };

                result = await _integrationFixture.SeedController.UpdateSeedInfo(newSeed);
                Assert.Equal(typeof(OkResult), result.GetType());

                result = await _integrationFixture.SeedController.GetSeedsList();
                Assert.Equal(typeof(OkObjectResult), result.GetType());
                seedList = (result as OkObjectResult).Value as List<SeedItemModel>;

                var tomato = seedList.FirstOrDefault(x => x.Id == seedId);
                var newSeedItem = seedList.FirstOrDefault(x => x.PlantType == newSeed.PlantType && x.Breed == newSeed.Breed);

                Assert.NotNull(tomato);
                Assert.NotNull(newSeedItem);

                Assert.Equal(editSeed.Breed, tomato.Breed);
                Assert.Equal(editSeed.PlantType, tomato.PlantType);
                Assert.False(tomato.IsExpired);

                Assert.Equal(newSeed.Breed, newSeedItem.Breed);
                Assert.Equal(newSeed.PlantType, newSeedItem.PlantType);
                Assert.False(tomato.IsExpired);

                result = await _integrationFixture.SeedController.DeleteSeedInfo(newSeedItem.Id);
                Assert.Equal(typeof(OkResult), result.GetType());

                result = await _integrationFixture.SeedController.GetSeedsList();
                Assert.Equal(typeof(OkObjectResult), result.GetType());
                seedList = (result as OkObjectResult).Value as List<SeedItemModel>;
                var newerSeedItem = seedList.FirstOrDefault(x => x.PlantType == newSeed.PlantType && x.Breed == newSeed.Breed);

                Assert.Null(newerSeedItem);
                await _integrationFixture.TestDataAccess.RemoveSeed(newSeedItem.Id);

            }
            finally
            {
                await _integrationFixture.TestDataAccess.RemoveTestData();
            }
        }
    }
}