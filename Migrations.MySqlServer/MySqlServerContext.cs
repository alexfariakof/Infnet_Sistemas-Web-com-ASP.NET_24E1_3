using Repository.Constants;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace Migrations.MySqlServer;
public class MySqlServerContext : BaseContext<MySqlServerContext>
{
    public override DefaultValueSqlConstants BASE_CONSTS { get;  } = new DefaultValueSqlConstants();

    public MySqlServerContext(DbContextOptions<MySqlServerContext> options) : base(options)
    {
        this.BASE_CONSTS.CURRENT_DATE = "Now()";
    }
}
