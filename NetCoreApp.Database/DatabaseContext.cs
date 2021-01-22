using Microsoft.EntityFrameworkCore;
using NetCoreApp.Database.Model;

namespace NetCoreApp.Database
{
  public class DatabaseContext : DbContext
  {
    public virtual DbSet<User> Users { get; set; }

    public DatabaseContext( DbContextOptions<DatabaseContext> options ) : base( options )
    {
    }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
    }
  }
}
