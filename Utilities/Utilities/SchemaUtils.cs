using Microsoft.EntityFrameworkCore;

namespace Utilities;

public static class SchemaUtils
{
    public static async Task<string> AddClassifications(string connectionString)
    {
        var options = new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
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

    public static async Task<string> CombineFlyway(params string[] folders)
    {
        var allContents = folders
            .SelectMany(x => Directory.GetFiles(x))
            .OrderBy(x => Path.GetFileName(x))
            .Select(x => File.ReadAllText(x))
            .Select(x => x + "\n");

        return string.Join("", allContents.Select(x => x + "\n"));
    }
}
