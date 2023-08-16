using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;


        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        //CREATE Walk
        //POST
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
                var walksModel = _mapper.Map<Walk>(addWalkRequestDto);
                await _walkRepository.CreateAsync(walksModel);
                //Map Domain model to DTO
                return Ok(_mapper.Map<WalkDto>(walksModel));
        }

        [HttpGet]
        //GET : /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {

                var walksModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

                //Create an exception
                throw new Exception("This is a new exception");
                return Ok(_mapper.Map<List<WalkDto>>(walksModel));

        }
        //GET walk by id
        [HttpGet]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkModel = await _walkRepository.GetByIdAsync(id);
            if (walkModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walkModel));
        }
        //Update
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
           
                var walkModel = _mapper.Map<Walk>(updateWalkRequestDto);
                walkModel = await _walkRepository.UpdateAsync(id, walkModel);
                if (walkModel == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<WalkDto>(walkModel));
           
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkModel = await _walkRepository.DeleteByIdAsync(id);
            if (deletedWalkModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(deletedWalkModel));

        }
    }
}
