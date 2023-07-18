using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
using System;
using System.Threading.Tasks;

namespace PlantingCalendar.DataAccess
{
    public class CalendarHelper : ICalendarHelper
    {
        private ICalendarDataAccess _calendarDataAccess;
        public CalendarHelper(ICalendarDataAccess calendarDataAccess)
        {
            _calendarDataAccess = calendarDataAccess;
        }

        public async Task<CalendarDetailsModel> FormatCalendar(long id)
        {
            var calendarDetails = await _calendarDataAccess.GetCalendar(id);

            var first = calendarDetails.FirstOrDefault();

            if (first == null)
            {
                throw new Exception("Unexpected list length");
            }

            var months = GetMonths(first.Year);
            var seeds = MakeSeeds(calendarDetails, months);

            try
            {
                foreach (var seed in seeds)
                {
                    var dict = new Dictionary<int, List<CalendarTask>>();

                    foreach (var month in months)
                    {
                        dict.Add(month.Order, calendarDetails
                                .Where(y => y.SeedId == seed.Id)
                                .Where(y => (!y.IsRanged && y.SetDate.Value.Month == month.Order)
                                || (y.IsRanged &&
                                    month.Order <= y.RangeEndDate.Value.Month &&
                                    month.Order >= y.RangeStartDate.Value.Month))
                                .Select(y => new CalendarTask
                                {
                                    Id = y.TaskId.Value,
                                    IsDisplay = y.IsDisplay.Value,
                                    DisplayChar = y.IsDisplay.Value ? y.DisplayChar?.First() : null,
                                    DisplayColour = y.IsDisplay.Value ? '#' + y.DisplayColour : null,
                                    IsComplete = y.IsComplete.Value,
                                    IsRanged = y.IsRanged,
                                    TaskDate = y.SetDate,
                                    TaskEndDate = y.RangeEndDate,
                                    TaskStartDate = y.RangeStartDate,
                                    TaskDescription = y.TaskDescription,
                                    TaskName = y.TaskName
                                }).ToList());
                    }

                    seed.Tasks = dict;
                }


                return new CalendarDetailsModel
                {
                    CalendarId = first.CalendarId,
                    CalendarName = first.CalendarName,
                    Year = first.Year,
                    Seeds = seeds,
                    Months = months.ToList()
                };
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private IEnumerable<Month> GetMonths(int year)
        {
            return Enumerable.Range(1, 12).Select(i => new Month
            {
                MonthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                MonthCode = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i).Substring(0, 3).ToUpperInvariant(),
                DayCount = DateTime.DaysInMonth(year, i),
                Order = i,
                Days = GetDaysInMonth(i, year)
            });
        }

        private List<Seed> MakeSeeds(List<SqlCalendarDetailsModel> calendarDetails, IEnumerable<Month> months)
        {
            return calendarDetails.Where(x => x.SeedId != null).Select(x => new Seed
            {
                Id = x.SeedId.Value,
                PlantBreed = x.PlantBreed,
                PlantTypeName = x.PlantTypeName
            }).DistinctBy(x => x.Id).ToList();
        }

        private List<DayInMonth> GetDaysInMonth(int month, int year)
        {
            DateTime baseDate = new DateTime(year, month, 1);

            return Enumerable.Range(1, DateTime.DaysInMonth(baseDate.Year, baseDate.Month))
                .Select(dayNumber => new DateTime(baseDate.Year, baseDate.Month, dayNumber))
                .Select(dayName => new DayInMonth
                {
                    DayName = dayName.DayOfWeek.ToString(),
                    Day = dayName.Day 
                }).ToList();
        }

        public async Task<long> GenerateCalendar(GenerateCalendarModel model)
        {
            var calendarId = await _calendarDataAccess.GenerateNewCalendar(model.CalendarName, model.CalendarYear, JsonConvert.SerializeObject(model.Seeds));

            return calendarId;
        }

        public async Task UpdateCalendarSeeds(long calendarId, List<long> seedIds)
        {
            await _calendarDataAccess.UpdateCalendarSeeds(calendarId, seedIds);
        }
    }
}