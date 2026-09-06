namespace SistemaGastronomico.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Domain.Entities;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<Local> Locales { get; }
    DbSet<Coordenada> Coordenadas { get; }
    DbSet<Plato> Platos { get; }
    DbSet<Promocion> Promociones { get; }
    DbSet<Resena> Resenas { get; }
    DbSet<EstadisticaEvento> Estadisticas { get; }
    DbSet<MenuFavorito> MenusFavoritos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
