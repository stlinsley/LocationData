namespace LocationData.Data.Data
{
    using Core.Models;
    using Core.Models.City;
    using Microsoft.EntityFrameworkCore;

    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options) : base(options)
        {

        }

        public DbSet<CombinedLocationData> CombinedLocationData { get; set; }
        public DbSet<Currency> Currency { get; set; }
    }
}
