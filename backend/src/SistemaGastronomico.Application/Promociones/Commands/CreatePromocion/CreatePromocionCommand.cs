namespace SistemaGastronomico.Application.Promociones.Commands.CreatePromocion;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Entities;

public record CreatePromocionCommand(
    Guid LocalId,
    string Titulo,
    string Descripcion,
    decimal? PrecioOriginal,
    decimal PrecioPromocional,
    string? ImagenUrl,
    DateTime FechaInicio,
    DateTime FechaFin
) : IRequest<PromocionDto>;

public class CreatePromocionCommandValidator : AbstractValidator<CreatePromocionCommand>
{
    public CreatePromocionCommandValidator()
    {
        RuleFor(v => v.LocalId)
            .NotEmpty().WithMessage("El ID del local es obligatorio.");

        RuleFor(v => v.Titulo)
            .NotEmpty().WithMessage("El título de la promoción es obligatorio.")
            .MaximumLength(150);

        RuleFor(v => v.PrecioPromocional)
            .GreaterThan(0).WithMessage("El precio promocional debe ser mayor a 0.");

        RuleFor(v => v.FechaFin)
            .GreaterThan(v => v.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");
    }
}

public class CreatePromocionCommandHandler : IRequestHandler<CreatePromocionCommand, PromocionDto>
{
    private readonly IApplicationDbContext _context;

    public CreatePromocionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PromocionDto> Handle(CreatePromocionCommand request, CancellationToken cancellationToken)
    {
        var localExiste = await _context.Locales
            .AnyAsync(l => l.Id == request.LocalId && l.IsActive, cancellationToken);

        if (!localExiste)
        {
            throw new NotFoundException("Local", request.LocalId);
        }

        var promo = new Promocion
        {
            LocalId = request.LocalId,
            Titulo = request.Titulo.Trim(),
            Descripcion = request.Descripcion.Trim(),
            PrecioOriginal = request.PrecioOriginal,
            PrecioPromocional = request.PrecioPromocional,
            ImagenUrl = request.ImagenUrl,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            EstaActiva = true,
            IsActive = true
        };

        _context.Promociones.Add(promo);
        await _context.SaveChangesAsync(cancellationToken);

        return new PromocionDto(
            promo.Id,
            promo.LocalId,
            promo.Titulo,
            promo.Descripcion,
            promo.PrecioOriginal,
            promo.PrecioPromocional,
            promo.ImagenUrl,
            promo.FechaInicio,
            promo.FechaFin,
            promo.EstaActiva
        );
    }
}
