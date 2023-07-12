using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System.Globalization;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarDataAccess
    {
        Task<List<CalendarItemBasicModel>> GetBasicCalendars();

        Task<List<SqlCalendarDetailsModel>> GetCalendar(long id);

        Task<long> GenerateNewCalendar(string calendarName, int calendarYear, string seedListJson);

        Task UpdateCalendarSeeds(long calendarId, List<long> seedIds);
    }
}