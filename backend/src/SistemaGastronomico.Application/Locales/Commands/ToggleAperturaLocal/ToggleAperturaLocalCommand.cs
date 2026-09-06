namespace SistemaGastronomico.Application.Locales.Commands.ToggleAperturaLocal;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;

public record ToggleAperturaLocalCommand(Guid LocalId) : IRequest<bool>;

public class ToggleAperturaLocalCommandHandler : IRequestHandler<ToggleAperturaLocalCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ToggleAperturaLocalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleAperturaLocalCommand request, CancellationToken cancellationToken)
    {
        var local = await _context.Locales
            .FirstOrDefaultAsync(l => l.Id == request.LocalId && l.IsActive, cancellationToken);

        if (local == null)
        {
            throw new NotFoundException("Local", request.LocalId);
        }

        local.EstaAbierto = !local.EstaAbierto;
        local.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return local.EstaAbierto;
    }
}
