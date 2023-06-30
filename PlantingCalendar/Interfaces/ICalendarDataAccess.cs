using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarDataAccess
    {
        Task<List<CalendarItemBasicModel>> GetBasicCalendars();

        Task<SqlCalendarDetailsModel> GetCalendar(long id);

        Task<long> GenerateNewCalendar(string calendarName, int calendarYear, string seedListJson);
    }
}