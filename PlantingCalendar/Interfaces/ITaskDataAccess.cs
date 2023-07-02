using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ITaskDataAccess
    {
        Task<List<RepeatableType>> GetRepeatableTypes();

        Task SetTaskDate(UploadTaskDate taskDate);
    }
}