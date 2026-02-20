using System.Net.Http;
using System.Net.Http.Json;
using JobOffersManager.Shared;
using System.Diagnostics;

namespace JobOffersManager.WPF.Services;

public class ApiService : IDisposable
{
    private readonly HttpClient _httpClient;
    private bool _disposed;

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5134/")
        };
    }

    public async Task<JobOffersResponseDto?> GetJobsAsync(
        int page = 1,
        int pageSize = 5,
        string? location = null,
        string? seniority = null)
    {
        try
        {
            var url = $"api/jobs?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(location))
                url += $"&location={Uri.EscapeDataString(location)}";

            if (!string.IsNullOrWhiteSpace(seniority))
                url += $"&seniority={Uri.EscapeDataString(seniority)}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<JobOffersResponseDto>();
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"HTTP Error in GetJobsAsync: {ex.Message}");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            Debug.WriteLine($"Timeout in GetJobsAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<JobOfferDto?> CreateJobAsync(CreateJobOfferDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/jobs", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"API Error ({response.StatusCode}): {errorContent}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<JobOfferDto>();
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"HTTP Error in CreateJobAsync: {ex.Message}");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            Debug.WriteLine($"Timeout in CreateJobAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/jobs/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"HTTP Error in DeleteJobAsync: {ex.Message}");
            return false;
        }
        catch (TaskCanceledException ex)
        {
            Debug.WriteLine($"Timeout in DeleteJobAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<JobOfferDto?> UpdateJobAsync(int id, UpdateJobOfferDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/jobs/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"API Error ({response.StatusCode}): {errorContent}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<JobOfferDto>();
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"HTTP Error in UpdateJobAsync: {ex.Message}");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            Debug.WriteLine($"Timeout in UpdateJobAsync: {ex.Message}");
            return null;
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _httpClient?.Dispose();
        _disposed = true;
    }
}
