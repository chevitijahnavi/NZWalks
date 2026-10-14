using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{

    public class AutoMapperProfiles : Profile
    {
        /* Basic Example of Mapping
         public AutoMapperProfiles()
         {
             CreateMap<UserDTO, UserDomain>()
                  .ForMemb<er(x => x.Name, opt => opt.MapFrom(x => x.FullName)).ReverseMap();

         }
        */

        //lets create map between the Region Domain Model and Region DTO
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionRequest, Region>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<UpdateRequestWalkDTO, Walk>().ReverseMap();
        }


    }
    public class UserDTO
    {
        public string FullName {  get; set; }
    }
    public class UserDomain
    {
        public string Name { get; set; }
    }
}
