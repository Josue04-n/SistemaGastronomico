namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;

public class Coordenada : BaseEntity
{
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string? DireccionReferencial { get; set; }

    // Relación 1:1 con Local
    public Guid LocalId { get; set; }
    public Local? Local { get; set; }
}
