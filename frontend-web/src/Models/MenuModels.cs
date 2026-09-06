namespace src.Models;

using System.ComponentModel.DataAnnotations;

public enum CategoriaPlatoModel
{
    Almuerzo = 1,
    Desayuno = 2,
    PlatoCarta = 3,
    ComidaRapida = 4,
    Bebida = 5,
    Postre = 6,
    Snack = 7,
    Combo = 8,
    Otro = 99
}

public class PlatoModel
{
    public Guid Id { get; set; }
    public Guid LocalId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string CategoriaNombre { get; set; } = string.Empty;
    public CategoriaPlatoModel Categoria { get; set; } = CategoriaPlatoModel.Almuerzo;
    public decimal Precio { get; set; }
    public string? ImagenUrl { get; set; }
    public bool EstaDisponible { get; set; } = true;
    public bool EsPlatoDelDia { get; set; } = false;
    public int OrdenVisualizacion { get; set; }
}

public class CreateOrUpdatePlatoModel
{
    public Guid? Id { get; set; }
    public Guid LocalId { get; set; }

    [Required(ErrorMessage = "El nombre del plato o combo es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Precio { get; set; } = 2.50m;

    public string? ImagenUrl { get; set; }
    public CategoriaPlatoModel Categoria { get; set; } = CategoriaPlatoModel.Almuerzo;
    public bool EstaDisponible { get; set; } = true;
    public bool EsPlatoDelDia { get; set; } = false;
}
