namespace SistemaGastronomico.Application.Estadisticas;

public record LocalEstadisticasDto(
    Guid LocalId,
    string LocalNombre,
    int TotalVisitas,
    int TotalMenusGuardados,
    int TotalPlatos,
    int PlatosDisponibles,
    int PromocionesActivas,
    int TotalResenas,
    double CalificacionPromedio
);
