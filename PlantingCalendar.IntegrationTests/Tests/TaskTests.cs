using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Models;
using Xunit;

namespace PlantingCalendar.UnitTests
{
    public class TaskTests
    {
        private readonly IntegrationFixture _integrationFixture;

        public TaskTests()
        {
            _integrationFixture = new IntegrationFixture();
        }

        [Fact]
        public async Task RunIntegrationTaskTests()
        {
            try
            {
                await _integrationFixture.TestDataAccess.SetupTestData();

                var result = await _integrationFixture.TaskController.GetTaskTypes();
                Assert.Equal(typeof(OkObjectResult), result.GetType());
                var types = (result as OkObjectResult).Value as List<string>;

                Assert.Equal(4, types.Count());

                var seedId = _integrationFixture.TestDataAccess.SeedIds.First();

                var newTask = new UploadNewTask()
                {
                    CalendarId = _integrationFixture.TestDataAccess.CalendarId,
                    Description = "Integration Test: New TestTask",
                    IsRanged = true,
                    RangeStartDate = "2023-01-01",
                    RangeEndDate = "2023-02-02",
                    IsDisplay = false,
                    Name = "Integration Test: New TestTask",
                    Seeds = new List<long> { seedId }
                };

                result = await _integrationFixture.TaskController.CreateNewTask(newTask);
                Assert.Equal(typeof(OkResult), result.GetType());

                var updatedTask = new UploadTaskDetails()
                {
                    TaskDescription = "Updated TestTask",
                    IsRanged = true,
                    TaskStartDate = "2023-05-01",
                    TaskEndDate = "2023-06-02",
                    IsDisplay = false,
                    TaskName = "Updated TestTask",
                };

                result = await _integrationFixture.TaskController.UpdateTask(_integrationFixture.TestDataAccess.CalendarId, _integrationFixture.TestDataAccess.Tasks.First(), updatedTask);
                Assert.Equal(typeof(OkResult), result.GetType());

                var deletedTask = _integrationFixture.TestDataAccess.Tasks.Last();
                result = await _integrationFixture.TaskController.DeleteTask(deletedTask);
                Assert.Equal(typeof(OkResult), result.GetType());

                var completedTask = _integrationFixture.TestDataAccess.Tasks[1];
                result = await _integrationFixture.TaskController.ToggleCompleteTask(completedTask);
                Assert.Equal(typeof(OkResult), result.GetType());

                result = await _integrationFixture.CalendarController.GetCalendar(_integrationFixture.TestDataAccess.CalendarId);
                Assert.Equal(typeof(OkObjectResult), result.GetType());
                var calendar = (result as OkObjectResult).Value as CalendarDetailsModel;

                Assert.False(calendar.Seeds.First(x => x.Id == seedId).Tasks.SelectMany(x => x.Value).Any(x => x.Id == deletedTask));
                var calendarNewTask = calendar.Seeds.First(x => x.Id == seedId).Tasks.SelectMany(x => x.Value).First(x => x.TaskName == newTask.Name);
                Assert.NotNull(calendarNewTask);
                Assert.False(calendarNewTask.IsDisplay);

                var calendarEditedTask = calendar.Seeds.First(x => x.Id == seedId).Tasks.SelectMany(x => x.Value).First(x => x.Id == _integrationFixture.TestDataAccess.Tasks.First());
                Assert.NotNull(calendarEditedTask);
                Assert.False(calendarEditedTask.IsDisplay);
                Assert.True(calendarEditedTask.IsRanged);
                Assert.Equal(updatedTask.TaskName, calendarEditedTask.TaskName);

                var calendarCompletedTask = calendar.Seeds.First(x => x.Id == seedId).Tasks.SelectMany(x => x.Value).First(x => x.Id == completedTask);
                Assert.True(calendarCompletedTask.IsComplete);
            }
            finally
            {
                await _integrationFixture.TestDataAccess.RemoveTestData();
            }
        }
    }
}