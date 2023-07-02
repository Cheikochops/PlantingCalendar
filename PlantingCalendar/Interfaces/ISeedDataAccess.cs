using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ISeedDataAccess
    {
        Task<List<SeedItemModel>> GetAllSeeds();

        Task<List<SqlSeedDetailsModel>> GetSeedDetails(long seedId, bool includeDeleted);

        Task DeleteSeed(long seedId);

        Task SaveSeed(string seedDetails);
    }
}