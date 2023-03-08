using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilities;

[Keyless]
public class DbColumn
{
    [Column("TABLE_CATALOG")]
    public string Catalog { get; set; }
    [Column("TABLE_SCHEMA")]
    public string Schema { get; set; }
    [Column("TABLE_NAME")]
    public string Table { get; set; }
    [Column("COLUMN_NAME")]
    public string Column { get; set; }
}
