using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ISeedDataAccess
    {
        Task<List<SeedItemModel>> GetAllSeeds();

        Task<List<SqlSeedDetailsModel>> GetSeedDetails(long seedId);

        Task DeleteSeed(long seedId);
    }
}