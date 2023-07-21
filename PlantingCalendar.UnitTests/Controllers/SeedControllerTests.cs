using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using PlantingCalendar.Controllers;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Reflection;

namespace PlantingCalendar.UnitTests
{
    public class SeedControllerTests
    {
        private readonly Mock<ISeedHelper> _seedHelper;
        private readonly SeedController _seedController;

        public SeedControllerTests()
        {
            _seedHelper = new Mock<ISeedHelper>();
            _seedController = new SeedController(_seedHelper.Object);
        }

        [Fact]
        public async Task GetSeedInfo()
        {
            var seedId = long.MaxValue;

            var seedDetail = new SeedDetailModel
            {
                Id = seedId
            };

            _seedHelper.Setup(x => x.GetFormatedSeedItem(seedId, true))
                .ReturnsAsync(seedDetail)
                .Verifiable();

            var result = await _seedController.GetSeedInfo(seedId);

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal(seedDetail, ((OkObjectResult)result).Value);

            _seedHelper.Verify();
        }

        [Fact]
        public async Task GetSeedsList()
        {
            var seedItems = new List<SeedItemModel>
            {
                new SeedItemModel
                {
                    Id = 1
                },
                new SeedItemModel
                {
                    Id = 2
                }
            };

            _seedHelper.Setup(x => x.GetSeedList())
                .ReturnsAsync(seedItems)
                .Verifiable();

            var result = await _seedController.GetSeedsList();

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal(seedItems, ((OkObjectResult)result).Value);

            _seedHelper.Verify();
        }

        [Fact]
        public async Task UpdateSeedInfo_Ok()
        {
            var seedItems = new UploadSeedDetailModel 
            {
                Id = 1
            };

            _seedHelper.Setup(x => x.SaveSeedInfo(seedItems))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _seedController.UpdateSeedInfo(seedItems);
            
            Assert.Equal(typeof(OkResult), result.GetType());

            _seedHelper.Verify();
        }

        [Fact]
        public async Task UpdateSeedInfo_BadRequest()
        {
            var seedItems = new UploadSeedDetailModel
            {
                Id = 1
            };

            _seedHelper.Setup(x => x.SaveSeedInfo(seedItems))
                .Throws(new ValidationException("Uhoh"))
                .Verifiable();

            var result = await _seedController.UpdateSeedInfo(seedItems);

            Assert.Equal(typeof(BadRequestResult), result.GetType());

            _seedHelper.Verify();
        }

        [Fact]
        public async Task DeleteSeedInfo_Ok()
        {
            var seedId = long.MaxValue;

            _seedHelper.Setup(x => x.DeleteSeed(seedId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _seedController.DeleteSeedInfo(seedId);

            Assert.Equal(typeof(OkResult), result.GetType());

            _seedHelper.Verify();
        }

        [Fact]
        public async Task DeleteSeedInfo_BadRequest()
        {
            var seedId = long.MaxValue;

            _seedHelper.Setup(x => x.DeleteSeed(seedId))
                .Throws(new ValidationException("Uhoh"))
                .Verifiable();

            var result = await _seedController.DeleteSeedInfo(seedId);

            Assert.Equal(typeof(BadRequestResult), result.GetType());

            _seedHelper.Verify();
        }

    }
}