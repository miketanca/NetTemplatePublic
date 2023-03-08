using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
                var columns =
                    string.Join(",\n", x.Select(y => $"    [{y.Schema}].[{y.Table}].[{y.Column}]"))
                    + "\n";
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
            .OrderBy(x =>
            {
                var name = Path.GetFileName(x);
                var match = Regex.Match(name, @"^V(\d+)_(\d+)_(\d+)");
                var ns = new[]
                {
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value)
                };
                return 10000 * ns[0] + 100 * ns[1] + ns[2];
            })
            .Select(x => File.ReadAllText(x))
            .Select(x => x + "\n");

        return string.Join("", allContents.Select(x => x + "\n"));
    }
}
