namespace SistemaGastronomico.Application.Menus.Commands.ToggleDisponibilidadPlato;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;

public record ToggleDisponibilidadPlatoCommand(Guid PlatoId) : IRequest<bool>;

public class ToggleDisponibilidadPlatoCommandHandler : IRequestHandler<ToggleDisponibilidadPlatoCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ToggleDisponibilidadPlatoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleDisponibilidadPlatoCommand request, CancellationToken cancellationToken)
    {
        var plato = await _context.Platos
            .FirstOrDefaultAsync(p => p.Id == request.PlatoId && p.IsActive, cancellationToken);

        if (plato == null)
        {
            throw new NotFoundException("Plato", request.PlatoId);
        }

        plato.EstaDisponible = !plato.EstaDisponible;
        plato.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return plato.EstaDisponible;
    }
}
