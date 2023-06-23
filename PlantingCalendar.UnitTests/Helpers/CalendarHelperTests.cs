using PlantingCalendar.DataAccess;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.UnitTests
{
    public class CalendarHelperTests
    {
        [Fact]
        public void FormatCalendar_NoCalendar()
        {
            var calendarHelper = new CalendarHelper();

            var calendarDetails = new List<SqlCalendarDetailsModel>();

            Assert.Throws<Exception>(() => calendarHelper.FormatCalendar(calendarDetails));
        }

        [Fact]
        public void FormatCalendar_NoSeeds()
        {
            var calendarHelper = new CalendarHelper();
            var calendarDetails = new List<SqlCalendarDetailsModel>() {
                new SqlCalendarDetailsModel()
                {
                    CalendarId = 1,
                    CalendarName = "Test",
                    Year = 2023
                }
            };

            var calendar = calendarHelper.FormatCalendar(calendarDetails);

            Assert.Equal(1, calendar.CalendarId);
            Assert.Equal("Test", calendar.CalendarName);
            Assert.Equal(2023, calendar.Year);
            Assert.Empty(calendar.Seeds);
        }

        [Fact]
        public void FormatCalendar_WithSeeds()
        {
            var calendarHelper = new CalendarHelper();
            var calendarDetails = new List<SqlCalendarDetailsModel>() {
                new SqlCalendarDetailsModel()
                {
                    CalendarId = 1,
                    CalendarName = "Test",
                    Year = 2023,
                    IsComplete = false,
                    SetTaskDate = new DateTime(2023, 03, 31),
                    RangeTaskEndDate = null,
                    RangeTaskStartDate = null,
                    SeedId = 1,
                    PlantBreed = "Masterpiece",
                    PlantTypeName = "Cucumber",
                    TaskId = 1,
                    TaskName = "Sow",
                    TaskDisplayChar = "S",
                    TaskDisplayColour = null,
                    TaskTypeId = 1,
                    TaskDescription = "This is a single task to sow"
                },
                new SqlCalendarDetailsModel()
                {
                    CalendarId = 1,
                    CalendarName = "Test",
                    Year = 2023,
                    IsComplete = false,
                    SeedId = 1,
                    PlantBreed = "Masterpiece",
                    PlantTypeName = "Cucumber",
                    SetTaskDate = null,
                    RangeTaskEndDate = new DateTime(2023, 07, 01),
                    RangeTaskStartDate = new DateTime(2023, 05, 15),
                    TaskId = 2,
                    TaskName = "Sow RANGE",
                    TaskDisplayChar = "R",
                    TaskDisplayColour = null,
                    TaskTypeId = 2,
                    TaskDescription = "This is a ranged task to sow"
                },
                new SqlCalendarDetailsModel()
                {
                    CalendarId = 1,
                    CalendarName = "Test",
                    Year = 2023,
                    IsComplete = false,
                    SeedId = 3,
                    PlantBreed = "Black Russian",
                    PlantTypeName = "Tomato",
                    SetTaskDate = null,
                    RangeTaskEndDate = null,
                    RangeTaskStartDate = null,
                    TaskId = null,
                    TaskName = null,
                    TaskDisplayChar = null,
                    TaskDisplayColour = null,
                    TaskTypeId = null,
                    TaskDescription = null
                }
            };

            var calendar = calendarHelper.FormatCalendar(calendarDetails);

            Assert.Equal(1, calendar.CalendarId);
            Assert.Equal("Test", calendar.CalendarName);
            Assert.Equal(2023, calendar.Year);
            Assert.Equal(2, calendar.Seeds.Count());

            //foreach (var seed in calendar.Seeds)
            //{
            //    Assert.Equal(12, seed.Months.Count());

            //    foreach (var month in seed.Months) {

            //        if (month.Order == 3 && seed.Id == 1)
            //        {
            //            Assert.Single(month.Tasks);
            //            var task = month.Tasks.First();

            //            Assert.Equal("Sow", task.TaskName);
            //            Assert.Equal(1, task.Id);
            //        }
            //        else if (month.Order >= 5 && month.Order <= 7 && seed.Id == 1)
            //        {
            //            Assert.Single(month.Tasks);
            //            var task = month.Tasks.First();

            //            Assert.Equal("Sow RANGE", task.TaskName);
            //            Assert.Equal(2, task.Id);
            //        }
            //        else
            //        {
            //            Assert.Empty(month.Tasks);
            //        }
            //    }
            //}
        }
    }
}