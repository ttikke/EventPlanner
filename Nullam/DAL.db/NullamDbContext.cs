using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.db;

public class NullamDbContext : DbContext
{
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Participant> Participants { get; set; } = null!;
    public DbSet<Corporation> Corporations { get; set; } = null!;

    public NullamDbContext(DbContextOptions<NullamDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<Participant>().Property(p => p.Id).ValueGeneratedNever();
        modelBuilder.Entity<Corporation>().Property(c => c.Id).ValueGeneratedNever();
        
        modelBuilder.Entity<Event>().Property(e => e.Details).HasMaxLength(Event.MaxDetailsLength);
        modelBuilder.Entity<Participant>().Property(p => p.Details).HasMaxLength(Participant.MaxDetailsLength);
        modelBuilder.Entity<Corporation>().Property(c => c.Details).HasMaxLength(Corporation.MaxDetailsLength);
        
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Participants)
            .WithOne();
        
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Corporations)
            .WithOne();

    }
}