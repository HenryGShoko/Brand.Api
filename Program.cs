using Brand.Api.Data;
using Brand.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BrandConnectionString"));
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed the database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    SeedDatabase(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();  // Apply CORS middleware

app.UseAuthorization();
app.MapControllers();
app.Run();

void SeedDatabase(AppDbContext dbContext)
{
    // Delete all brands in the database
    dbContext.Brands.RemoveRange(dbContext.Brands);
    dbContext.SaveChanges();

    // Add seed data if there's none
    if (!dbContext.Brands.Any())
    {
        var brandData = File.ReadAllText("seedData.json");
        var brandsDto = JsonConvert.DeserializeObject<List<BrandDto>>(brandData);

        foreach (var brandDto in brandsDto)
        {
            var brand = new Brand.Api.Models.Domain.Brand
            {
                Name = brandDto.name,
                PublishedDate = DateTime.Parse(brandDto.published),
                ImgURL = brandDto.imgURL
            };
            dbContext.Brands.Add(brand);
        }

        dbContext.SaveChanges();
    }
}

public class BrandDto
{
    public string name { get; set; }
    public string published { get; set; }
    public string imgURL { get; set; }
}
