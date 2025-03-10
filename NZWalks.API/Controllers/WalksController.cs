using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        //Post: /api/walks
        [HttpPost]
        [ValidateModel]

        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            {
                //Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await walkRepository.CreateAsync(walkDomainModel);

                //Map Domain Model into DTO
                var walkDto = mapper.Map<WalkDto>(walkDomainModel);

                return Ok(walkDto);
            }
        }



        // GET Walks
        //Get: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? FilterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, FilterQuery, sortBy, isAscending ?? true, 
                pageNumber, pageSize);

            //Map Domain Model to DTO
            return Ok(mapper.Map<LinkedList<WalkDto>>(walksDomainModel));
        }


        // GET Walk By Id
        //Get: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain model to DTO
            var walkDto = (mapper.Map<WalkDto>(walkDomainModel));

            return Ok(walkDto);
        }



        // Update walk by Id
        //  Put: /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]

        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkReqDto UpdateWalkReqDto)
        {
            {
                //Map DTO to domain Model
                var walkDomainModel = mapper.Map<Walk>(UpdateWalkReqDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                var walkDto = (mapper.Map<WalkDto>(walkDomainModel));
                return Ok(walkDto);
            }
        }


        // Delete walk by Id
        // DELETE: /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedDomainModel = await walkRepository.DeleteAsync(id);

            if (deletedDomainModel == null)
            {
                return NotFound();
            }

            var walkDto = (mapper.Map<WalkDto>(deletedDomainModel));

            return Ok(walkDto);
        }
    }
}
