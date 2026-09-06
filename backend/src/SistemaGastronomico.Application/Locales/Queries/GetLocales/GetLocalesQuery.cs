namespace SistemaGastronomico.Application.Locales.Queries.GetLocales;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Enums;

public record GetLocalesQuery(
    CampusUTA? Campus = null,
    CategoriaLocal? Categoria = null,
    string? Search = null
) : IRequest<List<LocalDto>>;

public class GetLocalesQueryHandler : IRequestHandler<GetLocalesQuery, List<LocalDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLocalesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LocalDto>> Handle(GetLocalesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Locales
            .Include(l => l.Propietario)
            .Where(l => l.IsActive)
            .AsNoTracking();

        if (request.Campus.HasValue)
        {
            query = query.Where(l => l.CampusCercano == request.Campus.Value);
        }

        if (request.Categoria.HasValue)
        {
            query = query.Where(l => l.Categoria == request.Categoria.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower().Trim();
            query = query.Where(l => l.Nombre.ToLower().Contains(search) || l.Descripcion.ToLower().Contains(search));
        }

        var locales = await query
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
