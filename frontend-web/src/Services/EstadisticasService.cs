namespace src.Services;

using System.Net.Http.Json;
using src.Models;

public class EstadisticasService
{
    private readonly HttpClient _httpClient;

    public EstadisticasService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LocalEstadisticasModel?> GetEstadisticasAsync(Guid localId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<LocalEstadisticasModel>($"api/estadisticas/local/{localId}");
        }
        catch
        {
            return null;
        }
    }
}
