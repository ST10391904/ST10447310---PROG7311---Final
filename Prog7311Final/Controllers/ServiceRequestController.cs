using Microsoft.AspNetCore.Mvc;
using Prog7311Final.Models;
using System.Net.Http.Json;

namespace Prog7311Final.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public ServiceRequestController(IHttpClientFactory factory, IConfiguration config)
        {
            _http = factory.CreateClient();
            _baseUrl = $"{config["ApiSettings:BaseUrl"]}/api/servicerequests";
        }

        public async Task<IActionResult> Index(
            string? Name,
            ContractStatus? status,
            DateOnly? startDate,
            DateOnly? endDate)
        {
            var url =
            $"{_baseUrl}?clientName={Name}&status={status}&startDate={startDate}&endDate={endDate}";

            var data = await _http.GetFromJsonAsync<List<ServiceRequest>>(url);
            return View(data);
        }

        public async Task<IActionResult> RequestSLA(int id)
        {
            var response = await _http.GetAsync($"{_baseUrl}/sla/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Unable to request SLA");

            return Content(await response.Content.ReadAsStringAsync());
        }
    }
}