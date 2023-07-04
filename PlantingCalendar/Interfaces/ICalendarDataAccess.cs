using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarDataAccess
    {
        Task<List<CalendarItemBasicModel>> GetBasicCalendars();

        Task<List<SqlCalendarDetailsModel>> GetCalendar(long id);

        Task<long> GenerateNewCalendar(string calendarName, int calendarYear, string seedListJson);

        Task RemoveSeedFromCalendar(long calendarId, long seedId);

        Task AddSeedToCalendar(long calendarId, long seedId);
    }
}