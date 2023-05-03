using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;

namespace PlantingCalendar.Interfaces
{
    public interface ISeedHelper
    {
        SeedDetailModel FormatSeedItem(List<SqlSeedDetailsModel> seedDetails);

        IOrderedEnumerable<SeedItemModel> FilterSeedItems(List<SeedItemModel> seeds, string filter);
    }
}