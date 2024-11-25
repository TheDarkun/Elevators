using Elevators.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .HasOne<Round>()
            .WithMany(r => r.Players)
            .HasForeignKey(p => p.RoundId) // Ensure this is the actual foreign key
            .OnDelete(DeleteBehavior.Cascade);
    }

    
    public DbSet<User> Users { get; set; }
    public DbSet<Models.SelectedGuild> SelectedGuilds { get; set; }
    public DbSet<Models.Game> Games { get; set; }
    public DbSet<Models.Round> Rounds { get; set; }
}