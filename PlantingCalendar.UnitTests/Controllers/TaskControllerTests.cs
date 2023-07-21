using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

namespace PlantingCalendar.UnitTests
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskHelper> _taskHelper;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _taskHelper = new Mock<ITaskHelper>();
            _taskController = new TaskController(_taskHelper.Object);
        }

        [Fact]
        public async Task GetTaskTypes()
        {
            var types = new List<string>
            {

            };

            _taskHelper.Setup(x => x.GetRepeatableTypes())
                .Returns(types)
                .Verifiable();

            var result = await _taskController.GetTaskTypes();

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal(types, ((OkObjectResult)result).Value);

            _taskHelper.Verify();
        }

        [Fact]
        public async Task CreateNewTask()
        {
            var task = new UploadNewTask
            {
                CalendarId = long.MaxValue
            };

            _taskHelper.Setup(x => x.CreateNewTask(task))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _taskController.CreateNewTask(task);

            Assert.Equal(typeof(OkResult), result.GetType());

            _taskHelper.Verify();
        }

        [Fact]
        public async Task UpdateTask()
        {
            var taskId = long.MaxValue;

            var task = new UploadTaskDetails
            {
                TaskName = "This is a task"
            };

            _taskHelper.Setup(x => x.EditTask(taskId, task))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _taskController.UpdateTask(taskId, task);

            Assert.Equal(typeof(OkResult), result.GetType());

            _taskHelper.Verify();
        }

        [Fact]
        public async Task DeleteTask()
        {
            var taskId = long.MaxValue;

            _taskHelper.Setup(x => x.DeleteTask(taskId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _taskController.DeleteTask(taskId);

            Assert.Equal(typeof(OkResult), result.GetType());

            _taskHelper.Verify();
        }

        [Fact]
        public async Task ToggleCompleteTask()
        {
            var taskId = long.MaxValue;

            _taskHelper.Setup(x => x.ToggleCompleteTask(taskId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _taskController.ToggleCompleteTask(taskId);

            Assert.Equal(typeof(OkResult), result.GetType());

            _taskHelper.Verify();
        }
    }
}