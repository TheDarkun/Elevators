using Elevators.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Models.SelectedGuild> SelectedGuilds { get; set; }
    public DbSet<Models.Game> Games { get; set; }
}