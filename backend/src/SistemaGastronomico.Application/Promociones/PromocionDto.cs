namespace SistemaGastronomico.Application.Promociones;

public record PromocionDto(
    Guid Id,
    Guid LocalId,
    string Titulo,
    string Descripcion,
    decimal? PrecioOriginal,
    decimal PrecioPromocional,
    string? ImagenUrl,
    DateTime FechaInicio,
    DateTime FechaFin,
    bool EstaActiva
);
