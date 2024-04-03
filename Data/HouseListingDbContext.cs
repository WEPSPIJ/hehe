using Microsoft.EntityFrameworkCore;

namespace HouseListAPI.Data
{
    public class HouseListingDbContext :DbContext
    {
        public HouseListingDbContext(DbContextOptions options):base(options)
        {

            
        }
        public DbSet<House> Houses { get; set; }   
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "England",
                    ShortName = "EN"
                },
                new Country
                {
                    Id = 2,
                    Name = "Sudan",
                    ShortName = "SD"
                },
                new Country
                {
                    Id = 3,
                    Name = "Pakistan",
                    ShortName ="PK"

                },
                new Country
                {
                    Id = 4,
                    Name ="Nigeria",
                    ShortName = "NI"
                }
                );

            modelBuilder.Entity<House>().HasData(
                new House
                {
                    Id = 1,
                    Name = "Imans House",
                    Address = "Gainsborough",
                    CountryId = 3,
                    Rating = 3.5.ToString()
                },
                new House
                {
                    Id = 2,
                    Name = "PSP Offices",
                    Address = "Spalding",
                    CountryId = 1,
                    Rating = 4.5.ToString()
                }
                );
        }
    }
}
