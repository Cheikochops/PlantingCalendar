using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Options;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.UnitTests.Models;
using System.Data.SqlClient;

namespace PlantingCalendar.UnitTests
{
    public class TestDataAccess : AbstractDataAccess
    {
        public TestDataAccess(IOptions<DataAccessSettings> dataAccessSettings) 
            : base(dataAccessSettings)
        {
        }

        public async Task SetupTestData()
        {
            await ExecuteSql("Exec plantbase.TestData_Setup");
        }

        public async Task<List<NameIdModel>> GetTestCalendarIds()
        {
           return await ExecuteSql<NameIdModel>("Select Id, Name From plantbase.Calendar Where LEFT(Name, 4) = 'TEST'");
        }

        public async Task<List<NameIdModel>> GetTestSeedIds()
        {
            return await ExecuteSql<NameIdModel>("Select Id, Name From plantbase.Seed Where LEFT(Name, 4) = 'TEST'");
        }

        public async Task RemoveTestData()
        {
            await ExecuteSql("Exec plantbase.TestData_Remove");
        }
    }
}