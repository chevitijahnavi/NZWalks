using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext<IdentityUser>
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }
        //Create the roles and we want to seed  same roles into the database
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerId= "ec8bdf97 - 36b6 - 44ae - ab6d - 7d7bab1edd7e";
            var writerRoleId = "b7fedfc7-4135-448b-93c6-07caabb33fb5";
            //Creating roles to seed the data
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=readerId,
                    ConcurrencyStamp=readerId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id=writerRoleId,
                    ConcurrencyStamp=writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()
                }

            };

            //seeding the data into database
            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
