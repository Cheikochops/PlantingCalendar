using PlantingCalendar.Models;

namespace PlantingCalendar.Interfaces
{
    public interface ITaskHelper
    {
        Task CreateNewTask(UploadNewTask task);

        Task SetTaskDate(UploadTaskDate taskDate);

        List<string> GetRepeatableTypes();

        Task DeleteTask(long taskId);

        Task ToggleCompleteTask(long taskId);
    }
}