using Moq;
using Newtonsoft.Json;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Runtime.Intrinsics.X86;

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
                SingleDate = new DateTime(2023, 09, 09),
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
                    Assert.Null(task.SetDate);

                    Assert.True(task.IsRanged);
                    Assert.True(task.IsDisplay);
                    Assert.Equal(newTask.Name, task.Name);
                    Assert.Equal(1, task.SeedId);
                }).Verifiable();

            await _taskHelper.CreateNewTask(newTask);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task CreateNewTask_SetSingle()
        {
            var newTask = new UploadNewTask()
            {
                CalendarId = 1,
                IsDisplay = true,
                DisplayChar = "A",
                DisplayColour = "#123456",
                Name = "Test Task",
                IsRanged = false,
                RangeStartDate = new DateTime(2023, 07, 20),
                RangeEndDate = new DateTime(2023, 09, 10),
                SingleDate = new DateTime(2023, 01, 01),
                RepeatableType = "0",
                Seeds = new List<long>
                {
                    1, 2
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

                    Assert.Equal(2, tasks.Count);

                    var seed1 = tasks.First(x => x.SeedId == 1);
                    var seed2 = tasks.First(x => x.SeedId == 2);

                    Assert.NotNull(seed1);
                    Assert.NotNull(seed2);

                    seed1.SeedId = long.MaxValue;
                    seed2.SeedId = long.MaxValue;

                    Assert.Equal(JsonConvert.SerializeObject(seed1), 
                        JsonConvert.SerializeObject(seed2));

                    Assert.Null(seed1.RangeStartDate);
                    Assert.Null(seed1.RangeEndDate);
                    Assert.Equal(newTask.SingleDate, seed1.SetDate);

                    Assert.False(seed1.IsRanged);
                    Assert.True(seed1.IsDisplay);
                    Assert.Equal(newTask.Name, seed1.Name);
                    Assert.Equal('A', seed1.DisplayChar);
                    Assert.Equal("123456", seed1.DisplayColour);

                }).Verifiable();

            await _taskHelper.CreateNewTask(newTask);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task CreateNewTask_SetWeekly()
        {
            var newTask = new UploadNewTask()
            {
                CalendarId = 1,
                IsDisplay = true,
                DisplayChar = "A",
                DisplayColour = "#123456",
                Name = "Test Task",
                IsRanged = false,
                RangeStartDate = new DateTime(2023, 07, 20),
                RangeEndDate = new DateTime(2023, 09, 10),
                SingleDate = new DateTime(2023, 01, 01),
                FromDate = new DateTime(2023, 01, 05),
                ToDate = new DateTime(2023, 02, 10),
                RepeatableType = "1",
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

            var expectedDates = new DateTime[]
            {
                new DateTime(2023, 01, 05),
                new DateTime(2023, 01, 12),
                new DateTime(2023, 01, 19),
                new DateTime(2023, 01, 26),
                new DateTime(2023, 02, 02),
                new DateTime(2023, 02, 09)
            };

            _taskDataAccessMock.Setup(x => x.CreateTask(newTask.CalendarId, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<long, string>((id, json) =>
                {
                    var tasks = JsonConvert.DeserializeObject<List<SqlSaveNewTaskModel>>(json);

                    Assert.Equal(6, tasks.Count);

                    foreach (var task in tasks)
                    {
                        Assert.NotNull(task.SetDate);
                        Assert.Contains(task.SetDate.Value, expectedDates);
                        Assert.False(task.IsRanged);
                        Assert.True(task.IsDisplay);
                        Assert.Equal(newTask.Name, task.Name);
                        Assert.Equal('A', task.DisplayChar);
                        Assert.Equal("123456", task.DisplayColour);

                        Assert.Null(task.RangeStartDate);
                        Assert.Null(task.RangeEndDate);

                    }
                }).Verifiable();

            await _taskHelper.CreateNewTask(newTask);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task CreateNewTask_SetFortnightly()
        {
            var newTask = new UploadNewTask()
            {
                CalendarId = 1,
                IsDisplay = true,
                DisplayChar = "A",
                DisplayColour = "#123456",
                Name = "Test Task",
                IsRanged = false,
                RangeStartDate = new DateTime(2023, 07, 20),
                RangeEndDate = new DateTime(2023, 09, 10),
                SingleDate = new DateTime(2023, 01, 01),
                FromDate = new DateTime(2023, 01, 05),
                ToDate = new DateTime(2023, 02, 10),
                RepeatableType = "2",
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

            var expectedDates = new DateTime[]
            {
                new DateTime(2023, 01, 05),
                new DateTime(2023, 01, 19),
                new DateTime(2023, 02, 02)
            };

            _taskDataAccessMock.Setup(x => x.CreateTask(newTask.CalendarId, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<long, string>((id, json) =>
                {
                    var tasks = JsonConvert.DeserializeObject<List<SqlSaveNewTaskModel>>(json);

                    Assert.Equal(3, tasks.Count);

                    foreach (var task in tasks)
                    {
                        Assert.NotNull(task.SetDate);
                        Assert.Contains(task.SetDate.Value, expectedDates);
                        Assert.False(task.IsRanged);
                        Assert.True(task.IsDisplay);
                        Assert.Equal(newTask.Name, task.Name);
                        Assert.Equal('A', task.DisplayChar);
                        Assert.Equal("123456", task.DisplayColour);

                        Assert.Null(task.RangeStartDate);
                        Assert.Null(task.RangeEndDate);

                    }
                }).Verifiable();

            await _taskHelper.CreateNewTask(newTask);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task CreateNewTask_SetMonthly()
        {
            var newTask = new UploadNewTask()
            {
                CalendarId = 1,
                IsDisplay = true,
                DisplayChar = "A",
                DisplayColour = "#123456",
                Name = "Test Task",
                IsRanged = false,
                RangeStartDate = new DateTime(2023, 07, 20),
                RangeEndDate = new DateTime(2023, 09, 10),
                SingleDate = new DateTime(2023, 01, 01),
                FromDate = new DateTime(2023, 01, 05),
                ToDate = new DateTime(2023, 04, 10),
                RepeatableType = "3",
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

            var expectedDates = new DateTime[]
            {
                new DateTime(2023, 01, 05),
                new DateTime(2023, 02, 05),
                new DateTime(2023, 03, 05),
                new DateTime(2023, 04, 05)
            };

            _taskDataAccessMock.Setup(x => x.CreateTask(newTask.CalendarId, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<long, string>((id, json) =>
                {
                    var tasks = JsonConvert.DeserializeObject<List<SqlSaveNewTaskModel>>(json);

                    Assert.Equal(4, tasks.Count);

                    foreach (var task in tasks)
                    {
                        Assert.NotNull(task.SetDate);
                        Assert.Contains(task.SetDate.Value, expectedDates);
                        Assert.False(task.IsRanged);
                        Assert.True(task.IsDisplay);
                        Assert.Equal(newTask.Name, task.Name);
                        Assert.Equal('A', task.DisplayChar);
                        Assert.Equal("123456", task.DisplayColour);

                        Assert.Null(task.RangeStartDate);
                        Assert.Null(task.RangeEndDate);

                    }
                }).Verifiable();

            await _taskHelper.CreateNewTask(newTask);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task CreateNewTask_SetMonthly_EndOfMonth()
        {
            var newTask = new UploadNewTask()
            {
                CalendarId = 1,
                IsDisplay = true,
                DisplayChar = "A",
                DisplayColour = "#123456",
                Name = "Test Task",
                IsRanged = false,
                RangeStartDate = new DateTime(2023, 07, 20),
                RangeEndDate = new DateTime(2023, 09, 10),
                SingleDate = new DateTime(2023, 01, 01),
                FromDate = new DateTime(2023, 01, 31),
                ToDate = new DateTime(2023, 05, 31),
                RepeatableType = "3",
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

            var expectedDates = new DateTime[]
            {
                new DateTime(2023, 01, 31),
                new DateTime(2023, 02, 28),
                new DateTime(2023, 03, 31),
                new DateTime(2023, 04, 30),
                new DateTime(2023, 05, 31)
            };

            _taskDataAccessMock.Setup(x => x.CreateTask(newTask.CalendarId, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<long, string>((id, json) =>
                {
                    var tasks = JsonConvert.DeserializeObject<List<SqlSaveNewTaskModel>>(json);

                    Assert.Equal(5, tasks.Count);

                    foreach (var task in tasks)
                    {
                        Assert.NotNull(task.SetDate);
                        Assert.Contains(task.SetDate.Value, expectedDates);
                        Assert.False(task.IsRanged);
                        Assert.True(task.IsDisplay);
                        Assert.Equal(newTask.Name, task.Name);
                        Assert.Equal('A', task.DisplayChar);
                        Assert.Equal("123456", task.DisplayColour);

                        Assert.Null(task.RangeStartDate);
                        Assert.Null(task.RangeEndDate);

                    }
                }).Verifiable();

            await _taskHelper.CreateNewTask(newTask);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task EditTask_Ranged()
        {
            var taskId = long.MaxValue;
            var taskDetails = new UploadTaskDetails
            {
                IsRanged = true,
                IsDisplay = true,
                DisplayChar = "A",
                DisplayColour = "#123876",
                TaskName = "Test Name",
                TaskDescription = "This is the task",
                TaskStartDate = new DateTime(2023, 03, 01),
                TaskEndDate = new DateTime(2023, 07, 10),
                TaskSetDate = new DateTime(2023, 01, 01)
            };

            _taskDataAccessMock.Setup(x => x.UpdateTask(taskId, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<long, string>((l, json) =>
                {
                    var editTask = JsonConvert.DeserializeObject<UploadTaskDetails>(json);

                    Assert.Equal(taskDetails.TaskName, editTask.TaskName);
                    Assert.Equal(taskDetails.TaskDescription, editTask.TaskDescription);
                    Assert.Equal(taskDetails.IsDisplay, editTask.IsDisplay);
                    Assert.Equal("A", editTask.DisplayChar);
                    Assert.Equal("123876", editTask.DisplayColour);
                    Assert.True(editTask.IsRanged);
                    Assert.Null(editTask.TaskSetDate);
                    Assert.Equal(taskDetails.TaskEndDate, editTask.TaskEndDate);
                    Assert.Equal(taskDetails.TaskStartDate, editTask.TaskStartDate);
                }).Verifiable();

            await _taskHelper.EditTask(taskId, taskDetails);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task EditTask_Single()
        {
            var taskId = long.MaxValue;
            var taskDetails = new UploadTaskDetails
            {
                IsRanged = false,
                IsDisplay = false,
                DisplayChar = "A",
                DisplayColour = "#123876",
                TaskName = "Test Name",
                TaskDescription = "This is the task",
                TaskStartDate = new DateTime(2023, 03, 01),
                TaskEndDate = new DateTime(2023, 07, 10),
                TaskSetDate = new DateTime(2023, 01, 01)
            };

            _taskDataAccessMock.Setup(x => x.UpdateTask(taskId, It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<long, string>((l, json) =>
                {
                    var editTask = JsonConvert.DeserializeObject<UploadTaskDetails>(json);

                    Assert.Equal(taskDetails.TaskName, editTask.TaskName);
                    Assert.Equal(taskDetails.TaskDescription, editTask.TaskDescription);
                    Assert.Equal(taskDetails.IsDisplay, editTask.IsDisplay);
                    Assert.Null(editTask.DisplayChar);
                    Assert.Null(editTask.DisplayColour);
                    Assert.False(editTask.IsRanged);
                    Assert.Equal(taskDetails.TaskSetDate, editTask.TaskSetDate);
                    Assert.Null(editTask.TaskEndDate);
                    Assert.Null(editTask.TaskStartDate);
                }).Verifiable();

            await _taskHelper.EditTask(taskId, taskDetails);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task GetRepeatableTypes ()
        {
            var types = _taskHelper.GetRepeatableTypes();

            Assert.Equal(4, types.Count);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task DeleteTask()
        {
            var taskId = long.MaxValue;

            _taskDataAccessMock.Setup(x => x.DeleteTask(taskId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _taskHelper.DeleteTask(taskId);

            _mockRepository.Verify();
        }

        [Fact]
        public async Task ToggleCompleteTask()
        {
            var taskId = long.MaxValue;

            _taskDataAccessMock.Setup(x => x.ToggleCompleteTask(taskId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _taskHelper.ToggleCompleteTask(taskId);

            _mockRepository.Verify();
        }
    }
}