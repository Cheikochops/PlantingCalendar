using Microsoft.Extensions.Options;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.UnitTests
{
    public class TestDataAccess : AbstractDataAccess
    {
        public List<long> SeedIds { get; set; }

        public List<long> SeedActionIds { get; set; }

        public long CalendarId { get; set; }

        public long CalendarSeedId { get; set; }

        public List<long> Tasks { get; set; }

        public TestDataAccess(IOptions<DataAccessSettings> dataAccessSettings)
            : base(dataAccessSettings)
        {
        }

        public async Task SetupTestData()
        {
            await SetupTestSeed();
            await SetupTestCalendar();
        }

        public async Task RemoveTestData()
        {
            await RemoveCalendar(CalendarId);

            foreach (var seed in SeedIds)
            {
                await RemoveSeed(seed);
            }
        }

        public async Task RemoveCalendar(long calendarId)
        {
            await ExecuteSql($"Delete from plantbase.Calendar where id = {calendarId}");
        }

        public async Task RemoveSeed(long seedId)
        {
            await ExecuteSql($"Delete from plantbase.Seed where id = {seedId}");
        }

        private async Task SetupTestSeed()
        {
            var seedId = await ExecuteSql<SqlIdModel>("Insert Into plantbase.Seed (PlantType, Breed, Description, WaterRequirement, SunRequirement, ExpiryDate ) Output INSERTED.Id VALUES ( 'IntegrationTest: Tomato', 'Gardener''s Delight', 'A high production small fruit, vine variety', '1 - 2 inches per week', '8 - 16 hours', '2023-05-10' ), ( 'TEST: Courgette', 'Ambassador F1 ', 'An early variety with the production of dark green fruits', '1 + inch per week', 'Full Sun, 6 - 8 hours', '2023-07-10' )");
            SeedIds = seedId.Select(x => x.Id.Value).ToList();

            var tomatoSeedId = SeedIds.First();
            var seedActionId = await ExecuteSql<SqlIdModel>($"Insert Into plantbase.SeedAction ( Name, Description, FK_SeedId, StartDate, EndDate, Enum_ActionTypeId, IsDisplay, DisplayColour, DisplayChar ) OUTPUT inserted.Id VALUES ( 'Sow', 'Sow the plant', {tomatoSeedId}, '0102', '3004', 1, 1, '00000F', 'S' ), ( 'Harvest', 'Harvest the plant', {tomatoSeedId}, '1007', '2010', 2, 1, '111111', 'H' )");
            SeedActionIds = seedActionId.Select(x => x.Id.Value).ToList();
        }

        private async Task SetupTestCalendar()
        {
            var calendarId = await ExecuteSql<SqlIdModel>("Insert Into plantbase.Calendar ( Name, Year ) Output inserted.Id VALUES ( 'IntegrationTest: Greenhouse Calendar', 2023 )");
            CalendarId = calendarId.First().Id.Value;

            var calendarSeedId = await ExecuteSql<SqlIdModel>($"Insert Into plantbase.CalendarSeed  ( FK_CalendarId, FK_SeedId ) Output inserted.Id VALUES ( {CalendarId}, {SeedIds.First()} );");
            CalendarSeedId = calendarSeedId.First().Id.Value;

            var taskIds = await ExecuteSql<SqlIdModel>($"Insert Into plantbase.Task ( FK_CalendarSeedId, Name, Description, IsRanged, RangeStartDate, RangeEndDate, SetDate, IsDisplay, DisplayChar, DisplayColour, IsComplete ) Output inserted.Id VALUES ( {CalendarSeedId}, 'Sow', 'Sow the plant', 1, '2023-02-01', '2023-04-30', null, 1, 'S', '00FF00', 0 ), ( {CalendarSeedId}, 'Harvest', 'Harvest the plant', 1, '2023-07-01', '2023-10-30', null, 1, 'H', '0000FF', 0 ), ( {CalendarSeedId}, 'Trim', 'Trim the bottom leaves off of plants', 0, null, null, '2023-05-01', 0, null, null, 0 )");
            Tasks = taskIds.Select(x => x.Id.Value).ToList();
        }

    }
}