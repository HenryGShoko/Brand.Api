using Brand.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brand.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Brand.Api.Models.Domain.Brand> Brands { get; set; }

    }
}
