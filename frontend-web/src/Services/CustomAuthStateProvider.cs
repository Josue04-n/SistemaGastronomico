namespace src.Services;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;
    private const string TokenKey = "authToken";

    public CustomAuthStateProvider(IJSRuntime jsRuntime, HttpClient httpClient)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;

        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }
        catch
        {
            return null;
        }
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            foreach (var kvp in keyValuePairs)
            {
                var key = kvp.Key;
                if (key == "role" || key == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                {
                    key = ClaimTypes.Role;
                }
                else if (key == "nameid" || key == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                {
                    key = ClaimTypes.NameIdentifier;
                }
                else if (key == "name" || key == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                {
                    key = ClaimTypes.Name;
                }
                else if (key == "email" || key == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                {
                    key = ClaimTypes.Email;
                }

                claims.Add(new Claim(key, kvp.Value?.ToString() ?? string.Empty));
            }
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/'));
    }
}
