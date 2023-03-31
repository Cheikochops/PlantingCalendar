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
        public void FormatCalendar_NoTasks()
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
            Assert.Equal(12, calendar.Months.Count());

            foreach (var month in calendar.Months)
            {
                Assert.Empty(month.Tasks);
            }
        }

        [Fact]
        public void FormatCalendar_WithTasks()
        {
            var calendarHelper = new CalendarHelper();
            var calendarDetails = new List<SqlCalendarDetailsModel>() {
                new SqlCalendarDetailsModel()
                {
                    CalendarId = 1,
                    CalendarName = "Test",
                    Year = 2023,
                    IsComplete = false,
                    TaskDate = new DateOnly(2023, 03, 31),
                    TaskEndDate = null,
                    TaskStartDate = null,
                    TaskId = 1,
                    TaskTypeName = "Sow",
                    DisplayChar = 'S',
                    DisplayColour = null,
                    TaskTypeId = 1,
                    TaskTypeDescription = "This is a single task to sow"
                },
                new SqlCalendarDetailsModel()
                {
                    CalendarId = 1,
                    CalendarName = "Test",
                    Year = 2023,
                    IsComplete = false,
                    TaskDate = null,
                    TaskEndDate = new DateOnly(2023, 07, 01),
                    TaskStartDate = new DateOnly(2023, 05, 15),
                    TaskId = 2,
                    TaskTypeName = "Sow RANGE",
                    DisplayChar = 'R',
                    DisplayColour = null,
                    TaskTypeId = 2,
                    TaskTypeDescription = "This is a ranged task to sow"
                }
            };

            var calendar = calendarHelper.FormatCalendar(calendarDetails);

            Assert.Equal(1, calendar.CalendarId);
            Assert.Equal("Test", calendar.CalendarName);
            Assert.Equal(2023, calendar.Year);
            Assert.Equal(12, calendar.Months.Count());

            foreach (var month in calendar.Months)
            {
                if (month.Order == 3)
                {
                    Assert.Single(month.Tasks);
                    var task = month.Tasks.First();

                    Assert.Equal("Sow", task.TaskName);
                    Assert.Equal(1, task.Id);
                }
                else if (month.Order >= 5 && month.Order <= 7)
                {
                    Assert.Single(month.Tasks);
                    var task = month.Tasks.First();

                    Assert.Equal("Sow RANGE", task.TaskName);
                    Assert.Equal(2, task.Id);
                }
                else
                {
                    Assert.Empty(month.Tasks);
                }
            }
        }
    }
}