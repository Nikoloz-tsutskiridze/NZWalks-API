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
using System.Data.Common;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        // GET ALL REGIONS
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain models
            var regions = await regionRepository.GetAllAsync();

            // Return DTOs
            logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regions)}");
            return Ok(mapper.Map<List<RegionDTO>>(regions));
        }


        // GET SINGLE REGION BASED ON id
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var region = await regionRepository.GetByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDTO>(region));
        }



        //POST To Create New Region
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            {

                //Map or convert DTO to Domain models
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Use Domain Model to create Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Map Domain model back to DTO
                var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }

        }


        //UPDATE Region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            {
                //Map DTO to Domain Model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                //Checking if region exists
                regionDomainModel = await regionRepository.UpdateAync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Convert Domain model to DTO
                var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

                return Ok(regionDto);
            }

        }



        //Delete Region
        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Return deleted region back
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
