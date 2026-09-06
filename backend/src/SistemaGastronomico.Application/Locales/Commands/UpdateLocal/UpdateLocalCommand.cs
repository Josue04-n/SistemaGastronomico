namespace SistemaGastronomico.Application.Locales.Commands.UpdateLocal;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Exceptions;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Enums;

public record UpdateLocalCommand(
    Guid Id,
    string Nombre,
    string Descripcion,
    string Direccion,
    string? Referencia,
    CampusUTA CampusCercano,
    CategoriaLocal Categoria,
    double Latitud,
    double Longitud,
    string? TelefonoContacto,
    string? WhatsApp,
    string? HorarioApertura,
    string? LogoUrl,
    string? PortadaUrl,
    bool EstaAbierto
) : IRequest<LocalDto>;

public class UpdateLocalCommandValidator : AbstractValidator<UpdateLocalCommand>
{
    public UpdateLocalCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("El ID del local es obligatorio.");

        RuleFor(v => v.Nombre)
            .NotEmpty().WithMessage("El nombre del local es obligatorio.")
            .MaximumLength(150);

        RuleFor(v => v.Direccion)
            .NotEmpty().WithMessage("La dirección es obligatoria.")
            .MaximumLength(250);
    }
}

public class UpdateLocalCommandHandler : IRequestHandler<UpdateLocalCommand, LocalDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateLocalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LocalDto> Handle(UpdateLocalCommand request, CancellationToken cancellationToken)
    {
        var local = await _context.Locales
            .Include(l => l.Propietario)
            .Include(l => l.Coordenadas)
            .FirstOrDefaultAsync(l => l.Id == request.Id && l.IsActive, cancellationToken);

        if (local == null)
        {
            throw new NotFoundException("Local", request.Id);
        }

        local.Nombre = request.Nombre.Trim();
        local.Descripcion = request.Descripcion.Trim();
        local.Direccion = request.Direccion.Trim();
        local.Referencia = request.Referencia?.Trim();
        local.CampusCercano = request.CampusCercano;
        local.Categoria = request.Categoria;
        local.Latitud = request.Latitud;
        local.Longitud = request.Longitud;
        local.TelefonoContacto = request.TelefonoContacto?.Trim();
        local.WhatsApp = request.WhatsApp?.Trim();
        local.HorarioApertura = request.HorarioApertura?.Trim();
        local.LogoUrl = request.LogoUrl;
        local.PortadaUrl = request.PortadaUrl;
        local.EstaAbierto = request.EstaAbierto;
        local.UpdatedAt = DateTime.UtcNow;

        if (local.Coordenadas != null)
        {
            local.Coordenadas.Latitud = request.Latitud;
            local.Coordenadas.Longitud = request.Longitud;
            local.Coordenadas.DireccionReferencial = request.Referencia ?? request.Direccion;
            local.Coordenadas.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new LocalDto(
            local.Id,
            local.Nombre,
            local.Descripcion,
            local.Direccion,
            local.Referencia,
            local.CampusCercano,
            local.Categoria,
            local.EstaAbierto,
            local.Latitud,
            local.Longitud,
            local.TelefonoContacto,
            local.WhatsApp,
            local.HorarioApertura,
            local.LogoUrl,
            local.PortadaUrl,
            local.PropietarioId,
            local.Propietario?.NombreCompleto
        );
    }
}
