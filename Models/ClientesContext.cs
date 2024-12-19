using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
namespace WebClientHotel.Models;

public partial class ClientesContext : DbContext
{
    public ClientesContext()
    {
    }

    public ClientesContext(DbContextOptions<ClientesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            DotNetEnv.Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");
            }

            optionsBuilder.UseMySQL(connectionString);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while configuring the database: {ex.Message}");
            throw; // Re-throw the exception to ensure the application is aware of the failure
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cliente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .HasColumnName("apellido");
            entity.Property(e => e.Dias).HasColumnName("dias");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
            entity.Property(e => e.Rut)
                .HasMaxLength(20)
                .HasColumnName("rut");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
