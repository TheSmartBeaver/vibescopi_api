using Microsoft.EntityFrameworkCore;
using VibeScopyAPI.Models;

namespace VibeScopyAPI.Infrastructure
{
	public class UnitOfWorkToto : DbContext
    {
        public UnitOfWorkToto(DbContextOptions<UnitOfWorkToto> options)
        : base(options)
        {

        }

        public DbSet<Profile> Profiles { get; set; } = default!;

        public DbSet<ProfileProposition> ProfilePropositions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.HasPostgresExtension("fuzzystrmatch"); // Fuzzy Search for LevenStein
            
        }
    }
}

