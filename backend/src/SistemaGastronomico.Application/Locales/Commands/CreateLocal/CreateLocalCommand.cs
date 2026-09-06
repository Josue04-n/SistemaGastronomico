namespace SistemaGastronomico.Application.Locales.Commands.CreateLocal;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Entities;
using SistemaGastronomico.Domain.Enums;
using ValidationException = SistemaGastronomico.Application.Common.Exceptions.ValidationException;

public record CreateLocalCommand(
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
    Guid PropietarioId
) : IRequest<LocalDto>;

public class CreateLocalCommandValidator : AbstractValidator<CreateLocalCommand>
{
    public CreateLocalCommandValidator()
    {
        RuleFor(v => v.Nombre)
            .NotEmpty().WithMessage("El nombre del local es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no debe superar los 150 caracteres.");

        RuleFor(v => v.Direccion)
            .NotEmpty().WithMessage("La dirección es obligatoria.")
            .MaximumLength(250).WithMessage("La dirección no debe superar los 250 caracteres.");

        RuleFor(v => v.PropietarioId)
            .NotEmpty().WithMessage("El identificador del propietario es obligatorio.");
    }
}

public class CreateLocalCommandHandler : IRequestHandler<CreateLocalCommand, LocalDto>
{
    private readonly IApplicationDbContext _context;

    public CreateLocalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LocalDto> Handle(CreateLocalCommand request, CancellationToken cancellationToken)
    {
        var propietario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == request.PropietarioId && u.IsActive, cancellationToken);

        if (propietario == null)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("PropietarioId", "El propietario especificado no existe.")
            });
        }

        var local = new Local
        {
            Nombre = request.Nombre.Trim(),
            Descripcion = request.Descripcion.Trim(),
            Direccion = request.Direccion.Trim(),
            Referencia = request.Referencia?.Trim(),
            CampusCercano = request.CampusCercano,
            Categoria = request.Categoria,
            Latitud = request.Latitud,
            Longitud = request.Longitud,
            TelefonoContacto = request.TelefonoContacto?.Trim(),
            WhatsApp = request.WhatsApp?.Trim(),
            HorarioApertura = request.HorarioApertura?.Trim(),
            LogoUrl = request.LogoUrl,
            PortadaUrl = request.PortadaUrl,
            EstaAbierto = true,
            PropietarioId = request.PropietarioId,
            IsActive = true
        };

        // Relación 1:1 con Coordenadas
        local.Coordenadas = new Coordenada
        {
            LocalId = local.Id,
            Latitud = request.Latitud,
            Longitud = request.Longitud,
            DireccionReferencial = request.Referencia ?? request.Direccion,
            IsActive = true
        };

        _context.Locales.Add(local);
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
            propietario.NombreCompleto
        );
    }
}
