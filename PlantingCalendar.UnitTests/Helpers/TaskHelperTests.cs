using Moq;
using Newtonsoft.Json;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.UnitTests
{
    public class TaskHelperTests
    {
        private readonly Mock<ITaskDataAccess> _taskDataAccessMock;
        private readonly Mock<ICalendarDataAccess> _calendarDataAccessMock;
        private readonly ITaskHelper _taskHelper;

        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Strict);

        public TaskHelperTests()
        {
            _taskDataAccessMock = _mockRepository.Create<ITaskDataAccess>();
            _calendarDataAccessMock = _mockRepository.Create<ICalendarDataAccess>();

            _taskHelper = new TaskHelper(_taskDataAccessMock.Object, _calendarDataAccessMock.Object);
        }

        [Fact]
        public async Task CreateNewTask_RangedTask()
        {
            var newTask = new UploadNewTask()
            {
                CalendarId = 1,
                IsDisplay = true,
                DisplayChar = "A",
                DisplayColour = "#000000",
                Name = "Test Task",
                IsRanged = true,
                RangeStartDate = new DateTime(2023, 07, 20),
                RangeEndDate = new DateTime(2023, 09, 10),
                Seeds = new List<long>
                {
                    1
                },
                Description = "This is a task"
            };

            _calendarDataAccessMock.Setup(x => x.GetCalendar(newTask.CalendarId))
                .ReturnsAsync(new List<SqlCalendarDetailsModel>
                {
                    new SqlCalendarDetailsModel
                    {
                        Year = 2023
                    }
                }).Verifiable();

            _taskDataAccessMock.Setup(x => x.CreateTask(newTask.CalendarId, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<long, string>((id, json) =>
                {
                    var tasks = JsonConvert.DeserializeObject<List<SqlSaveNewTaskModel>>(json);

                    Assert.Single(tasks);
                    var task = tasks.First();

                    Assert.Equal(newTask.RangeStartDate, task.RangeStartDate);
                    Assert.Equal(newTask.RangeEndDate, task.RangeEndDate);
                    Assert.True(task.IsRanged);
                    Assert.True(task.IsDisplay);
                    Assert.Equal(newTask.Name, task.Name);
                    Assert.Equal(1, task.SeedId);
                }).Verifiable();

            await _taskHelper.CreateNewTask(newTask);

            _mockRepository.Verify();
        }
    }
}