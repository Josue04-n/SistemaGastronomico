namespace src.Models;

using System.ComponentModel.DataAnnotations;

public enum CampusUTAModel
{
    Huachi = 1,
    Ingahurco = 2,
    Querochaca = 3
}

public enum CategoriaLocalModel
{
    Almuerzos = 1,
    ComidaRapida = 2,
    Cafeteria = 3,
    Pizzeria = 4,
    Panaderia = 5,
    Mariscos = 6,
    Asadero = 7,
    VegetarianoVegano = 8,
    Otro = 99
}

public class LocalModel
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public CampusUTAModel CampusCercano { get; set; } = CampusUTAModel.Huachi;
    public CategoriaLocalModel Categoria { get; set; } = CategoriaLocalModel.Almuerzos;
    public bool EstaAbierto { get; set; } = true;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string? TelefonoContacto { get; set; }
    public string? WhatsApp { get; set; }
    public string? HorarioApertura { get; set; }
    public string? LogoUrl { get; set; }
    public string? PortadaUrl { get; set; }
    public Guid PropietarioId { get; set; }
    public string? PropietarioNombre { get; set; }
}

public class CreateOrUpdateLocalModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "El nombre del local es requerido.")]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es requerida.")]
    public string Direccion { get; set; } = string.Empty;

    public string? Referencia { get; set; }
    public CampusUTAModel CampusCercano { get; set; } = CampusUTAModel.Huachi;
    public CategoriaLocalModel Categoria { get; set; } = CategoriaLocalModel.Almuerzos;
    public double Latitud { get; set; } = -1.2687; // Coordenadas de referencia UTA Huachi
    public double Longitud { get; set; } = -78.6254;
    public string? TelefonoContacto { get; set; }
    public string? WhatsApp { get; set; }
    public string? HorarioApertura { get; set; } = "07:30 - 18:00";
    public string? LogoUrl { get; set; }
    public string? PortadaUrl { get; set; }
    public bool EstaAbierto { get; set; } = true;
    public Guid PropietarioId { get; set; }
}
