namespace SistemaGastronomico.Application.Locales;

using SistemaGastronomico.Domain.Enums;

public record LocalDto(
    Guid Id,
    string Nombre,
    string Descripcion,
    string Direccion,
    string? Referencia,
    CampusUTA CampusCercano,
    CategoriaLocal Categoria,
    bool EstaAbierto,
    double Latitud,
    double Longitud,
    string? TelefonoContacto,
    string? WhatsApp,
    string? HorarioApertura,
    string? LogoUrl,
    string? PortadaUrl,
    Guid PropietarioId,
    string? PropietarioNombre
);
