using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PoeWebAPI.Models;

namespace PoeWebAPI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AttachToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var payload = JsonSerializer.Serialize(new { username, password });

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            return doc.RootElement.GetProperty("token").GetString();
        }

        public async Task<List<ContractAPI>> GetContractsAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("/api/contracts");

            if (!response.IsSuccessStatusCode)
                return new List<ContractAPI>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<ContractAPI>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ContractAPI>();
        }  

        public async Task<decimal> ConvertToZAR(string fromCurrency, decimal amount)
        {
            AttachToken();

            var payload = JsonSerializer.Serialize(new { fromCurrency, amount });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/currency/convert", content);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Failed to convert currency.");

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            return doc.RootElement.GetProperty("amountInZAR").GetDecimal();
        }
    }
}