namespace SistemaGastronomico.Application.Promociones.Commands.TogglePromocion;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;

public record TogglePromocionCommand(Guid PromocionId) : IRequest<bool>;

public class TogglePromocionCommandHandler : IRequestHandler<TogglePromocionCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public TogglePromocionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(TogglePromocionCommand request, CancellationToken cancellationToken)
    {
        var promo = await _context.Promociones
            .FirstOrDefaultAsync(p => p.Id == request.PromocionId && p.IsActive, cancellationToken);

        if (promo == null)
        {
            throw new NotFoundException("Promocion", request.PromocionId);
        }

        promo.EstaActiva = !promo.EstaActiva;
        promo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return promo.EstaActiva;
    }
}
