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

            var months = MakeMonths(first.Year, calendarDetails);

            months.ForEach(x =>
                    x.Seeds.ForEach(z => z.Tasks = calendarDetails
                            .Where(y => y.SeedId == z.Id)
                            .Where(y => (y.SetTaskDate != null && y.SetTaskDate.Value.Month == x.Order)
                            || (
                                y.RangeTaskStartDate != null && y.RangeTaskEndDate != null &&
                                x.Order <= y.RangeTaskEndDate.Value.Month &&
                                x.Order >= y.RangeTaskStartDate.Value.Month))
                            .Select(y => new CalendarTask
                            {
                                Id = y.TaskId.Value,
                                TaskTypeId = y.TaskTypeId.Value,
                                DisplayChar = y.TaskDisplayChar.Value,
                                DisplayColour = y.TaskDisplayColour,
                                IsComplete = y.IsComplete.Value,
                                TaskDate = y.SetTaskDate,
                                TaskEndDate = y.RangeTaskEndDate,
                                TaskStartDate = y.RangeTaskStartDate,
                                TaskDescription = y.TaskDescription,
                                TaskName = y.TaskName
                            }).ToList()));

            return new CalendarDetailsModel
            {
                CalendarId = first.CalendarId,
                CalendarName = first.CalendarName,
                Year = first.Year,
                Months = months
            };

        }

        private List<Month> MakeMonths(int year, List<SqlCalendarDetailsModel> calendarDetails)
        {
            return Enumerable.Range(1, 12).Select(i => new Month
            {
                MonthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                MonthCode = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i).Substring(0, 3).ToUpperInvariant(),
                DayCount = DateTime.DaysInMonth(year, i),
                Order = i,
                Seeds = calendarDetails.Where(x => x.SeedId != null).Select(x => new Seed
                {
                    Id = x.SeedId.Value,
                    PlantBreed = x.PlantBreed,
                    PlantTypeName = x.PlantTypeName
                }).DistinctBy(x => x.Id).ToList()
            }).ToList();
        }

    }
}