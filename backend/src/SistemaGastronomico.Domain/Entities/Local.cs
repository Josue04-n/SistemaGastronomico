namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;
using SistemaGastronomico.Domain.Enums;

public class Local : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public CampusUTA CampusCercano { get; set; } = CampusUTA.Huachi;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string? TelefonoContacto { get; set; }
    public string? WhatsApp { get; set; }
    public string? HorarioApertura { get; set; }
    public string? LogoUrl { get; set; }
    public string? PortadaUrl { get; set; }
    public CategoriaLocal Categoria { get; set; } = CategoriaLocal.Almuerzos;
    public bool EstaAbierto { get; set; } = true;

    // Relación con el Propietario / Administrador del local
    public Guid PropietarioId { get; set; }
    public Usuario? Propietario { get; set; }

    // Relación 1:1 con Coordenadas
    public Coordenada? Coordenadas { get; set; }

    // Colecciones de navegación
    public ICollection<Plato> Platos { get; set; } = new List<Plato>();
    public ICollection<Promocion> Promociones { get; set; } = new List<Promocion>();
    public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
    public ICollection<EstadisticaEvento> Estadisticas { get; set; } = new List<EstadisticaEvento>();
    public ICollection<MenuFavorito> Favoritos { get; set; } = new List<MenuFavorito>();
}
