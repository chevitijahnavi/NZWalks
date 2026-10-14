namespace NZWalks.API.Models.DTO
{
    public class RegionDTO
    {
        //RegionDTO can be subset of the Domain can have one or multiple properties from the classes of Domain
        //if you want to expose the any property that can be included in this class
        //these are included from the Region class 
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
