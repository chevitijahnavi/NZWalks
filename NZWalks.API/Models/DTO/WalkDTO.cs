using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string WalkImageUrl { get; set; }
        //this are being considered in the below DTO's so no needed
        //public Guid DifficultyId { get; set; }
        //public Guid RegionId { get; set; }
        //Navigation Properties
        
        //Region and Difficulty information should be include sp include DTO's
        public RegionDTO Region { get; set; }
        public DifficultyDTO Difficulty { get; set; }
    }
}
