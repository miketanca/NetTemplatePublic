using Microsoft.EntityFrameworkCore;

namespace Utilities;

public class UtilityDbContext : DbContext
{
    public UtilityDbContext(DbContextOptions options) : base(options) { }

    public DbSet<DbColumn> Columns { get; set; }
}
