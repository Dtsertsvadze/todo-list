namespace Services.WebApi;
using System.Net.Http.Json;
using Entities.DTOs.LogInDto;
using Entities.DTOs.RegisterDtos;
using Entities.Identity;

public class AccountWebApiService(HttpClient httpClient)
{
    public async Task<ApplicationUser?> RegisterAsync(RegisterRequest registerRequest)
    {
        var response = await httpClient.PostAsJsonAsync("api/account/register", registerRequest);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApplicationUser>();
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Registration failed: {errorContent}");
    }

    public async Task<bool> IsEmailAlreadyRegisteredAsync(string email)
    {
        var response = await httpClient.GetAsync($"api/account?email={Uri.EscapeDataString(email)}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        throw new HttpRequestException($"Failed to check email: {response.StatusCode}");
    }

    public async Task<AuthenticationResponse?> LoginAsync(LoginRequest loginRequest)
    {
        var response = await httpClient.PostAsJsonAsync("api/account/login", loginRequest);

        if (response.IsSuccessStatusCode)
        {
            var authenticationResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

            return authenticationResponse;
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error content: {errorContent}");

        throw new HttpRequestException($"Login failed: {response.StatusCode} - {errorContent}");
    }

    public async Task<bool> LogoutAsync()
    {
        var response = await httpClient.GetAsync("api/account/logout");
        return response.IsSuccessStatusCode;
    }
}
