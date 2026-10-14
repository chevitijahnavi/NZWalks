namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequest
    {
        public string Code { get; set; }
        //if clinet have no permission to update the Code ,then the property wont be considered in DTO
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

    }
}
