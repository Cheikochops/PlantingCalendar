using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Net;
using Xunit;

namespace PlantingCalendar.UnitTests
{
    public class CalendarTests
    {
        private readonly IntegrationFixture _integrationFixture;

        public CalendarTests ()
        {
            _integrationFixture = new IntegrationFixture();
        }

        [Fact]
        public async Task RunIntegrationCalendarTests()
        {
            try
            {
                await _integrationFixture.TestDataAccess.SetupTestData();

                var calendar = await GetCalendar(_integrationFixture.TestDataAccess.CalendarId, "IntegrationTest: Greenhouse Calendar");
                Assert.Single(calendar.Seeds);
                Assert.Equal(_integrationFixture.TestDataAccess.SeedIds.First(), calendar.Seeds.First().Id);

                await UpdateCalendarSeeds(_integrationFixture.TestDataAccess.CalendarId, new long[] { _integrationFixture.TestDataAccess.SeedIds.Last() });
                calendar = await GetCalendar(_integrationFixture.TestDataAccess.CalendarId, "IntegrationTest: Greenhouse Calendar");
                Assert.Single(calendar.Seeds);
                Assert.Equal(_integrationFixture.TestDataAccess.SeedIds.First(), calendar.Seeds.Last().Id);


                var newCalendar = new GenerateCalendarModel
                {
                    CalendarName = "IntegrationTest: NewTestCalendar",
                    CalendarYear = 2023,
                    Seeds = _integrationFixture.TestDataAccess.SeedIds
                };

                var calendarId = await GenerateNewCalendar(newCalendar);
                calendar = await GetCalendar(calendarId, newCalendar.CalendarName);
                foreach (var seed in _integrationFixture.TestDataAccess.SeedIds)
                {
                    Assert.Contains(seed, calendar.Seeds.Select(x => x.Id));
                }
                //Add soemthing to remove calendar + calendarseed + task
            }
            finally
            {
                await _integrationFixture.TestDataAccess.RemoveTestData();
            }

        }

        private async Task<CalendarDetailsModel> GetCalendar(long calendarId, string expectedCalendarName)
        {
            var result = await _integrationFixture.CalendarController.GetCalendar(calendarId);

            Assert.Equal(typeof(OkObjectResult), result.GetType());

            var calendar = (result as OkObjectResult).Value as CalendarDetailsModel;
  
            Assert.Equal(calendarId, calendar.CalendarId);
            Assert.Equal(2023, calendar.Year);
            Assert.Equal(expectedCalendarName, calendar.CalendarName);        
            Assert.Equal(12, calendar.Months.Count());

            return calendar;
        }

        private async Task UpdateCalendarSeeds(long calendarId, long[] seedIds)
        {
            var result = await _integrationFixture.CalendarController.UpdateCalendarSeeds(calendarId, seedIds);

            Assert.Equal(typeof(OkResult), result.GetType());
        }

        private async Task<long> GenerateNewCalendar(GenerateCalendarModel newCalendarModel)
        {
            var result = await _integrationFixture.CalendarController.GenerateCalendar(newCalendarModel);

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            var calendar = (result as OkObjectResult).Value as SqlIdModel;

            return calendar.Id.Value;
        }
    }
}