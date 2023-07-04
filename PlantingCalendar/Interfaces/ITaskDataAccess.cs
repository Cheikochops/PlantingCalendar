using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ITaskDataAccess
    {
        Task SetTaskDate(UploadTaskDate taskDate);

        Task DeleteTask(long taskId);

        Task CreateTask(string taskJson);

        Task ToggleCompleteTask(long taskId);
    }
}