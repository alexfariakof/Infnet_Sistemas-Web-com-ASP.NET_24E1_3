using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace Migrations.MySqlServer;
public class MySqlServerContextAdministrative : BaseContextAdmin<MySqlServerContextAdministrative>
{
    public MySqlServerContextAdministrative(DbContextOptions<MySqlServerContextAdministrative> options) : base(options) { }
}