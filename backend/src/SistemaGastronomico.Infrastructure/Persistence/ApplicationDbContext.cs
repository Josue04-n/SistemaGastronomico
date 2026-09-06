namespace SistemaGastronomico.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Entities;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Local> Locales => Set<Local>();
    public DbSet<Coordenada> Coordenadas => Set<Coordenada>();
    public DbSet<Plato> Platos => Set<Plato>();
    public DbSet<Promocion> Promociones => Set<Promocion>();
    public DbSet<Resena> Resenas => Set<Resena>();
    public DbSet<EstadisticaEvento> Estadisticas => Set<EstadisticaEvento>();
    public DbSet<MenuFavorito> MenusFavoritos => Set<MenuFavorito>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
