namespace src.Services;

using System.Net.Http.Json;
using src.Models;

public class MenuService
{
    private readonly HttpClient _httpClient;

    public MenuService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PlatoModel>> GetPlatosAsync(Guid localId)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<PlatoModel>>($"api/platos/local/{localId}");
            return result ?? new List<PlatoModel>();
        }
        catch
        {
            return new List<PlatoModel>();
        }
    }

    public async Task<(bool Exito, PlatoModel? Datos, string? Mensaje)> CreateAsync(CreateOrUpdatePlatoModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/platos", model);
            if (response.IsSuccessStatusCode)
            {
                var created = await response.Content.ReadFromJsonAsync<PlatoModel>();
                return (true, created, null);
            }

            return (false, null, "Error al crear el plato.");
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<(bool Exito, PlatoModel? Datos, string? Mensaje)> UpdateAsync(CreateOrUpdatePlatoModel model)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/platos/{model.Id}", model);
            if (response.IsSuccessStatusCode)
            {
                var updated = await response.Content.ReadFromJsonAsync<PlatoModel>();
                return (true, updated, null);
            }

            return (false, null, "Error al actualizar el plato.");
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/platos/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Módulo de actualización rápida: Alterna disponibilidad del plato con 1 clic.
    /// </summary>
    public async Task<bool> ToggleDisponibleAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"api/platos/{id}/toggle-disponible", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Módulo de actualización rápida: Alterna si es plato del día con 1 clic.
    /// </summary>
    public async Task<bool> TogglePlatoDelDiaAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"api/platos/{id}/toggle-plato-del-dia", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
