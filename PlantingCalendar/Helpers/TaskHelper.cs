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
            var taskType = new List<TaskType>();

            if (!task.IsRanged)
            {
                var repeatableTypeEnum = Enum.Parse(typeof(RepeatableTypeEnum), task.RepeatableType);

                switch (repeatableTypeEnum)
                {
                    case RepeatableTypeEnum.Never:
                        break;
                    case RepeatableTypeEnum.Weekly:
                        break;
                    case RepeatableTypeEnum.Monthly:
                        break;
                    case RepeatableTypeEnum.Fortnightly:
                        break;
                }
            }

            var taskJson = JsonConvert.SerializeObject(task);

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