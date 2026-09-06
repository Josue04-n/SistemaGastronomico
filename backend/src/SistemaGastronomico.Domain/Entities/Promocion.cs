namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;

public class Promocion : BaseEntity
{
    public Guid LocalId { get; set; }
    public Local? Local { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal? PrecioOriginal { get; set; }
    public decimal PrecioPromocional { get; set; }
    public string? ImagenUrl { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool EstaActiva { get; set; } = true;
}
