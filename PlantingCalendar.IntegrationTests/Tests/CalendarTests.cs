using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Models;
using System.Net;
using Xunit;

namespace PlantingCalendar.UnitTests
{
    public class CalendarTests
    {
        [Fact]
        public async Task GetCalendar()
        {
            var fixture = new IntegrationFixture();

            try
            {
                await fixture.TestDataAccess.SetupTestData();

                var calendarIds = await fixture.TestDataAccess.GetTestCalendarIds();
                var calendarId = calendarIds.First(x => x.Name.Contains("Current"));

                var calendarDetails = await fixture.CalendarController.GetCalendar(calendarId.Id);

                Assert.IsType<OkObjectResult>(calendarDetails);

                var okObjectResult = calendarDetails as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var data = okObjectResult.Value as CalendarDetailsModel;
                Assert.NotNull(data);

                Assert.Equal(12, data.Months.Count());
                Assert.Equal(2023, data.Year);
                Assert.Single(data.Seeds);
                Assert.Equal(calendarId.Name, data.CalendarName);
                Assert.Equal(calendarId.Id, data.CalendarId);
            }
            finally
            {
                await fixture.TestDataAccess.RemoveTestData();
            }
        }
    }
}