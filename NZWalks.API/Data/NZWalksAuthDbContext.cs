using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerId = "c66a054a-19a2-464b-b209-f5acd81ee1a8";
            var writerId = "2dd932e7-8ccb-4beb-ac63-dc14d6af1947";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                 Id = readerId,
                 ConcurrencyStamp = readerId,
                 Name="Reader",
                 NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                 Id = writerId,
                 ConcurrencyStamp = writerId,
                 Name="Writer",
                 NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
