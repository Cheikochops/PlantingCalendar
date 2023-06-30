using Microsoft.Extensions.Options;
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
                throw;
            }
        }

        public async Task<SqlCalendarDetailsModel> GetCalendar(long id)
        {
            try
            {
                var calendars = await ExecuteSql<SqlCalendarDetailsModel>($"Exec plantbase.Calendar_Read {id}");

                if (calendars == null || !calendars.Any())
                {
                    return new SqlCalendarDetailsModel();
                }

                return calendars.First();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> GenerateNewCalendar(string calendarName, int calendarYear, string seedListJson)
        {
            try
            {
                var calendarId = await ExecuteSql<long>($"Exec plantbase.NewCalendar_Create '{calendarName}', {calendarYear}, '{seedListJson}'");

                if (calendarId == null || !calendarId.Any())
                {
                    throw new Exception("Unable to get CalendarId");
                }

                return calendarId.First();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}