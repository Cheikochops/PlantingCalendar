using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Globalization;
using System.Threading.Tasks;

namespace PlantingCalendar.DataAccess
{
    public class CalendarDataAccess : AbstractDataAccess, ICalendarDataAccess
    {

        public CalendarDataAccess(IOptions<DataAccessSettings> dataAccessSettings) : base(dataAccessSettings)
        {
        }

        public async Task<List<CalendarItemBasicModel>> GetBasicCalendars()
        {
            try
            {
                var calendars = await ExecuteSql<CalendarItemBasicModel>("Exec plantbase.Calendar_Basic_Read");

                if (calendars == null)
                {
                    return new List<CalendarItemBasicModel>();
                }

                return calendars;
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Calendar_Basic_Read", ex);
            }
        }

        public async Task<List<SqlCalendarDetailsModel>> GetCalendar(long id)
        {
            try
            {
                var calendars = await ExecuteSql<SqlCalendarDetailsModel>($"Exec plantbase.Calendar_Read @id", new Dictionary<string, object>
                {
                    { "@id", id }
                });

                if (calendars == null || !calendars.Any())
                {
                    return new List<SqlCalendarDetailsModel>();
                }

                return calendars;
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Calendar_Read", ex);
            }
        }

        public async Task<long> GenerateNewCalendar(string calendarName, int calendarYear, string seedListJson)
        {
            try
            {
                var calendarId = await ExecuteSql<SqlIdModel>($"Exec plantbase.NewCalendar_Create @calendarName, @calendarYear, @seedListJson", new Dictionary<string, object>
                {
                    { "@calendarName", calendarName },
                    { "@calendarYear", calendarYear },
                    { "@seedListJson", seedListJson }
                });

                if (calendarId == null 
                    || !calendarId.Any() 
                    || calendarId.First().Id == null)
                {
                    throw new Exception("Unable to get returned CalendarId");
                }

                return calendarId.First().Id.Value;
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run NewCalendar_Create", ex);
            }
        }

        public async Task UpdateCalendarSeeds(long calendarId, List<long> seedIds)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Calendar_SeedUpdate @calendarId, @seedIds", new Dictionary<string, object>
                {
                    { "@calendarId", calendarId },
                    { "@seedIds", JsonConvert.SerializeObject(seedIds)}
                });
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Calendar_SeedUpdate", ex);
            }
        }
    }
}