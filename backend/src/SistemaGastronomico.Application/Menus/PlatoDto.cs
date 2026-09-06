namespace SistemaGastronomico.Application.Menus;

using SistemaGastronomico.Domain.Enums;

public record PlatoDto(
    Guid Id,
    Guid LocalId,
    string Nombre,
    string Descripcion,
    string CategoriaNombre,
    CategoriaPlato Categoria,
    decimal Precio,
    string? ImagenUrl,
    bool EstaDisponible,
    bool EsPlatoDelDia,
    int OrdenVisualizacion
);
