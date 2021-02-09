using Microsoft.EntityFrameworkCore;

using CameraClub.Function.Entities;

namespace CameraClub.Function
{
    public class CompetitionContext : DbContext
    {
        public CompetitionContext(DbContextOptions<CompetitionContext> options)
            : base(options)
        {
        }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Judge> Judges { get; set; }

        public DbSet<Photographer> Photographers { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<PhotoScore> PhotoScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoScore>()
                .HasKey(pk => new { pk.PhotoId, pk.JudgeId, pk.Round });
            modelBuilder.Entity<CompetitionJudge>()
                .HasKey(pk => new { pk.CompetitionId, pk.JudgeId });
        }
    }
}