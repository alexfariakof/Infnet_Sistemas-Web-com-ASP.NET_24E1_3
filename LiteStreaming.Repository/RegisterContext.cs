using Repository.Constants;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace Repository;
public class RegisterContext : BaseContext<RegisterContext>
{
    public override DefaultValueSqlConstants BASE_CONSTS { get; } = new DefaultValueSqlConstants();
    public RegisterContext(DbContextOptions<RegisterContext> options) : base(options) { }
}
