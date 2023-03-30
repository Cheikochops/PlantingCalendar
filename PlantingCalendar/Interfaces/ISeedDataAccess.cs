using PlantingCalendar.Models;

namespace PlantingCalendar.Interfaces
{
    public interface ISeedDataAccess
    {
        Task<List<SeedItemModel>> GetAllSeeds();

        Task<SeedDetailModel> GetSeedDetails(long seedId);
    }
}