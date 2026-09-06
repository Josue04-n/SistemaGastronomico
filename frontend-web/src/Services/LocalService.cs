namespace src.Services;

using System.Net.Http.Json;
using src.Models;

public class LocalService
{
    private readonly HttpClient _httpClient;

    public LocalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LocalModel>> GetMisLocalesAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<LocalModel>>("api/locales/mis-locales");
            return result ?? new List<LocalModel>();
        }
        catch
        {
            return new List<LocalModel>();
        }
    }

    public async Task<LocalModel?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<LocalModel>($"api/locales/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Exito, LocalModel? Datos, string? Mensaje)> CreateAsync(CreateOrUpdateLocalModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/locales", model);
            if (response.IsSuccessStatusCode)
            {
                var created = await response.Content.ReadFromJsonAsync<LocalModel>();
                return (true, created, null);
            }

            var err = await response.Content.ReadAsStringAsync();
            return (false, null, "Error al crear el local.");
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<(bool Exito, LocalModel? Datos, string? Mensaje)> UpdateAsync(CreateOrUpdateLocalModel model)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/locales/{model.Id}", model);
            if (response.IsSuccessStatusCode)
            {
                var updated = await response.Content.ReadFromJsonAsync<LocalModel>();
                return (true, updated, null);
            }

            var err = await response.Content.ReadAsStringAsync();
            return (false, null, "Error al actualizar el local.");
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<bool> ToggleAperturaAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"api/locales/{id}/toggle-apertura", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
