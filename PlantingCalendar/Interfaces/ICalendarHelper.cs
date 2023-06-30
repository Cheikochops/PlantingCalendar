using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ICalendarHelper
    {
        CalendarDetailsModel FormatCalendar(List<SqlCalendarDetailsModel> calendarDetails);

        Task<long> GenerateCalendar(GenerateCalendarModel model);
    }
}