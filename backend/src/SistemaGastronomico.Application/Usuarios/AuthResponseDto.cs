namespace SistemaGastronomico.Application.Usuarios;

public record AuthResponseDto(
    Guid UsuarioId,
    string NombreCompleto,
    string Email,
    string Rol,
    string Token
);
