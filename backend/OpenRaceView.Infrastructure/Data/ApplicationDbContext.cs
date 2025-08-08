using Microsoft.EntityFrameworkCore;
using OpenRaceView.Domain.Entities;

namespace OpenRaceView.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Lap> Laps { get; set; }
    public DbSet<LapSample> LapSamples { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Lap configuration
        modelBuilder.Entity<Lap>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.TrackName)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.StartTimeUtc)
                .IsRequired();
            
            entity.Property(e => e.DurationMs)
                .IsRequired();
            
            entity.Property(e => e.CreatedUtc)
                .IsRequired();

            entity.HasIndex(e => e.StartTimeUtc);
            entity.HasIndex(e => e.TrackName);
        });

        // LapSample configuration
        modelBuilder.Entity<LapSample>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.LapId)
                .IsRequired();
            
            entity.Property(e => e.Index)
                .IsRequired();
            
            entity.Property(e => e.TimestampOffsetMs)
                .IsRequired();
            
            entity.Property(e => e.Latitude)
                .IsRequired()
                .HasPrecision(10, 7);
            
            entity.Property(e => e.Longitude)
                .IsRequired()
                .HasPrecision(10, 7);

            // Foreign key relationship
            entity.HasOne(e => e.Lap)
                .WithMany(l => l.Samples)
                .HasForeignKey(e => e.LapId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(e => e.LapId);
            entity.HasIndex(e => new { e.LapId, e.Index })
                .IsUnique();
        });
    }
}