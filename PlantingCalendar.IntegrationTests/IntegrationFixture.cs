using Microsoft.Extensions.Options;
using PlantingCalendar.Controllers;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Models;

namespace PlantingCalendar.UnitTests
{
    public class IntegrationFixture
    {
        private DataAccessSettings DataAccessSettings { get; set; }
        private SeedDataAccess SeedDataAccess { get; set; }
        private CalendarDataAccess CalendarDataAccess { get; set; }
        private TaskDataAccess TaskDataAccess { get; set; }
        private SeedHelper SeedHelper { get; set; }
        private CalendarHelper CalendarHelper { get; set; }
        private TaskHelper TaskHelper { get; set; }
        public SeedController SeedController { get; set; }
        public CalendarController CalendarController { get; set; }
        public TaskController TaskController { get; set; }

        public TestDataAccess TestDataAccess { get; set; }

        public IntegrationFixture ()
        {
            //These tests will assume that you have data in the database.

            DataAccessSettings = new DataAccessSettings()
            {
                Plantbase = "Server=DESKTOP-K7D69O4\\SQLEXPRESS;Database=Plantbase;User Id=sa;Password=Pass.word!;"
            };

            var options = Options.Create(DataAccessSettings);

            SeedDataAccess = new SeedDataAccess(options);
            CalendarDataAccess = new CalendarDataAccess(options);
            TaskDataAccess = new TaskDataAccess(options);

            SeedHelper = new SeedHelper(SeedDataAccess);
            CalendarHelper = new CalendarHelper(CalendarDataAccess);
            TaskHelper = new TaskHelper(TaskDataAccess);

            SeedController = new SeedController(SeedHelper);
            CalendarController = new CalendarController(CalendarHelper);
            TaskController = new TaskController(TaskHelper);

            TestDataAccess = new TestDataAccess(options);
        }
    }
}