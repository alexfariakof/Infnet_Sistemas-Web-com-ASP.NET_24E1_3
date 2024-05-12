﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Admin.Agreggates;
using Repository.Mapping.Admin;
using Domain.Core.ValueObject;

namespace Migrations.MsSqlServer;
public class MsSqlServerContextAdministravtive: DbContext
{
    public MsSqlServerContextAdministravtive(DbContextOptions<MsSqlServerContextAdministravtive> options) : base(options) { }
    public DbSet<AdministrativeAccount> Admin{ get; set; }    
   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AdminAccountMap());
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(x => x.AddConsole()));
        base.OnConfiguring(optionsBuilder);
    }
}