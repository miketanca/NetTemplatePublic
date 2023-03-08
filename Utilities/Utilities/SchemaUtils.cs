using Microsoft.EntityFrameworkCore;

namespace Utilities;

public static class SchemaUtils
{
    public static async Task<string> AddClassifications()
    {
        var options = new DbContextOptionsBuilder()
            .UseSqlServer(
                "Server=localhost;Database=web-ua;Integrated Security=True;TrustServerCertificate=True"
            )
            .Options;
        var db = new UtilityDbContext(options);

        var columns = await db.Columns
            .FromSql($"select * from INFORMATION_SCHEMA.COLUMNS")
            .ToListAsync();

        var classificationSql = columns
            .GroupBy(x => x.Table)
            .Select(x =>
            {
                var columns = string.Join(
                    "",
                    x.Select(y => $"    [{y.Schema}].[{y.Table}].[{y.Column}]\n")
                );
                return "ADD SENSITIVITY CLASSIFICATION TO\n"
                    + columns
                    + "    WITH (LABEL='my classification', INFORMATION_TYPE='other');\n";
            });
        return string.Join("\n", classificationSql);
    }
}
