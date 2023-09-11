using Brand.Api.Data;
using Microsoft.AspNetCore.Http;
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
        public IActionResult GetBrands(string sortOrder, bool? isPublished)
        {
            var brands = _context.Brands.AsQueryable();

            if (isPublished.HasValue)
            {
                brands = brands.Where(b => b.IsPublished == isPublished.Value);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    brands = brands.OrderByDescending(b => b.Name);
                    break;
                default:
                    brands = brands.OrderBy(b => b.Name);
                    break;
            }

            return Ok(brands.ToList());
        }
    }

}
