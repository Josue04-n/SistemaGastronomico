namespace SistemaGastronomico.Application.Locales.Queries.GetLocalesByPropietario;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;

public record GetLocalesByPropietarioQuery(Guid PropietarioId) : IRequest<List<LocalDto>>;

public class GetLocalesByPropietarioQueryHandler : IRequestHandler<GetLocalesByPropietarioQuery, List<LocalDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLocalesByPropietarioQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LocalDto>> Handle(GetLocalesByPropietarioQuery request, CancellationToken cancellationToken)
    {
        var locales = await _context.Locales
            .Include(l => l.Propietario)
            .Where(l => l.PropietarioId == request.PropietarioId && l.IsActive)
            .AsNoTracking()
            .OrderBy(l => l.Nombre)
            .Select(l => new LocalDto(
                l.Id,
                l.Nombre,
                l.Descripcion,
                l.Direccion,
                l.Referencia,
                l.CampusCercano,
                l.Categoria,
                l.EstaAbierto,
                l.Latitud,
                l.Longitud,
                l.TelefonoContacto,
                l.WhatsApp,
                l.HorarioApertura,
                l.LogoUrl,
                l.PortadaUrl,
                l.PropietarioId,
                l.Propietario != null ? l.Propietario.NombreCompleto : null
            ))
            .ToListAsync(cancellationToken);

        return locales;
    }
}
