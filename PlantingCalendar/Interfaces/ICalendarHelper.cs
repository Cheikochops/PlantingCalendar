using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarHelper
    {
        Task<CalendarDetailsModel> FormatCalendar(long id);

        Task<long> GenerateCalendar(GenerateCalendarModel model);

        Task UpdateCalendarSeeds(long calendarId, List<long> seedIds);
    }
}