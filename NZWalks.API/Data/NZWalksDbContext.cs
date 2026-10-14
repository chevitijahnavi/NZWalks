
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.NativeInterop;
using NZWalks.API.Models;
using NZWalks.API.Models.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }
        //Seeding data means automatically adding some initial / default data into your database.
        //Think of it like putting some starting data into an empty database.
        //OnModelCreating() is an EF Core method used to configure how your C# classes (models) map to the database.
        //EF Core already makes many conventions automaticalBut sometimes you want to tell EF Core specific rules. That's where OnModelCreating() is useful.ly.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //seed the data for Difficulties
            //Easy, Medium , Hard
            var difficulties = new List<Difficulty>()
{
    new Difficulty()
    {
        Id=Guid.Parse("e1ba8fab-b57e-43d1-94d7-365cff9a25d8"),
        Name="Easy"
    },
     new Difficulty()
    {
        Id=Guid.Parse("77ef4588-cd0d-4101-afd5-a46f2456d6c8"),
        Name="Medium"
    },
      new Difficulty()
    {
        Id=Guid.Parse("43787b3a-7fde-44d0-8ef4-4d7f465c346f"),
        Name="Hard"
    }
};
            //The way to seed
            //Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //Seed data for Regions

            var regions = new List<Region>
            {
                new Region
                {
                    Id=Guid.Parse("17537ad4-edad-429c-a0ec-836e7fbd29ed"),
                    Name="Auckland",
                    Code="AKL",
                    RegionImageUrl="https://share.google/NGQhxYhv3iscmp3Ok"
                },
                new Region
                {
                    Id=Guid.Parse("d520dd3e-d8fa-4315-9af9-e0ef2c441e54"),
                    Name="Nelson",
                    Code="NSN",
                    RegionImageUrl="https://share.google/NGQhxYhv3iscmp3Ok"
                },
                new Region
                {
                    Id=Guid.Parse("113b638e-bb66-43fa-96bc-64263e64d8fa"),
                    Name="SouthLand",
                    Code="STL",
                    RegionImageUrl="https://share.google/NGQhxYhv3iscmp3Ok"
                }
            };

            modelBuilder.Entity<Region>().HasData(regions);


        }
    }
}
