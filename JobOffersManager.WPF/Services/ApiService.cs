using System.Net.Http;
using System.Net.Http.Json;
using JobOffersManager.Shared;

namespace JobOffersManager.WPF.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7101")
        };
    }

    public async Task<JobOffersResponseDto?> GetJobsAsync(int page = 1, int pageSize = 10)
    {
        return await _httpClient.GetFromJsonAsync<JobOffersResponseDto>(
            $"api/jobs?page={page}&pageSize={pageSize}");
    }
}
