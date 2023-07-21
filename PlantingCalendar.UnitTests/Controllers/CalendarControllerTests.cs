using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Reflection;

namespace PlantingCalendar.UnitTests
{
    public class CalendarControllerTests
    {
        private readonly Mock<ICalendarHelper> _calendarHelper;
        private readonly CalendarController _calendarController;

        public CalendarControllerTests ()
        {
            _calendarHelper = new Mock<ICalendarHelper>();
            _calendarController = new CalendarController(_calendarHelper.Object);
        }

        [Fact]
        public async Task GetCalendar()
        {
            var calendarId = long.MaxValue;

            var calendarDetails = new CalendarDetailsModel
            {
                CalendarId = calendarId
            };

            _calendarHelper.Setup(x => x.FormatCalendar(calendarId))
                .ReturnsAsync(calendarDetails)
                .Verifiable();

            var result = await _calendarController.GetCalendar(calendarId);

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal(calendarDetails, ((OkObjectResult)result).Value);

            _calendarHelper.Verify();
        }

        [Fact]
        public async Task UpdateCalendarSeeds()
        {
            var calendarId = long.MaxValue;
            var seedIds = new[] { 1, 2, long.MaxValue };

            _calendarHelper.Setup(x => x.UpdateCalendarSeeds(calendarId, It.IsAny<List<long>>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _calendarController.UpdateCalendarSeeds(calendarId, seedIds);

            Assert.Equal(typeof(OkResult), result.GetType());

            _calendarHelper.Verify();
        }

        [Fact]
        public async Task GenerateCalendar()
        {
            var generateCalendar = new GenerateCalendarModel
            {
                CalendarName = "Test",
                CalendarYear = 2023
            };

            var calendarId = long.MaxValue;

            _calendarHelper.Setup(x => x.GenerateCalendar(generateCalendar))
                .ReturnsAsync(calendarId)
                .Verifiable();

            var result = await _calendarController.GenerateCalendar(generateCalendar);

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal(calendarId, ((OkObjectResult)result).Value);

            _calendarHelper.Verify();
        }
    }
}