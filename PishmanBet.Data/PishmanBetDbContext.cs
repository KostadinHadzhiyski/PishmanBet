using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PishmanBet.Data.Models;

namespace PishmanBet.Data
{
    public class PishmanBetDbContext : IdentityDbContext
    {
        public PishmanBetDbContext(DbContextOptions<PishmanBetDbContext> options)
            : base(options)
        {
        }

        public DbSet<FootballTeam> Teams { get; set; }
        public DbSet<FootballMatch> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FootballMatch>()
                .HasOne(fm => fm.AwayTeam)
                .WithMany()
                .HasForeignKey(fm => fm.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FootballMatch>()
              .HasOne(fm => fm.HomeTeam)
              .WithMany()
              .HasForeignKey(fm => fm.HomeTeamId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}