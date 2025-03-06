using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // GET ALL REGIONS
        [HttpGet]
        public IActionResult GetAll()
        {
            // Get Data From Database - Domain models
            var regions = dbContext.Regions.ToList();

            // Map Domain models to DTOs
            var regionDTO = new List<RegionDTO>();
            foreach (var region in regions)
            {
                regionDTO.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            // Return DTOs
            return Ok(regions);
        }


        // GET SINGLE REGION BASED ON id
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            // Map Domain models to DTOs
            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(region);
        }



        //POST To Create New Region
        [HttpPost]
        public IActionResult Create() { }
    }
}
