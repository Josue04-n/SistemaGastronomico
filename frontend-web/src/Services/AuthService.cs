namespace src.Services;

using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using src.Models;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
    }

    public async Task<(bool Exito, string? Mensaje, AuthResponse? Datos)> LoginAsync(LoginModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResult != null && !string.IsNullOrWhiteSpace(authResult.Token))
                {
                    await ((CustomAuthStateProvider)_authStateProvider).MarkUserAsAuthenticated(authResult.Token);
                    return (true, null, authResult);
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, "Credenciales inválidas o correo no registrado.", null);
        }
        catch (Exception ex)
        {
            return (false, $"Error al conectar con el servidor: {ex.Message}", null);
        }
    }

    public async Task<(bool Exito, string? Mensaje, AuthResponse? Datos)> RegisterAsync(RegisterModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResult != null && !string.IsNullOrWhiteSpace(authResult.Token))
                {
                    await ((CustomAuthStateProvider)_authStateProvider).MarkUserAsAuthenticated(authResult.Token);
                    return (true, null, authResult);
                }
            }

            var error = await response.Content.ReadAsStringAsync();
            return (false, "Error al registrar el usuario. Verifique los datos o si el correo ya existe.", null);
        }
        catch (Exception ex)
        {
            return (false, $"Error al conectar con el servidor: {ex.Message}", null);
        }
    }

    public async Task LogoutAsync()
    {
        await ((CustomAuthStateProvider)_authStateProvider).MarkUserAsLoggedOut();
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return new UserInfo
        {
            Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            Nombre = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Rol = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
        };
    }
}
