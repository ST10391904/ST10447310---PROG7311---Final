using Microsoft.EntityFrameworkCore;
using PoeWebAPI.Models;

namespace PoeWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClientAPI> Clients { get; set; }
        public DbSet<ContractAPI> Contracts { get; set; }
        public DbSet<ServiceRequestAPI> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContractAPI>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Contracts)
                .HasForeignKey(c => c.ClientId);
        }
    }
}