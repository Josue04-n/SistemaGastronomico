namespace SistemaGastronomico.Application.Menus.Commands.DeletePlato;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;

public record DeletePlatoCommand(Guid Id) : IRequest;

public class DeletePlatoCommandHandler : IRequestHandler<DeletePlatoCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePlatoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePlatoCommand request, CancellationToken cancellationToken)
    {
        var plato = await _context.Platos
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive, cancellationToken);

        if (plato == null)
        {
            throw new NotFoundException("Plato", request.Id);
        }

        // Borrado lógico
        plato.IsActive = false;
        plato.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
