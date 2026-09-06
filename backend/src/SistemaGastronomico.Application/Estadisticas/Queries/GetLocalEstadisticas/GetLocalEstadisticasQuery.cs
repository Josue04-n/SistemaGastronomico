namespace SistemaGastronomico.Application.Estadisticas.Queries.GetLocalEstadisticas;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Enums;

public record GetLocalEstadisticasQuery(Guid LocalId) : IRequest<LocalEstadisticasDto>;

public class GetLocalEstadisticasQueryHandler : IRequestHandler<GetLocalEstadisticasQuery, LocalEstadisticasDto>
{
    private readonly IApplicationDbContext _context;

    public GetLocalEstadisticasQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LocalEstadisticasDto> Handle(GetLocalEstadisticasQuery request, CancellationToken cancellationToken)
    {
        var local = await _context.Locales
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == request.LocalId && l.IsActive, cancellationToken);

        if (local == null)
        {
            throw new NotFoundException("Local", request.LocalId);
        }

        var totalVisitas = await _context.Estadisticas
            .CountAsync(e => e.LocalId == request.LocalId && e.TipoEvento == TipoEventoEstadistica.VisitaPerfil, cancellationToken);

        var totalMenusGuardados = await _context.MenusFavoritos
            .CountAsync(f => f.LocalId == request.LocalId && f.IsActive, cancellationToken);

        var totalPlatos = await _context.Platos
            .CountAsync(p => p.LocalId == request.LocalId && p.IsActive, cancellationToken);

        var platosDisponibles = await _context.Platos
            .CountAsync(p => p.LocalId == request.LocalId && p.IsActive && p.EstaDisponible, cancellationToken);

        var now = DateTime.UtcNow;
        var promocionesActivas = await _context.Promociones
            .CountAsync(pr => pr.LocalId == request.LocalId && pr.IsActive && pr.EstaActiva && pr.FechaFin >= now, cancellationToken);

        var resenas = await _context.Resenas
            .Where(r => r.LocalId == request.LocalId && r.IsActive)
            .Select(r => r.Calificacion)
            .ToListAsync(cancellationToken);

        var totalResenas = resenas.Count;
        var calificacionPromedio = totalResenas > 0 ? Math.Round(resenas.Average(), 1) : 0.0;

        return new LocalEstadisticasDto(
            local.Id,
            local.Nombre,
            totalVisitas,
            totalMenusGuardados,
            totalPlatos,
            platosDisponibles,
            promocionesActivas,
            totalResenas,
            calificacionPromedio
        );
    }
}
