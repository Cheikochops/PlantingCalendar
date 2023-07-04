using Microsoft.AspNetCore.Mvc;
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
                var calendarId = calendarIds.First();

                var calendarDetails = await fixture.CalendarController.GetCalendar(calendarId);

                Assert.IsType<OkResult>(calendarDetails);

                var data = (OkObjectResult)calendarDetails;

       

            }
            finally
            {
                await fixture.TestDataAccess.RemoveTestData();
            }
        }
    }
}