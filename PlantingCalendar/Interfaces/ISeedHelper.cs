using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ISeedHelper
    {
        Task<SeedDetailModel> GetFormatedSeedItem(long seedId);

        Task<IEnumerable<SeedItemModel>> GetSeedList();

        Task SaveSeedInfo(SeedDetailModel seed);

        Task DeleteSeed(long seedId);
    }
}