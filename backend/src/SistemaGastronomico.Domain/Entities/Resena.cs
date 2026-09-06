namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;

public class Resena : BaseEntity
{
    public Guid LocalId { get; set; }
    public Local? Local { get; set; }

    public Guid EstudianteId { get; set; }
    public Usuario? Estudiante { get; set; }

    public int Calificacion { get; set; } // Valor de 1 a 5 estrellas
    public string? Comentario { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}
