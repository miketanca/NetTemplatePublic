using Microsoft.EntityFrameworkCore;

namespace Utilities;

public static class SchemaUtils
{
    public static async Task AddClassifications()
    {
        var options = new DbContextOptionsBuilder().UseSqlServer(
            "Server=localhost;Database=web-ua;Integrated Security=True;TrustServerCertificate=True"
        ).Options;
        var db = new UtilityDbContext(options);

        var columns = await db.Columns.FromSql($"select * from INFORMATION_SCHEMA.COLUMNS").ToListAsync();
    }
}
