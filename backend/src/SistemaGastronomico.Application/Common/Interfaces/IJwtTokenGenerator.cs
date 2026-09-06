namespace SistemaGastronomico.Application.Common.Interfaces;

using SistemaGastronomico.Domain.Entities;

public interface IJwtTokenGenerator
{
    string GenerateToken(Usuario usuario);
}
