using Microsoft.EntityFrameworkCore;
using VibeScopyAPI.Models;

namespace VibeScopyAPI.Infrastructure
{
	public class VibeScopUnitOfWork : DbContext
    {
        public VibeScopUnitOfWork(DbContextOptions<VibeScopUnitOfWork> options)
        : base(options)
        {

        }

        public DbSet<UserProfile> Profiles { get; set; } = default!;

        public DbSet<ProfileProposition> ProfilePropositions { get; set; } = default!;

        public DbSet<AnswersFilament> AnswersFilaments { get; set; } = default!;

        public DbSet<Activity> Activities { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.HasPostgresExtension("fuzzystrmatch"); // Fuzzy Search for LevenStein
            modelBuilder.HasPostgresExtension("postgis");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);
        }
}

