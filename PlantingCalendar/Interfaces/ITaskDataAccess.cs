using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ITaskDataAccess
    {
        Task DeleteTask(long taskId);

        Task CreateTask(string newTaskJson);

        Task ToggleCompleteTask(long taskId);

        Task UpdateTask(long taskId, string taskDetailsJson);
    }
}