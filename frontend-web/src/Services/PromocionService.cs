namespace src.Services;

using System.Net.Http.Json;
using src.Models;

public class PromocionService
{
    private readonly HttpClient _httpClient;

    public PromocionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PromocionModel>> GetPromocionesAsync(Guid localId)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<PromocionModel>>($"api/promociones/local/{localId}");
            return result ?? new List<PromocionModel>();
        }
        catch
        {
            return new List<PromocionModel>();
        }
    }

    public async Task<(bool Exito, PromocionModel? Datos, string? Mensaje)> CreateAsync(CreatePromocionModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/promociones", model);
            if (response.IsSuccessStatusCode)
            {
                var created = await response.Content.ReadFromJsonAsync<PromocionModel>();
                return (true, created, null);
            }

            return (false, null, "Error al crear la promoción.");
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<bool> ToggleAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"api/promociones/{id}/toggle", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
