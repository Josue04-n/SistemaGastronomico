namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;

public class MenuFavorito : BaseEntity
{
    public Guid EstudianteId { get; set; }
    public Usuario? Estudiante { get; set; }

    public Guid LocalId { get; set; }
    public Local? Local { get; set; }

    public DateTime FechaGuardado { get; set; } = DateTime.UtcNow;
}
