using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventSource;

namespace Azure.SQLDB.Samples.DynamicSchema;

public class ToDoContext(DbContextOptions<ToDoContext> options) : DbContext(options)
{
    public DbSet<ToDo> ToDo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("global_sequence");

        modelBuilder.Entity<ToDo>(etb => {
            etb.OwnsOne(todo => todo.Extensions, builder => { builder.ToJson("extension").HasColumnType("json"); });
        });

        modelBuilder.Entity<ToDo>()
            .Property(todo => todo.Id)
            .HasDefaultValueSql("next value for dbo.global_sequence");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)       
        => optionsBuilder
        .LogTo(Console.WriteLine, (_, level) => level == LogLevel.Information)       
        .EnableSensitiveDataLogging();

}
