using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum DatabaseProvider
{
    [Display(Name = "PostgreSQL")]
    PostgreSQL,

    [Display(Name = "SQL Server")]
    SqlServer,

    [Display(Name = "MySQL")]
    MySql
}
