using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using NZWalks.API.Models.Domain;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositries;
using AutoMapper;
using NZWalks.API.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
namespace NZWalks.API.Controllers
{
    //Route paramtere here represents the controllere if in the url api/RegionalController is mentioned then it points to this controller
    //for Example https://localhost:1234/api/regions it will bw pointing to the RegionalContoller 
    [Route("api/[controller]")]
    [ApiController]
    [DisableHttpMetrics]
   
    public class RegionsController : ControllerBase
    {
        //using the dependency injection
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, 
            IRegionRepository regionRepository, 
            IMapper mapper,
            ILogger<RegionsController> logger)

        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /* //IActionResult is an interface that represents the result/response your controller sends back to the client.
         //it is return type of contoller
         //It allows your method to return different kinds of HTTP responses.
         [HttpGet]
         //HttpGet is the attribute
         public IActionResult GetAll()
         {

             //this are the hardcoded list not the data from the tables
             var regions = new List<Region>
             {
                 new Region
                 {
             Id=Guid.NewGuid(),
                Name="Auckland",
                Code="AKL",
                 RegionImageUrl="https://share.google/19s3hQEuzDGUSOV8i"},

             new Region
             {
                 Id = Guid.NewGuid(),
                 Name = "Wellingtion Region",
 Code = "WLG",
                 RegionImageUrl = "https://share.google/19s3hQEuzDGUSOV8i"
             }
         };
             return Ok(regions);
             */
        /* This code is for not the hardcoded values but directly from the database
        [HttpGet]
        public IActionResult GetAll()
        {
            //dbContext class is used to access the SQL Server and database directly
            var regions = dbContext.Regions.ToList();
            //using dbContext for Talking to the Regions to get all the values
            return Ok(regions);
        } 
        */
        /* 
         //it is to retrieve the data by id
        //GET SINGLE REGION(Get Region By ID)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //find can be used only for id's
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }
        */

        //GET ALL REGIONS by DTO's
        // GET: https:/localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles ="Reader")]

        public async Task<IActionResult> GetAll()
        {

            try
            {
                //throw new Exception("This is a custom exception");
                var regionsDomain = await regionRepository.GetAllAsync();
                logger.LogInformation($"Finished GetAllRegions request with data:{JsonSerializer.Serialize(regionsDomain)}");
                var regionDTO = mapper.Map<List<RegionDTO>>(regionsDomain);
                //Return DTOs
                return Ok(regionDTO);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            /*
            logger.LogInformation("Get All Action Method was invoked");

            logger.LogWarning("This is a warning log");

            logger.LogError("This is a error log");
            
            //Get Data from Database - Domain models
            //var regionsDomain = await dbContext.Regions.ToListAsync();

//in the above dbContext is used to retrieve the data
// the repository has been used to get the data
var regionsDomain= await regionRepository.GetAllAsync();

            //here now the concrete class SQLRegionrepository is responsibel for giving the information
            //Here the RegionRepository is the layer between the database and the application



            //Map Domain Models to DTOs
            //var regionDTO = new List<RegionDTO>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionDTO.Add(new RegionDTO()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}

            //the above code is commented to use the AutoMapper
            //Map Domain Models to DTOs

            logger.LogInformation($"Finished GetAllRegions reuest with data:{JsonSerializer.Serialize(regionsDomain)}");
           var regionDTO = mapper.Map<List<RegionDTO>>(regionsDomain);
            //Return DTOs
            return Ok(regionDTO);

            //or
            //return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
            */
        }
        //Get The region by id by DTO
        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            
            //var region = dbContext.Regions.Find(id);
            //find can be used only for id's
            //Get Region Domain Model From Database

            //var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            //Instead of calling the dbContext directly calling using the repository
            var regionDomain = await regionRepository.GetByIdAsync(id);
                if (regionDomain == null)
                {
                    return NotFound();
                }
                //Map/Convert Region Domain Model to Region DTO
                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomain.Id,
                //    Code = regionDomain.Code,
                //    Name = regionDomain.Name,
                //    RegionImageUrl = regionDomain.RegionImageUrl
                //};
                //Return DTO  back to client
                //return Ok(regionDTO);
                return Ok(mapper.Map<RegionDTO>(regionDomain));
            }
           
        

        // Example of Create Region Action Method
        //POST To Create a New Region .;
        
        //POST:https://localhost:portnumber/api/regions
        [HttpPost]
        //[ValidateModel]
        //As it is post method and want information coming from the client we annotated with FromBody
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {

            /*
            if (ModelState.IsValid)
            {
                //whole beloe code for model validation in normal way
            }
            else
            {
                return BadRequest(ModelState);
            }
            */


            //Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);
            //var regionDomainModel = new Region
            //{
            //    //here we have to map the three properties that are coming from the AddRegionRequestDTO
            //    Code = addRegionRequestDTO.Code,
            //    Name = addRegionRequestDTO.Name,
            //    RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            //};

            //Use Domain Model to Create a Region
            //use the dbContext to add the Region
            //dbContext.Regions.Add(regionDomainModel);
            //At this moment nothing will happen not entry will be there
            //this should excuted by that Region will be created
           // dbContext.SaveChanges();

        regionDomainModel= await   regionRepository.CreateAsync(regionDomainModel);

            //Map Domain model back to DTO
            //it is to return the model to the client
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            //post method wont return ok method because that is 200 method
            //for the CreatedAtAction method first parameter the action name here GetById is passed beacause that is creating single resource
            //the location of the method should be passed explicitly through the return type
            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO.Id);

        }
        //Update region
        //PUT: https;//localhost:portnumbet/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        //[ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            //Another DTO will be create for the update method
            //Here FromBody is the mentioned beacuse this will be a request of the region values that we want to update
            //in the above the values that are being associated with id that will be assigned to the updateRegionRequest this process is called model binding/deserialization
            //check if region exists

            //var regionDomainModel = dbContext.Regions.Find(id);

            //map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequest);


            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequest.Code,
            //    Name = updateRegionRequest.Name,
            //    RegionImageUrl = updateRegionRequest.RegionImageUrl
            //};

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //Map DTO to Domain modal

            //updating the code which is coming from the request object
            //regionDomainModel.Code = updateRegionRequest.Code;
            //regionDomainModel.Name = updateRegionRequest.Name;
            //regionDomainModel.RegionImageUrl = updateRegionRequest.RegionImageUrl;
            //here no need of updating because it is updating or changing the existing object
            dbContext.SaveChanges();

            //convert domain model to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            //we wont pass the Domain Model always passes the DTO's
            return Ok(regionDTO);
            //or
            // return Ok(mapper.Map<RegionDTO>(regionDomainModel.Id);)
        }


        //Delete Region
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer, Reader")]
        public async  Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //Delete region
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();


            //return deleted region back
            //map Domain Model to DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            //return Ok(regionDTO);
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
            //return Ok() can also be returned
        }
    }
}

