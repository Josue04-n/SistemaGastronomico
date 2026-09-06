namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;
using SistemaGastronomico.Domain.Enums;

public class EstadisticaEvento : BaseEntity
{
    public Guid LocalId { get; set; }
    public Local? Local { get; set; }

    public TipoEventoEstadistica TipoEvento { get; set; }
    public Guid? EstudianteId { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public string? Detalle { get; set; }
}
