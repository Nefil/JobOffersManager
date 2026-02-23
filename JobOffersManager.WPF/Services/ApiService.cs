using JobOffersManager.Shared;
using JobOffersManager.WPF.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace JobOffersManager.WPF.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public string? Token { get; private set; }
    public string? Role { get; private set; }

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7101/")
        };
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login",
                new { username, password });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result == null)
                return false;

            Token = result.Token;
            Role = result.Role?.Trim();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<JobOffersResponseDto?> GetJobsAsync(
        int page = 1,
        int pageSize = 5,
        string? location = null,
        string? seniority = null)
    {
        var url = $"api/jobs?page={page}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(location))
            url += $"&location={Uri.EscapeDataString(location)}";

        if (!string.IsNullOrWhiteSpace(seniority))
            url += $"&seniority={Uri.EscapeDataString(seniority)}";

        return await _httpClient.GetFromJsonAsync<JobOffersResponseDto>(url);
    }

    public async Task<JobOfferDto?> CreateJobAsync(CreateJobOfferDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/jobs", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<JobOfferDto>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Create job failed: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/jobs/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<JobOfferDto?> UpdateJobAsync(int id, UpdateJobOfferDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/jobs/{id}", dto);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<JobOfferDto>();
    }
}