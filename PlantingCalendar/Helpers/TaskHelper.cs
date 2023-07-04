using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

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
            var taskJson = JsonConvert.SerializeObject(task);

            await TaskDataAccess.CreateTask(taskJson);
        }

        public async Task SetTaskDate(UploadTaskDate taskDate)
        {
            await TaskDataAccess.SetTaskDate(taskDate);
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