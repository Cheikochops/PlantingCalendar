using Microsoft.Extensions.Options;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using System.Threading.Tasks;

namespace PlantingCalendar.DataAccess
{
    public class SeedDataAccess : AbstractDataAccess, ISeedDataAccess
    {
        public SeedDataAccess(IOptions<DataAccessSettings> dataAccessSettings) : base(dataAccessSettings)
        {
        }

        public async Task<List<SeedItemModel>> GetAllSeeds()
        {
            try
            {
                var seeds = await ExecuteSql<SeedItemModel>("Exec plantbase.Seeds_Read");

                if (seeds == null)
                {
                    return new List<SeedItemModel>();
                }

                return seeds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<SeedDetailModel> GetSeedDetails(long seedId)
        {
            try
            {
                //Create this proc
                var seeds = await ExecuteSql<SeedDetailModel>($"Exec plantbase.Seed_Details_Read {seedId}");

                if (seeds == null)
                {
                    return new SeedDetailModel();
                }

                return seeds.First();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}