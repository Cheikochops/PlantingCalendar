using PlantingCalendar.Models;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarDataAccess
    {
        Task<List<CalendarItemBasicModel>> GetBasicCalendars();

        Task<CalendarDetailsModel> GetCalendar(long id);
    }
}