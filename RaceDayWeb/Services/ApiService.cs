using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RaceDayWeb.Models;

namespace RaceDayWeb.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"] ?? "https://localhost:5201");
    }

    private void AddAuthHeader()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var cookie = session?.GetString("AuthCookie");
        if (!string.IsNullOrEmpty(cookie))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookie);
        }
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(responseContent);
        }

        return null;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/auth/register", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<EventDto>?> GetEventsAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/events");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EventDto>>(content);
        }

        return null;
    }

    public async Task<EventDto?> GetEventAsync(int id)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync($"/api/events/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EventDto>(content);
        }

        return null;
    }

    public async Task<bool> CreateEventAsync(EventCreateRequest request)
    {
        AddAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/events", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateEventAsync(int id, EventUpdateRequest request)
    {
        AddAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"/api/events/{id}", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        AddAuthHeader();
        var response = await _httpClient.DeleteAsync($"/api/events/{id}");

        return response.IsSuccessStatusCode;
    }

    public async Task<List<CategoryDto>?> GetCategoriesByEventAsync(int eventId)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync($"/api/categories/event/{eventId}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CategoryDto>>(content);
        }

        return null;
    }

    public async Task<bool> CreateCategoryAsync(CategoryCreateRequest request)
    {
        AddAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/categories", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EnrolAsync(EnrolmentRequest request)
    {
        AddAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/enrolments", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<EnrolmentDto>?> GetMyEnrolmentsAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/enrolments/my-enrolments");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EnrolmentDto>>(content);
        }

        return null;
    }

    public async Task<List<EnrolmentDto>?> GetEventEnrolmentsAsync(int eventId)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync($"/api/enrolments/event/{eventId}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EnrolmentDto>>(content);
        }

        return null;
    }

    public async Task<bool> CreateResultAsync(ResultCreateRequest request)
    {
        AddAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/results", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<ResultDto>?> GetMyResultsAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/results/my-results");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ResultDto>>(content);
        }

        return null;
    }

    public async Task<UserProfileDto?> GetProfileAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/users/profile");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfileDto>(content);
        }

        return null;
    }

    public async Task<bool> UpdateProfileAsync(UserProfileUpdateRequest request)
    {
        AddAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("/api/users/profile", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<string?> UploadFileAsync(IFormFile file, string type)
    {
        AddAuthHeader();
        using var content = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();
        content.Add(new StreamContent(stream), "file", file.FileName);

        var endpoint = type == "banner" ? "/api/files/upload/event-banner" : "/api/files/upload/profile-picture";
        var response = await _httpClient.PostAsync(endpoint, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UploadResponse>(responseContent);
            return result?.ImageUrl;
        }

        return null;
    }
}
