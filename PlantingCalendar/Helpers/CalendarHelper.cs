using Microsoft.Extensions.Options;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System;
using System.Threading.Tasks;

namespace PlantingCalendar.DataAccess
{
    public class CalendarHelper : ICalendarHelper
    {
        public CalendarHelper()
        {
        }

        public CalendarDetailsModel FormatCalendar(List<SqlCalendarDetailsModel> calendarDetails)
        {
            var first = calendarDetails.FirstOrDefault();

            if (first == null)
            {
                throw new Exception("Unexpected list length");
            }

            var months = MakeMonths(first.Year);

            months.ForEach(x =>
                    x.Tasks = calendarDetails.Where(y => (y.TaskDate != null && y.TaskDate.Value.Month == x.Order)
                    || (
                        y.TaskStartDate != null && y.TaskEndDate != null &&
                        x.Order <= y.TaskEndDate.Value.Month &&
                        x.Order >= y.TaskStartDate.Value.Month))
                    .Select(y => new CalendarTask
                    {
                        Id = y.TaskId.Value,
                        TaskTypeId = y.TaskTypeId.Value,
                        DisplayChar = y.DisplayChar.Value,
                        DisplayColour = y.DisplayColour,
                        IsComplete = y.IsComplete.Value,
                        TaskDate = y.TaskDate,
                        TaskEndDate = y.TaskEndDate,
                        TaskStartDate = y.TaskStartDate,
                        TaskDescription = y.TaskTypeDescription,
                        TaskName = y.TaskTypeName
                    }).ToList());

            return new CalendarDetailsModel
            {
                CalendarId = first.CalendarId,
                CalendarName = first.CalendarName,
                Year = first.Year,
                Months = months
            };

        }

        private List<Month> MakeMonths(int year)
        {
            return Enumerable.Range(1, 12).Select(i => new Month
            {
                MonthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                MonthCode = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i).Substring(0, 3).ToUpperInvariant(),
                DayCount = DateTime.DaysInMonth(year, i),
                Order = i
            }).ToList();
        }
    }
}