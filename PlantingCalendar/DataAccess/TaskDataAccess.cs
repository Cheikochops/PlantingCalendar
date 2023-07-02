using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Threading.Tasks;

namespace PlantingCalendar.DataAccess
{
    public class TaskDataAccess : AbstractDataAccess, ITaskDataAccess
    {
        public TaskDataAccess(IOptions<DataAccessSettings> dataAccessSettings) : base(dataAccessSettings)
        {
        }

        public async Task<List<RepeatableType>> GetRepeatableTypes()
        {
            try
            {
                var types = await ExecuteSql<RepeatableType>("Exec plantbase.RepeatableTypes_Read");

                return types;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SetTaskDate(UploadTaskDate task)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Task_SetDate {task.TaskId}, {task.Day}, {task.Month}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}