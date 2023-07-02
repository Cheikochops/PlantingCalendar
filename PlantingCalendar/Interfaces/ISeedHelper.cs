using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ISeedHelper
    {
        Task<SeedDetailModel> GetFormatedSeedItem(long seedId, bool includeDeleted);

        Task<IEnumerable<SeedItemModel>> GetSeedList();

        Task SaveSeedInfo(UploadSeedDetailModel seed);

        Task DeleteSeed(long seedId);
    }
}