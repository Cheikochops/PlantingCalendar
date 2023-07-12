using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ITaskDataAccess
    {
        Task DeleteTask(long taskId);

        Task CreateTask(string taskJson);

        Task ToggleCompleteTask(long taskId);
    }
}