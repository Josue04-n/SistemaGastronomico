namespace SistemaGastronomico.Application.Promociones.Queries.GetPromocionesByLocal;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;

public record GetPromocionesByLocalQuery(Guid LocalId, bool? SoloActivas = null) : IRequest<List<PromocionDto>>;

public class GetPromocionesByLocalQueryHandler : IRequestHandler<GetPromocionesByLocalQuery, List<PromocionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPromocionesByLocalQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PromocionDto>> Handle(GetPromocionesByLocalQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Promociones
            .Where(p => p.LocalId == request.LocalId && p.IsActive)
            .AsNoTracking();

        if (request.SoloActivas.HasValue && request.SoloActivas.Value)
        {
            var now = DateTime.UtcNow;
            query = query.Where(p => p.EstaActiva && p.FechaInicio <= now && p.FechaFin >= now);
        }

        return await query
            .OrderByDescending(p => p.EstaActiva)
            .ThenByDescending(p => p.FechaInicio)
            .Select(p => new PromocionDto(
                p.Id,
                p.LocalId,
                p.Titulo,
                p.Descripcion,
                p.PrecioOriginal,
                p.PrecioPromocional,
                p.ImagenUrl,
                p.FechaInicio,
                p.FechaFin,
                p.EstaActiva
            ))
            .ToListAsync(cancellationToken);
    }
}
