namespace src.Models;

using System.ComponentModel.DataAnnotations;

public class PromocionModel
{
    public Guid Id { get; set; }
    public Guid LocalId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal? PrecioOriginal { get; set; }
    public decimal PrecioPromocional { get; set; }
    public string? ImagenUrl { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool EstaActiva { get; set; }
}

public class CreatePromocionModel
{
    public Guid LocalId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    public string Titulo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public decimal? PrecioOriginal { get; set; }

    [Required(ErrorMessage = "El precio promocional es obligatorio.")]
    [Range(0.01, 999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal PrecioPromocional { get; set; } = 2.00m;

    public string? ImagenUrl { get; set; }

    public DateTime FechaInicio { get; set; } = DateTime.Today;
    public DateTime FechaFin { get; set; } = DateTime.Today.AddDays(7);
}
