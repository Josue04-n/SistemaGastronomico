namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;
using SistemaGastronomico.Domain.Enums;

public class Plato : BaseEntity
{
    public Guid LocalId { get; set; }
    public Local? Local { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string? ImagenUrl { get; set; }
    public CategoriaPlato Categoria { get; set; } = CategoriaPlato.Almuerzo;

    /// <summary>
    /// Soporta el módulo de actualización rápida: el dueño activa o desactiva la disponibilidad con un solo clic.
    /// </summary>
    public bool EstaDisponible { get; set; } = true;

    /// <summary>
    /// Identifica si el plato corresponde al plato/menú del día de hoy.
    /// </summary>
    public bool EsPlatoDelDia { get; set; } = false;

    public int OrdenVisualizacion { get; set; } = 0;
}
