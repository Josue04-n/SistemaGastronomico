namespace src.Models;

public class LocalEstadisticasModel
{
    public Guid LocalId { get; set; }
    public string LocalNombre { get; set; } = string.Empty;
    public int TotalVisitas { get; set; }
    public int TotalMenusGuardados { get; set; }
    public int TotalPlatos { get; set; }
    public int PlatosDisponibles { get; set; }
    public int PromocionesActivas { get; set; }
    public int TotalResenas { get; set; }
    public double CalificacionPromedio { get; set; }
}
