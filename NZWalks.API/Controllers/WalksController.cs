using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;
using System.Reflection.Metadata;
using static System.Net.WebRequestMethods;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WalksController : ControllerBase
    {
        
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        

        //CREATE Walk
        //POST:/api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //create the mapping profile for this typoe of mapping
            //Map DTO to Domain Model
        var walkDomainModel =   mapper.Map<Walk>(addWalkRequestDto);

            //Repository for walks should be called , so create the repostiory for the walk
            await walkRepository.CreateAsync(walkDomainModel);

            //Map Domain model to DTO
           
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
        }

        //Get Walks
        //GET: /api/walks
        //for filtering
        //GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true
        [HttpGet]
       public async Task<IActionResult> GetAll([FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, 
            [FromQuery] int pageNumber, [FromQuery]int pageSize )
        {
         

                //filterQuery is simply a variable / parameter that contains the text the user wants to filter by.

                var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //Create an exception
            throw new Exception("This is a new exception");

            //Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDTO>>(walksDomainModel));
         
        }


        //Get Walk By ID
        //GET:/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel=await walkRepository.GetByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
        }

        //Update walk By Id
        //PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateRequestWalkDTO updateRequestWalkDTO)
        {
            //Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateRequestWalkDTO);
            await walkRepository.UpdateAsync(id, walkDomainModel);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //map Domain moels to DTO
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
        }


        //Delete a walk By id
        //DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
          var deleteWalkDomainModel=  await walkRepository.DeleteAsync(id);
        if(deleteWalkDomainModel == null)
            {
                return NotFound();
            }
            //Map Domain Model to DTO
            return Ok(mapper.Map<WalkDTO>(deleteWalkDomainModel));
        
        }
    }
}
