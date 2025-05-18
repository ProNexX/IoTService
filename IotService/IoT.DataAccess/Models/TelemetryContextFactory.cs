using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace IoT.DataAccess.Models
{
    public class TelemetryContextFactory : IDesignTimeDbContextFactory<TelemetryContext>
    {
        public TelemetryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TelemetryContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=telemetrydb;Username=postgres;Password=123456a.");

            return new TelemetryContext(optionsBuilder.Options);
        }
    }
}
