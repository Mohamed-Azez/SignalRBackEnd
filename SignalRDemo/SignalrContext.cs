using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SignalRDemo;

public partial class SignalrContext : DbContext
{
    public SignalrContext(DbContextOptions<SignalrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Signalr;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Connection>(entity =>
        {
            entity.ToTable("Connection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SignalRid).HasColumnName("SignalRId");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
