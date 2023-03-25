using PlantingCalendar.Models;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarDataAccess
    {
        Task<List<CalendarItemModel>> GetAllCalendars();
    }
}