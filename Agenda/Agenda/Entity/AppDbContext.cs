using Microsoft.EntityFrameworkCore;

namespace Agenda.Entity;

/// <summary>
///     Contexto de Entity Framework Core para la base de datos de contactos.
/// </summary>
public class AppDbContext : DbContext {
    private readonly string _connection;

    public AppDbContext(string connection) {
        _connection = connection;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        _connection = "";
    }

    public DbSet<ContactosEntity> Contacto { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite(_connection);
    }

    public void EnsureCreated() {
        Database.EnsureCreated();
    }
}