using Microsoft.EntityFrameworkCore;

using CameraClub.Function.Entities;

namespace CameraClub.Function
{
    public class CompetitionContext : DbContext
    {
        public CompetitionContext()
        {
            // Needed for Mock
        }

        public CompetitionContext(DbContextOptions<CompetitionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Competition> Competitions { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Club> Clubs { get; set; }

        public virtual DbSet<Judge> Judges { get; set; }

        public virtual DbSet<Photographer> Photographers { get; set; }

        public virtual DbSet<CompetitionPhotographer> CompetitionPhotographer { get; set; }

        public virtual DbSet<Photo> Photos { get; set; }

        public virtual DbSet<PhotoScore> PhotoScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>()
                .HasOne(c => c.Category);
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Photographer);
            modelBuilder.Entity<Photo>()
                .HasMany(ps => ps.PhotoScores);

            modelBuilder.Entity<Photographer>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Photographer>()
                .HasMany(p => p.Photos);

            modelBuilder.Entity<PhotoScore>()
                .HasKey(ps => new { ps.PhotoId, ps.JudgeId, ps.Round });
            modelBuilder.Entity<PhotoScore>()
                .HasOne(ps => ps.Judge);

            modelBuilder.Entity<CompetitionJudge>()
                .HasKey(pk => new { pk.CompetitionId, pk.JudgeId });

            modelBuilder.Entity<CompetitionPhotographer>()
                .HasKey(cp => new { cp.CompetitionId, cp.PhotographerId });

            modelBuilder.Entity<Competition>()
                .HasMany(c => c.CompetitionJudge);
            modelBuilder.Entity<Competition>()
                .HasMany(c => c.CompetitionPhotographer);
            modelBuilder.Entity<Competition>()
                .HasMany(c => c.Photos);

            modelBuilder.Entity<Judge>()
                .HasMany(j => j.CompetitionJudge);
        }
    }
}