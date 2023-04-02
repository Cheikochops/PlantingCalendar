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
            var seeds = MakeSeeds(calendarDetails, months);

            seeds.ForEach(x => x.Months.ForEach(z => z.Tasks = calendarDetails
                            .Where(y => y.SeedId == x.Id)
                            .Where(y => (y.SetTaskDate != null && y.SetTaskDate.Value.Month == z.Order)
                            || (
                                y.RangeTaskStartDate != null && y.RangeTaskEndDate != null &&
                                z.Order <= y.RangeTaskEndDate.Value.Month &&
                                z.Order >= y.RangeTaskStartDate.Value.Month))
                            .Select(y => new CalendarTask
                            {
                                Id = y.TaskId.Value,
                                TaskTypeId = y.TaskTypeId.Value,
                                DisplayChar = y.TaskDisplayChar.First(),
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
                Seeds = seeds
            };

        }

        private IEnumerable<Month> MakeMonths(int year)
        {
            return Enumerable.Range(1, 12).Select(i => new Month
            {
                MonthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                MonthCode = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i).Substring(0, 3).ToUpperInvariant(),
                DayCount = DateTime.DaysInMonth(year, i),
                Order = i
            });
        }

        private List<Seed> MakeSeeds(List<SqlCalendarDetailsModel> calendarDetails, IEnumerable<Month> months)
        {
            return calendarDetails.Where(x => x.SeedId != null).Select(x => new Seed
            {
                Id = x.SeedId.Value,
                PlantBreed = x.PlantBreed,
                PlantTypeName = x.PlantTypeName,
                Months = months.ToList()
            }).DistinctBy(x => x.Id).ToList();
        }

    }
}