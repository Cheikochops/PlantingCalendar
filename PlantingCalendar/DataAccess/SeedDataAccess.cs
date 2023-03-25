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

        public async Task<List<CalendarItemModel>> GetAllCalendars()
        {
            try
            {
                var calendars = await ExecuteSql<CalendarItemModel>("Exec plantbase.Calendar_Read");

                if (calendars == null)
                {
                    return new List<CalendarItemModel>();
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