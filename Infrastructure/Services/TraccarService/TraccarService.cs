using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using arkbo_inventory.Features.Devices;
using arkbo_inventory.Infrastructure.Data;

namespace arkbo_inventory.Infrastructure.Services;

public class TraccarService
{
    private readonly HttpClient _httpClient;
    private string? _authToken;
    private DateTime _tokenExpiry = DateTime.MinValue;
    private const string TraccarUsername = "traccar.com";
    private const string TraccarPassword = "Traccar";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TokenSessionKey = "TraccarToken";
    private readonly ApplicationDbContext _dbContext;

    public TraccarService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }
    
    private async Task<string> EnsureTokenAsync()
    {
        var session=_httpContextAccessor.HttpContext?.Session;
        string? sessionToken=session?.GetString(TokenSessionKey);

        if (string.IsNullOrEmpty(sessionToken))
        {
            var newToken = await LoginAndGetTokenAsync();
            session?.SetString(TokenSessionKey, newToken);
            return newToken;
        }
        return sessionToken;
    }
    public async Task<string> LoginAndGetTokenAsync()
    {
        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{TraccarUsername}:{TraccarPassword}"));

        var request = new HttpRequestMessage(HttpMethod.Post, "session/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
        request.Content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseData = await response.Content.ReadAsStringAsync();
        _authToken= responseData.Trim('"');
        var session=_httpContextAccessor.HttpContext?.Session;
        string? sessionToken=session?.GetString(TokenSessionKey);
        if (string.IsNullOrEmpty(sessionToken))
        {
            session?.SetString(TokenSessionKey, _authToken);
            return _authToken;
        }
        Console.WriteLine(responseData);
        return _authToken;
    }

    public async Task<List<TraccarDeviceResponse>?> GetDevicesForUserAsync()
    {
        var token = await EnsureTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Get, "devices");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",token);

        var response = await _httpClient.SendAsync(request);
        var contents = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Token expired, retrying with new token...");
                _authToken = null;
                return await GetDevicesForUserAsync();
            }
            Console.WriteLine($"Failed to get devices. Status code: {response.StatusCode}");
            Console.WriteLine("Error response: " + contents);
            return null;
        }
        return JsonSerializer.Deserialize<List<TraccarDeviceResponse>>(contents, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    }

    public async Task<List<TraccarDeviceResponse>?> DeployDeviceToTraccar(int deviceId)
    {
        var device = await _dbContext.arkbo_devices.FindAsync(deviceId);
        if (device == null)
            return null;
        var token = await EnsureTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Post, "devices");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var traccarPayload = new
        {
            name = device.Name,
            uniqueId = device.UniqueId,
            disabled=false
        };
        request.Content=new StringContent(JsonSerializer.Serialize(traccarPayload), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to create device in Traccar: {response.StatusCode} - {error}");
        }
        var contents = await response.Content.ReadAsStringAsync();
        var deployedDevice = JsonSerializer.Deserialize<TraccarDeviceResponse>(contents, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return new List<TraccarDeviceResponse> { deployedDevice };
    } 
    public class TraccarDeviceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UniqueId { get; set; } = string.Empty;
        public string Status{get;set;}= string.Empty;
    }
    
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
    
}