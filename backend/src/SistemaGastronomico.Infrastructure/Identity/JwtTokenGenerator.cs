namespace SistemaGastronomico.Infrastructure.Identity;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Entities;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Usuario usuario)
    {
        var secretKey = _configuration["JwtSettings:Secret"] ?? "ClaveSecretaSuperSeguraParaSistemaGastronomicoUTA2026";
        var issuer = _configuration["JwtSettings:Issuer"] ?? "SistemaGastronomicoUTA";
        var audience = _configuration["JwtSettings:Audience"] ?? "SistemaGastronomicoUTACommunity";
        var expiryMinutes = int.TryParse(_configuration["JwtSettings:ExpiryMinutes"], out var minutes) ? minutes : 1440; // 24 horas

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.NombreCompleto),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Rol.ToString()),
            new("rol", usuario.Rol.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
