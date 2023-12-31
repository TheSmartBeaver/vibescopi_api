using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using VibeScopyAPI.Models;
using VibeScopyAPI.Models.Enums;

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

        public DbSet<LaunchedActivity> LaunchedActivities { get; set; } = default!;

        public DbSet<UserPreferences> UserPreferences { get; set; } = default!;

        public DbSet<Photo> Photos { get; set; } = default!;

        public DbSet<UserLikeProfile> UserLikeProfiles { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.HasPostgresExtension("fuzzystrmatch"); // Fuzzy Search for LevenStein
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.Entity<UserProfile>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.AuthentUid).IsUnique();
            });

            modelBuilder.CreateEnumMapping<UserPreferences, Gender>(up => up.FriendGenders);
            modelBuilder.CreateEnumMapping<UserPreferences, Gender>(up => up.LovingGenders);
            modelBuilder.CreateEnumMapping<UserPreferences, RelationShip>(up => up.LookingRelationShips);
            modelBuilder.CreateEnumMapping<LaunchedActivity, Gender>(up => up.Gender);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Photos)
                .WithOne(e => e.UserProfile)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLikeProfile>()
                .HasKey(ul => new { ul.UserProfileId, ul.LikedPersonId });

            modelBuilder.Entity<UserLikeProfile>()
                .HasOne(ul => ul.UserProfile)        // Une clé étrangère vers UserProfile
                .WithMany()
                .HasForeignKey(ul => ul.UserProfileId);

            modelBuilder.Entity<UserLikeProfile>()
                .HasOne(ul => ul.LikedPerson)        // Une clé étrangère vers UserProfile
                .WithMany()
                .HasForeignKey(ul => ul.LikedPersonId);

            modelBuilder.Entity<UserProfile>()
                .HasMany(ul => ul.UsersLiked)        // Une clé étrangère vers UserProfile
                .WithOne(ul => ul.UserProfile)
                .HasForeignKey(ul => ul.UserProfileId);

            modelBuilder.Entity<Activity>()
                .HasMany(ul => ul.SubActivities)
                .WithOne();

            modelBuilder.Entity<LaunchedActivity>()
                .HasOne(x => x.Creator)
                .WithMany();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);
        }
}

