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

        public async Task DeleteTask(long taskId)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Task_Delete {taskId}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task ToggleCompleteTask(long taskId)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Task_ToggleComplete {taskId}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateTask(string taskJson)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Task_ToggleComplete {taskJson}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}