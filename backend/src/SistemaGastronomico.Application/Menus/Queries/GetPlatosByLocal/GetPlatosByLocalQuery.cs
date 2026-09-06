namespace SistemaGastronomico.Application.Menus.Queries.GetPlatosByLocal;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Enums;

public record GetPlatosByLocalQuery(
    Guid LocalId,
    bool? SoloDisponibles = null,
    CategoriaPlato? Categoria = null
) : IRequest<List<PlatoDto>>;

public class GetPlatosByLocalQueryHandler : IRequestHandler<GetPlatosByLocalQuery, List<PlatoDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPlatosByLocalQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlatoDto>> Handle(GetPlatosByLocalQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Platos
            .Where(p => p.LocalId == request.LocalId && p.IsActive)
            .AsNoTracking();

        if (request.SoloDisponibles.HasValue && request.SoloDisponibles.Value)
        {
            query = query.Where(p => p.EstaDisponible);
        }

        if (request.Categoria.HasValue)
        {
            query = query.Where(p => p.Categoria == request.Categoria.Value);
        }

        return await query
            .OrderByDescending(p => p.EsPlatoDelDia)
            .ThenBy(p => p.OrdenVisualizacion)
            .ThenBy(p => p.Nombre)
            .Select(p => new PlatoDto(
                p.Id,
                p.LocalId,
                p.Nombre,
                p.Descripcion,
                p.Categoria.ToString(),
                p.Categoria,
                p.Precio,
                p.ImagenUrl,
                p.EstaDisponible,
                p.EsPlatoDelDia,
                p.OrdenVisualizacion
            ))
            .ToListAsync(cancellationToken);
    }
}
