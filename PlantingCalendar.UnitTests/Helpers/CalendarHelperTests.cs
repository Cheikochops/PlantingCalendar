using Moq;
using Newtonsoft.Json;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Reflection;

namespace PlantingCalendar.UnitTests
{
    public class CalendarHelperTests
    {
        [Fact]
        public void FormatCalendar_NoCalendarDetails()
        {
            var calendarDataAccess = new Mock<ICalendarDataAccess>(MockBehavior.Strict);
            var calendarHelper = new CalendarHelper(calendarDataAccess.Object);

            var calendarDetails = new List<SqlCalendarDetailsModel>();
            var calendarId = long.MaxValue;

            calendarDataAccess.Setup(x => x.GetCalendar(calendarId))
                .ReturnsAsync(calendarDetails)
                .Verifiable();

            Assert.ThrowsAnyAsync<Exception>(async () => await calendarHelper.FormatCalendar(calendarId));

            calendarDataAccess.Verify();
        }

        [Fact]
        public async Task FormatCalendar_TasksAndSeeds()
        {
            var calendarDataAccess = new Mock<ICalendarDataAccess>(MockBehavior.Strict);
            var calendarHelper = new CalendarHelper(calendarDataAccess.Object);

            var calendarDetails = new List<SqlCalendarDetailsModel>()
            {
                new SqlCalendarDetailsModel
                {
                    CalendarId = 1,
                    CalendarName = "Test Calendar",
                    Year = 2022,
                    PlantBreed = "Alisa Craig",
                    PlantTypeName = "Tomato",
                    SeedId = 1,
                    TaskTypeId = 1,
                    TaskName = "Sow",
                    TaskDescription = "Sow a seed",
                    TaskId = 1,
                    TaskDisplayChar = "S",
                    TaskDisplayColour = "00000",
                    RangeTaskEndDate = new DateTime(2022, 06, 10),
                    RangeTaskStartDate = new DateTime(2022, 01, 10),
                    SetTaskDate = null,
                    IsComplete = false
                },
                new SqlCalendarDetailsModel
                {
                    CalendarId = 1,
                    CalendarName = "Test Calendar",
                    Year = 2022,
                    PlantBreed = "Alisa Craig",
                    PlantTypeName = "Tomato",
                    SeedId = 1,
                    TaskTypeId = 2,
                    TaskName = "Harvest",
                    TaskDescription = "Harvest a seed",
                    TaskId = 2,
                    TaskDisplayChar = "S",
                    TaskDisplayColour = "00000",
                    RangeTaskEndDate = new DateTime(2022, 10, 10),
                    RangeTaskStartDate = new DateTime(2022, 08, 10),
                    SetTaskDate = null,
                    IsComplete = false
                },
                new SqlCalendarDetailsModel
                {
                    CalendarId = 1,
                    CalendarName = "Test Calendar",
                    Year = 2022,
                    PlantBreed = "Alisa Craig",
                    PlantTypeName = "Tomato",
                    SeedId = 1,
                    TaskTypeId = 3,
                    TaskName = "Trim Lower Leaves",
                    TaskDescription = "Trim Lower Leaves of plants",
                    TaskId = 3,
                    TaskDisplayChar = "T",
                    TaskDisplayColour = "00000",
                    RangeTaskEndDate = null,
                    RangeTaskStartDate = null,
                    SetTaskDate = new DateTime(2022, 07, 15),
                    IsComplete = false
                },
                new SqlCalendarDetailsModel
                {
                    CalendarId = 1,
                    CalendarName = "Test Calendar",
                    Year = 2022,
                    PlantBreed = "Basic",
                    PlantTypeName = "Celery",
                    SeedId = 2,
                    TaskTypeId = 4,
                    TaskName = "Trim Lower Leaves",
                    TaskDescription = "Trim Lower Leaves of plants",
                    TaskId = 4,
                    TaskDisplayChar = "T",
                    TaskDisplayColour = "00000",
                    RangeTaskEndDate = null,
                    RangeTaskStartDate = null,
                    SetTaskDate = new DateTime(2022, 07, 15),
                    IsComplete = false
                }
            };

            var calendarId = long.MaxValue;

            calendarDataAccess.Setup(x => x.GetCalendar(calendarId))
                .ReturnsAsync(calendarDetails)
                .Verifiable();

            var calendar = await calendarHelper.FormatCalendar(calendarId);

            Assert.Equal("Test Calendar", calendar.CalendarName);
            Assert.Equal(2022, calendar.Year);
            Assert.Equal(1, calendar.CalendarId);

            Assert.Equal(12, calendar.Months.Count());
            Assert.Equal(2, calendar.Seeds.Count());

            var tomatoSeed = calendar.Seeds.First(x => x.Id == 1);
            var celerySeed = calendar.Seeds.First(x => x.Id == 2);
            Assert.Equal(12, tomatoSeed.Tasks.Count());
            Assert.Equal(12, celerySeed.Tasks.Count());

            Assert.Equal(6, tomatoSeed.Tasks.Where(x => x.Value.Any(y => y.Id == 1)).Count());
            Assert.Equal(3, tomatoSeed.Tasks.Where(x => x.Value.Any(y => y.Id == 2)).Count());
            Assert.Single(tomatoSeed.Tasks[7]);

            foreach(var month in calendar.Months)
            {
                Assert.Equal(DateTime.DaysInMonth(calendar.Year, month.Order), month.DayCount);
            }

            calendarDataAccess.Verify();
        }

        [Fact]
        public async Task GenerateCalendar()
        {
            var model = new GenerateCalendarModel()
            {
                CalendarName = "Test Calendar",
                CalendarYear = 2022,
                Seeds = new List<long>
                {
                    1,
                    5,
                    40
                }
            };

            var calendarDataAccess = new Mock<ICalendarDataAccess>(MockBehavior.Strict);
            calendarDataAccess.Setup(x =>
                x.GenerateNewCalendar(model.CalendarName,
                model.CalendarYear,
                JsonConvert.SerializeObject(model.Seeds)))
                .ReturnsAsync(long.MaxValue)
                .Verifiable();

            var calendarHelper = new CalendarHelper(calendarDataAccess.Object);

            var calendarId = await calendarHelper.GenerateCalendar(model);
            Assert.Equal(long.MaxValue, calendarId);

            calendarDataAccess.Verify();
        }

        [Fact]
        public async Task RemoveSeedFromCalendar()
        {
            var seedId = long.MaxValue;
            var calendarId = long.MaxValue - 5;

            var calendarDataAccess = new Mock<ICalendarDataAccess>(MockBehavior.Strict);
            calendarDataAccess.Setup(x => x.RemoveSeedFromCalendar(seedId, calendarId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var calendarHelper = new CalendarHelper(calendarDataAccess.Object);

            await calendarHelper.RemoveSeedFromCalendar(seedId, calendarId);

            calendarDataAccess.Verify();
        }

        [Fact]
        public async Task AddSeedToCalendar()
        {
            var seedId = long.MaxValue;
            var calendarId = long.MaxValue - 5;

            var calendarDataAccess = new Mock<ICalendarDataAccess>(MockBehavior.Strict);
            calendarDataAccess.Setup(x => x.AddSeedToCalendar(seedId, calendarId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var calendarHelper = new CalendarHelper(calendarDataAccess.Object);

            await calendarHelper.AddSeedToCalendar(seedId, calendarId);

            calendarDataAccess.Verify();
        }
    }
}