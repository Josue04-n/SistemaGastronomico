namespace SistemaGastronomico.Application.Estadisticas.Commands.RegistrarEventoEstadistica;

using MediatR;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Entities;
using SistemaGastronomico.Domain.Enums;

public record RegistrarEventoEstadisticaCommand(
    Guid LocalId,
    TipoEventoEstadistica TipoEvento,
    Guid? EstudianteId = null,
    string? Detalle = null
) : IRequest<Guid>;

public class RegistrarEventoEstadisticaCommandHandler : IRequestHandler<RegistrarEventoEstadisticaCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public RegistrarEventoEstadisticaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(RegistrarEventoEstadisticaCommand request, CancellationToken cancellationToken)
    {
        var evento = new EstadisticaEvento
        {
            LocalId = request.LocalId,
            TipoEvento = request.TipoEvento,
            EstudianteId = request.EstudianteId,
            Detalle = request.Detalle,
            FechaRegistro = DateTime.UtcNow,
            IsActive = true
        };

        _context.Estadisticas.Add(evento);
        await _context.SaveChangesAsync(cancellationToken);

        return evento.Id;
    }
}
