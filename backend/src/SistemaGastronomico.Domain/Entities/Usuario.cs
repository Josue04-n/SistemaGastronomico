namespace SistemaGastronomico.Domain.Entities;

using SistemaGastronomico.Domain.Common;
using SistemaGastronomico.Domain.Enums;

public class Usuario : BaseEntity
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public RolUsuario Rol { get; set; } = RolUsuario.DuenoLocal;

    // Relaciones de navegación
    public ICollection<Local> Locales { get; set; } = new List<Local>();
    public ICollection<MenuFavorito> MenusFavoritos { get; set; } = new List<MenuFavorito>();
    public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
}
