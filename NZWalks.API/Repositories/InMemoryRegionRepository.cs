using NZWalks.API.Models.Domain;
using NZWalks.API.Repositries;

namespace NZWalks.API.Repositories
{
    public class InMemoryRegionRepository
    {
        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>
            {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "GMN",
                    Name = "Gmmnnsmnai"
                }
            };
        }
    }
}
