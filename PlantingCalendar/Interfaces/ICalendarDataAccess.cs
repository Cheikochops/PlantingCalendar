using PlantingCalendar.Models;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarDataAccess
    {
        Task<List<CalendarModel>> GetAllCalendars();
    }
}