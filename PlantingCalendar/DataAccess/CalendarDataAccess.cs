using Microsoft.Extensions.Options;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Threading.Tasks;

namespace PlantingCalendar.DataAccess
{
    public class CalendarDataAccess : AbstractDataAccess, ICalendarDataAccess
    {
        private ICalendarHelper _calendarHelper;

        public CalendarDataAccess(IOptions<DataAccessSettings> dataAccessSettings, ICalendarHelper calendarHelper) : base(dataAccessSettings)
        {
            _calendarHelper = calendarHelper;
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

        public async Task<CalendarDetailsModel> GetCalendar(long id)
        {
            try
            {
                var calendars = await ExecuteSql<SqlCalendarDetailsModel>($"Exec plantbase.Calendar_Read {id}");

                if (calendars == null)
                {
                    return new CalendarDetailsModel();
                }

                return _calendarHelper.FormatCalendar(calendars);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task GenerateNewCalendar(string calendarName, int calendarYear, string seedListJson)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.NewCalendar_Create '{calendarName}', {calendarYear}, '{seedListJson}'");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}