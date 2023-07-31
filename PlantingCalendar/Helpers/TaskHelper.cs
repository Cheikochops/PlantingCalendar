using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Threading.Tasks;

namespace PlantingCalendar.DataAccess
{
    public class TaskHelper : ITaskHelper
    {
        private ITaskDataAccess TaskDataAccess { get; set; }
        private ICalendarDataAccess CalendarDataAccess { get; set; }

        public TaskHelper(ITaskDataAccess taskDataAccess, ICalendarDataAccess calendarDataAccess)
        {
            TaskDataAccess = taskDataAccess;
            CalendarDataAccess = calendarDataAccess;
        }

        public async Task CreateNewTask(UploadNewTask task)
        {
            var calendarDetails = (await CalendarDataAccess.GetCalendar(task.CalendarId)).First();

            DateTime.TryParse(task.RangeEndDate, out var rangeEnd);
            DateTime.TryParse(task.RangeStartDate, out var rangeStart);
            DateTime.TryParse(task.SingleDate, out var singleDate);

            DateTime.TryParse(task.FromDate, out var fromDate);
            DateTime.TryParse(task.ToDate, out var toDate);

            //Validate
            if (string.IsNullOrEmpty(task.Name)
                || !task.Seeds.Any()
                || (task.IsRanged && (rangeEnd == default || task.RangeStartDate == null))
                || (task.IsRanged && (rangeEnd.Year != calendarDetails.Year || rangeStart.Year != calendarDetails.Year))
                || (!task.IsRanged && task.SingleDate == null && (task.FromDate == null || task.ToDate == null))
                || (!task.IsRanged && singleDate.Year != calendarDetails.Year && (fromDate.Year != calendarDetails.Year || toDate.Year != calendarDetails.Year)))
            {
                throw new ValidationException("Validation for task failed");
            }

            var tasks = new List<SqlSaveNewTaskModel>();

            foreach (var seed in task.Seeds) {

                if (!task.IsRanged)
                {
                    var repeatableTypeEnum = Enum.Parse(typeof(RepeatableTypeEnum), task.RepeatableType);

                    switch (repeatableTypeEnum)
                    {
                        case RepeatableTypeEnum.Never:
                            tasks.Add(new SqlSaveNewTaskModel
                            {
                                Name = task.Name,
                                Description = task.Description,
                                IsRanged = task.IsRanged,
                                SeedId = seed,
                                IsDisplay = task.IsDisplay,
                                DisplayChar = task.IsDisplay ? task.DisplayChar?.First() : null,
                                DisplayColour = task.IsDisplay ? task.DisplayColour?.Replace("#", "") : null,
                                SetDate = singleDate
                            });
                            break;
                        case RepeatableTypeEnum.Weekly:

                            for (var date = fromDate; date <= toDate; date = date.AddDays(7))
                            {
                                tasks.Add(new SqlSaveNewTaskModel
                                {
                                    Name = task.Name,
                                    Description = task.Description,
                                    IsRanged = task.IsRanged,
                                    SeedId = seed,
                                    IsDisplay = task.IsDisplay,
                                    DisplayChar = task.IsDisplay ? task.DisplayChar?.First() : null,
                                    DisplayColour = task.IsDisplay ? task.DisplayColour?.Replace("#", "") : null,
                                    SetDate = date
                                });
                            }

                            break;
                        case RepeatableTypeEnum.Monthly:

                            var isEndOfMonth = fromDate.Day == DateTime.DaysInMonth(fromDate.Year, fromDate.Month);

                            for (var month = fromDate.Month; month <= toDate.Month && month < 12; month++)
                            {
                                DateTime currentDate;
                                var endOfMonth = DateTime.DaysInMonth(fromDate.Year, month);

                                if (isEndOfMonth)
                                {
                                    currentDate = new DateTime(fromDate.Year, month, endOfMonth);
                                }
                                else
                                {
                                    var day = fromDate.Day;
                                    if (day > endOfMonth)
                                    {
                                        day = endOfMonth;
                                    }

                                    currentDate = new DateTime(fromDate.Year, month, day);
                                }

                                tasks.Add(new SqlSaveNewTaskModel
                                {
                                    Name = task.Name,
                                    Description = task.Description,
                                    IsRanged = task.IsRanged,
                                    SeedId = seed,
                                    IsDisplay = task.IsDisplay,
                                    DisplayChar = task.IsDisplay ? task.DisplayChar?.First() : null,
                                    DisplayColour = task.IsDisplay ? task.DisplayColour?.Replace("#", "") : null,
                                    SetDate = currentDate
                                });
                            }

                            break;
                        case RepeatableTypeEnum.Fortnightly:

                            for (var date = fromDate; date <= toDate; date = date.AddDays(14))
                            {
                                tasks.Add(new SqlSaveNewTaskModel
                                {
                                    Name = task.Name,
                                    Description = task.Description,
                                    IsRanged = task.IsRanged,
                                    SeedId = seed,
                                    IsDisplay = task.IsDisplay,
                                    DisplayChar = task.IsDisplay ?  task.DisplayChar.First() : null,
                                    DisplayColour = task.IsDisplay ? task.DisplayColour?.Replace("#", "") : null,
                                    SetDate = date
                                });
                            }

                            break;
                    }
                }
                else
                {
                    tasks.Add(new SqlSaveNewTaskModel
                    {
                        Name = task.Name,
                        Description = task.Description,
                        IsRanged = task.IsRanged,
                        SeedId = seed,
                        IsDisplay = task.IsDisplay,
                        DisplayChar = task.IsDisplay ? task.DisplayChar.First() : null,
                        DisplayColour = task.IsDisplay ? task.DisplayColour?.Replace("#", "") : null,
                        RangeEndDate = rangeEnd,
                        RangeStartDate = rangeStart,
                        SetDate = null
                    });
                }

            }

            var taskJson = JsonConvert.SerializeObject(tasks);

            await TaskDataAccess.CreateTask(task.CalendarId, taskJson);
        }

        public async Task EditTask(long calendarId, long taskId, UploadTaskDetails taskDetails)
        {
            if (string.IsNullOrEmpty(taskDetails.TaskName)) 
            {
                throw new ValidationException("Task name must not be empty");
            }

            var calendarDetails = (await CalendarDataAccess.GetCalendar(calendarId)).First();

            taskDetails.DisplayColour = taskDetails.IsDisplay ? taskDetails.DisplayColour?.Replace("#", "") : null;
            taskDetails.DisplayChar = taskDetails.IsDisplay ? taskDetails.DisplayChar?.First().ToString() : null;

            if (taskDetails.IsRanged)
            {
                taskDetails.TaskSetDate = null;

                if (
                    (taskDetails.TaskStartDate == null || DateTime.Parse(taskDetails.TaskStartDate).Year != calendarDetails.Year)
                    || (taskDetails.TaskEndDate == null || DateTime.Parse(taskDetails.TaskEndDate).Year != calendarDetails.Year)
                    )
                {
                    throw new ValidationException("Dates must be calendar's year");
                }
            }
            else
            {
                taskDetails.TaskStartDate = null;
                taskDetails.TaskEndDate = null;

                if (taskDetails.TaskSetDate == null || DateTime.Parse(taskDetails.TaskSetDate).Year != calendarDetails.Year)
                {
                    throw new ValidationException("Dates must be calendar's year");
                }
            }

            var taskJson = JsonConvert.SerializeObject(taskDetails);

            await TaskDataAccess.UpdateTask(taskId, taskJson);
        }

        public List<string> GetRepeatableTypes()
        {
            return Enum.GetValues(typeof(RepeatableTypeEnum))
                .Cast<RepeatableTypeEnum>()
                .Select(v => v.ToString())
                .ToList();
        }

        public async Task DeleteTask(long taskId)
        {
            await TaskDataAccess.DeleteTask(taskId);
        }

        public async Task ToggleCompleteTask(long taskId)
        {
            await TaskDataAccess.ToggleCompleteTask(taskId);
        }
    }
}