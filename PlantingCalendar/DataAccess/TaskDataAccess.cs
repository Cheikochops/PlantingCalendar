using Microsoft.Extensions.Options;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

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
                throw new SqlFailureException("Failed to run Task_Delete", ex);
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
                throw new SqlFailureException("Failed to run Task_ToggleComplete", ex);
            }
        }

        public async Task CreateTask(long calendarId, string newTaskJson)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Task_Create @calendarId, @newTaskJson", new Dictionary<string, object>
                {
                    { "@calendarId", calendarId },
                    { "@newTaskJson", newTaskJson }
                });
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Task_Create", ex);
            }
        }

        public async Task UpdateTask(long taskId, string taskDetailsJson)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Task_Update @taskId, @taskDetailsJson", new Dictionary<string, object>
                {
                    { "@taskId", taskId },
                    { "@taskDetailsJson", taskDetailsJson }
                });
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Task_Update", ex);
            }
        }
    }
}