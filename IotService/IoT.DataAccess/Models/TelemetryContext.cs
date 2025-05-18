using Microsoft.EntityFrameworkCore;

namespace IoT.DataAccess.Models
{
    public class TelemetryContext : DbContext
    {
        public TelemetryContext(DbContextOptions<TelemetryContext> options)
            : base(options)
        { }

        public DbSet<TelemetryData> TelemetryRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
            base.OnConfiguring(optionsBuilder);
        }
    }
}
