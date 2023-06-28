//using Microsoft.Extensions.Options;
//using PlantingCalendar.Interfaces;
//using PlantingCalendar.Models;
//using PlantingCalendar.Models.Sql;
//using System;
//using System.Threading.Tasks;

//namespace PlantingCalendar.DataAccess
//{
//    public class CalendarGenerationHelper : ICalendarGenerationHelper
//    {
//        private readonly ISeedDataAccess _seedDataAccess;

//        public CalendarGenerationHelper(ISeedDataAccess seedDataAccess)
//        {
//            _seedDataAccess = seedDataAccess;
//        }

//        public async Task GenerateNewCalendar(CalendarGenerationModel calendarGeneration)
//        {
//            var seeds = new List<SqlSeedDetailsModel>();

//            foreach (var seed in calendarGeneration.SeedIds)
//            {
//                seeds.AddRange(await _seedDataAccess.GetSeedDetails(seed));
//            }




//        }
//    }
//}