using Brand.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Brand.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetBrands([FromQuery] string sort = "name")
        {
            var brands = _context.Brands.AsQueryable();

            switch (sort.ToLower())
            {
                case "name_desc":
                    brands = brands.OrderByDescending(b => b.Name);
                    break;
                case "published":
                    brands = brands.OrderBy(b => b.PublishedDate);
                    break;
                case "published_desc":
                    brands = brands.OrderByDescending(b => b.PublishedDate);
                    break;
                default: // This covers the default "name" case as well.
                    brands = brands.OrderBy(b => b.Name);
                    break;
            }

            return Ok(brands.ToList());
        }
    }
}
