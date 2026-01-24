using AspireCraft.Core.Common.Extensions;

namespace AspireCraft.Core.Common.Enums;

public enum DatabaseProvider
{
    [DisplayName("SQL Server")]
    SqlServer,

    [DisplayName("PostgreSQL")]
    PostgreSQL,

    [DisplayName("MySQL")]
    MySQL,

    [DisplayName("SQLite")]
    SQLite,

    [DisplayName("MongoDB")]
    MongoDB,
}
