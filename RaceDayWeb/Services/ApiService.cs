using System.Net.Http.Json;
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
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    private void AddAuthHeader()
    {
        // Forward the API session cookie to the API so it knows who the user is
        var apiSessionCookie = _httpContextAccessor.HttpContext?.Session.GetString("ApiSessionCookie");
        if (!string.IsNullOrEmpty(apiSessionCookie))
        {
            _httpClient.DefaultRequestHeaders.Add("Cookie", $".AspNetCore.Session={apiSessionCookie}");
        }
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

        if (response.IsSuccessStatusCode)
        {
            // Store the API session cookie for future requests
            var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
            if (setCookieHeader != null)
            {
                var match = System.Text.RegularExpressions.Regex.Match(setCookieHeader, @"\.AspNetCore\.Session=([^;]+)");
                if (match.Success)
                {
                    var sessionCookie = match.Groups[1].Value;
                    _httpContextAccessor.HttpContext?.Session.SetString("ApiSessionCookie", sessionCookie);
                }
            }
            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }

        return null;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LogoutAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.PostAsync("/api/auth/logout", null);

        // Clear the stored API session cookie regardless of response
        _httpContextAccessor.HttpContext?.Session.Remove("ApiSessionCookie");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<EventDto>?> GetEventsAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/events");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<EventDto>>();
        }

        return null;
    }

    public async Task<EventDto?> GetEventAsync(int id)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync($"/api/events/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EventDto>();
        }

        return null;
    }

    public async Task<bool> CreateEventAsync(EventCreateRequest request)
    {
        AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync("/api/events", request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateEventAsync(int id, EventUpdateRequest request)
    {
        AddAuthHeader();
        var response = await _httpClient.PutAsJsonAsync($"/api/events/{id}", request);

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
            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        }

        return null;
    }

    public async Task<bool> CreateCategoryAsync(CategoryCreateRequest request)
    {
        AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync("/api/categories", request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EnrolAsync(EnrolmentRequest request)
    {
        AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync("/api/enrolments", request);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<EnrolmentDto>?> GetMyEnrolmentsAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/enrolments/my-enrolments");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<EnrolmentDto>>();
        }

        return null;
    }

    public async Task<List<EnrolmentDto>?> GetEventEnrolmentsAsync(int eventId)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync($"/api/enrolments/event/{eventId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<EnrolmentDto>>();
        }

        return null;
    }

    public async Task<bool> CreateResultAsync(ResultCreateRequest request)
    {
        AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync("/api/results", request);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<ResultDto>?> GetMyResultsAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/results/my-results");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<ResultDto>>();
        }

        return null;
    }

    public async Task<UserProfileDto?> GetProfileAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("/api/users/profile");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserProfileDto>();
        }

        return null;
    }

    public async Task<bool> UpdateProfileAsync(UserProfileUpdateRequest request)
    {
        AddAuthHeader();
        var response = await _httpClient.PutAsJsonAsync("/api/users/profile", request);

        return response.IsSuccessStatusCode;
    }

    public async Task<string?> UploadFileAsync(IFormFile file, string type)
    {
        AddAuthHeader();
        
        using var content = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "file", file.FileName);

        var endpoint = type == "banner" ? "/api/files/upload/event-banner" : "/api/files/upload/profile-picture";
        var response = await _httpClient.PostAsync(endpoint, content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<UploadResponse>();
            return result?.ImageUrl;
        }

        return null;
    }
}
