using Repository.Constants;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace Migrations.MsSqlServer;
public class MsSqlServerContext : BaseContext<MsSqlServerContext>
{
    public override DefaultValueSqlConstants BASE_CONSTS { get;  } = new DefaultValueSqlConstants();
    public MsSqlServerContext(DbContextOptions<MsSqlServerContext> options) : base(options) 
    {
        this.BASE_CONSTS.CURRENT_DATE = "GetDate()";
    }
}
