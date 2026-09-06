namespace SistemaGastronomico.Application.Menus.Commands.UpdatePlato;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Enums;

public record UpdatePlatoCommand(
    Guid Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    string? ImagenUrl,
    CategoriaPlato Categoria,
    bool EstaDisponible,
    bool EsPlatoDelDia
) : IRequest<PlatoDto>;

public class UpdatePlatoCommandValidator : AbstractValidator<UpdatePlatoCommand>
{
    public UpdatePlatoCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("El ID del plato es obligatorio.");

        RuleFor(v => v.Nombre)
            .NotEmpty().WithMessage("El nombre del plato es obligatorio.")
            .MaximumLength(150);

        RuleFor(v => v.Precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
    }
}

public class UpdatePlatoCommandHandler : IRequestHandler<UpdatePlatoCommand, PlatoDto>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlatoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PlatoDto> Handle(UpdatePlatoCommand request, CancellationToken cancellationToken)
    {
        var plato = await _context.Platos
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive, cancellationToken);

        if (plato == null)
        {
            throw new NotFoundException("Plato", request.Id);
        }

        plato.Nombre = request.Nombre.Trim();
        plato.Descripcion = request.Descripcion.Trim();
        plato.Precio = request.Precio;
        plato.ImagenUrl = request.ImagenUrl;
        plato.Categoria = request.Categoria;
        plato.EstaDisponible = request.EstaDisponible;
        plato.EsPlatoDelDia = request.EsPlatoDelDia;
        plato.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new PlatoDto(
            plato.Id,
            plato.LocalId,
            plato.Nombre,
            plato.Descripcion,
            plato.Categoria.ToString(),
            plato.Categoria,
            plato.Precio,
            plato.ImagenUrl,
            plato.EstaDisponible,
            plato.EsPlatoDelDia,
            plato.OrdenVisualizacion
        );
    }
}
