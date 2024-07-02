using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace Migrations.MsSqlServer;
public class MsSqlServerContextAdministrative : BaseContextAdmin<MsSqlServerContextAdministrative>
{
    public MsSqlServerContextAdministrative(DbContextOptions<MsSqlServerContextAdministrative> options) : base(options) { }
}