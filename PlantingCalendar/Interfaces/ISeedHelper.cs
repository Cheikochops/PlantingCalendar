using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ISeedHelper
    {
        Task<SeedDetailModel> GetFormatedSeedItem(long seedId);

        Task<IOrderedEnumerable<SeedItemModel>> GetFilteredSeedItems(string? filter, int? orderBy);

        Task SaveSeedInfo(SeedItemModel seed);

        Task DeleteSeed(long seedId);
    }
}