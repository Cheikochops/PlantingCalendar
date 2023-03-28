using Microsoft.Extensions.Options;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
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
    }
}