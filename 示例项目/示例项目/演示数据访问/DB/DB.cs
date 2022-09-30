using Microsoft.EntityFrameworkCore;

using System.DataFrancis;
using System.DataFrancis.DB.EF;

namespace System;

#pragma warning disable CS8618

public sealed class DB : DbContextFrancis
{
    public DbSet<Student> Students { get; set; }

    private string Connection { get; }

    public DB()
    {
    }
    public DB(DbContextOptions options)
        : base(options)
    {
    }



    public DB(string connection)
    {
        Connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(Connection ?? CreateEFCoreDB.ConnectionStringLocal(nameof(DB)));
    }
}
