using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Models.Sql;
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
                var seeds = await ExecuteSql<SeedItemModel>("Exec plantbase.Seed_Display_Read");

                if (seeds == null)
                {
                    return new List<SeedItemModel>();
                }

                return seeds;
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Seed_Display_Read", ex);
            }
        }

        public async Task<List<SqlSeedDetailsModel>> GetSeedDetails(long seedId, bool includeDeleted)
        {
            try
            {
                var seeds = await ExecuteSql<SqlSeedDetailsModel>($"Exec plantbase.Seed_Details_Read @seedId, @includeDeleted", new Dictionary<string, object>
                {
                    { "@seedId", seedId },
                    { "@includeDeleted", includeDeleted }
                });

                if (seeds == null)
                {
                    return new List<SqlSeedDetailsModel>();
                }

                return seeds;
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Seed_Details_Read", ex);
            }
        }

        public async Task SaveSeed(string seedDetails)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Seed_Save @seedDetails", new Dictionary<string, object>
                {
                    { "@seedDetails", seedDetails }
                });
            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Seed_Save", ex);
            }
        }

        public async Task DeleteSeed(long seedId)
        {
            try
            {
                await ExecuteSql($"Exec plantbase.Seed_Delete @seedId", new Dictionary<string, object>
                {
                    { "@seedId", seedId }
                });

            }
            catch (Exception ex)
            {
                throw new SqlFailureException("Failed to run Seed_Delete", ex);
            }
        }
    }
}