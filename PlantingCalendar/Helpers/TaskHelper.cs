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

        public TaskHelper(ITaskDataAccess taskDataAccess)
        {
            TaskDataAccess = taskDataAccess;
        }

        public async Task CreateNewTask(UploadNewTask task)
        {
            //Validate
            if (string.IsNullOrEmpty(task.Name)
                || !task.Seeds.Any()
                || (task.IsRanged && (task.RangeEndDate == null || task.RangeStartDate == null)))
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
                                DisplayChar = task.DisplayChar?.First(),
                                DisplayColour = task.DisplayColour,
                                SetDate = task.SingleDate
                            });
                            break;
                        case RepeatableTypeEnum.Weekly:

                            for (var date = task.FromDate.Value; date <= task.ToDate; date.AddDays(7))
                            {
                                tasks.Add(new SqlSaveNewTaskModel
                                {
                                    Name = task.Name,
                                    Description = task.Description,
                                    IsRanged = task.IsRanged,
                                    SeedId = seed,
                                    IsDisplay = task.IsDisplay,
                                    DisplayChar = task.DisplayChar.First(),
                                    DisplayColour = task.DisplayColour,
                                    SetDate = date
                                });
                            }

                            break;
                        case RepeatableTypeEnum.Monthly:

                            var isEndOfMonth = task.FromDate.Value.Day == DateTime.DaysInMonth(task.FromDate.Value.Year, task.FromDate.Value.Month);

                            for (var month = task.FromDate.Value.Month; month < task.ToDate.Value.Month && month < 12; month++)
                            {
                                DateTime currentDate;
                                var endOfMonth = DateTime.DaysInMonth(task.FromDate.Value.Year, month);

                                if (isEndOfMonth)
                                {
                                    currentDate = new DateTime(task.FromDate.Value.Year, month, endOfMonth);
                                }
                                else
                                {
                                    var day = task.FromDate.Value.Day;
                                    if (day > endOfMonth)
                                    {
                                        day = endOfMonth;
                                    }

                                    currentDate = new DateTime(task.FromDate.Value.Year, month, day);
                                }

                                tasks.Add(new SqlSaveNewTaskModel
                                {
                                    Name = task.Name,
                                    Description = task.Description,
                                    IsRanged = task.IsRanged,
                                    SeedId = seed,
                                    IsDisplay = task.IsDisplay,
                                    DisplayChar = task.DisplayChar.First(),
                                    DisplayColour = task.DisplayColour,
                                    SetDate = currentDate
                                });
                            }

                            break;
                        case RepeatableTypeEnum.Fortnightly:

                            for (var date = task.FromDate.Value; date <= task.ToDate; date.AddDays(14))
                            {
                                tasks.Add(new SqlSaveNewTaskModel
                                {
                                    Name = task.Name,
                                    Description = task.Description,
                                    IsRanged = task.IsRanged,
                                    SeedId = seed,
                                    IsDisplay = task.IsDisplay,
                                    DisplayChar = task.DisplayChar.First(),
                                    DisplayColour = task.DisplayColour,
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
                        DisplayChar = task.DisplayChar.First(),
                        DisplayColour = task.DisplayColour,
                        RangeEndDate = task.RangeEndDate,
                        RangeStartDate = task.RangeStartDate,
                        SetDate = null
                    });
                }

            }

            var taskJson = JsonConvert.SerializeObject(tasks);

            await TaskDataAccess.CreateTask(task.CalendarId, taskJson);
        }

        public async Task EditTask(long taskId, UploadTaskDetails taskDetails)
        {
            taskDetails.DisplayColour = taskDetails.DisplayColour.Replace("#", "");
            taskDetails.DisplayChar = taskDetails.DisplayChar.First().ToString();

            if (taskDetails.IsRanged)
            {
                taskDetails.TaskSetDate = null;
            }
            else
            {
                taskDetails.TaskStartDate = null;
                taskDetails.TaskEndDate = null;
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