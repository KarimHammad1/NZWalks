using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }
        //GET ALL REGIONS
        [HttpGet]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll Action Method was invoked");
            // Get Data From Database - Domain models  
           var regionsDomain = await _regionRepository.GetAllAsync();

           //Map Domain Models to DTO
          var regionsDTo = _mapper.Map<List<RegionDto>>(regionsDomain);
          _logger.LogInformation($"Finished GetAllRegions request with data : {JsonSerializer.Serialize(regionsDomain)}");
           // Return DTOs
           return Ok(regionsDTo);
        }
        //GET SINGLE REGION (Get Region by ID)
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //Get Region Domain Model From Database
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //Convert Region Domain to Region DTO
           var regionDTO = _mapper.Map<RegionDto>(region);

            return Ok(regionDTO);
        }
        //POST To Create a new Region
        //In a post method we receive a body from the client
        [HttpPost]
        [ValidateModel] // custom validation
        //We use another DTO that does not contain the ID because we are not get the ID from the user
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody]AddRegionRequestDto addRegionRequestDto)
        {
            
            
                //Convert DTo to Domain Model
                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
                //Use Domain Model To create Region
                await _regionRepository.CreateAsync(regionDomainModel);

                //Map Domain model back to DTo
                var regionDTO = _mapper.Map<RegionDto>(regionDomainModel);

                //http 201 that is used for http post
                return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
            
                 
            
        }
        //Update region
        //PUT
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateRegionRequestDTo updateRegionRequestDTo)
        {
            
            
                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDTo);
                regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                await _dbContext.SaveChangesAsync();
                //Convert Domain Model To DTo
                var regionDTO = _mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDTO);
           
        }
        //Delete Region
        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Writer")] 
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await _regionRepository.DeleteAsync(id);
            if (region == null)
            {
                return NotFound();

            }

            var regionDTo = _mapper.Map<RegionDto>(region);
            // return deleted region back
            return Ok(regionDTo);
        }
    }
}
