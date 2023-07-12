using PlantingCalendar.Models;

namespace PlantingCalendar.Interfaces
{
    public interface ITaskHelper
    {
        Task CreateNewTask(UploadNewTask task);

        Task EditTask(UploadTaskDetails taskDetails);

        List<string> GetRepeatableTypes();

        Task DeleteTask(long taskId);

        Task ToggleCompleteTask(long taskId);
    }
}