using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prog7311Final.Models;
using System.Net.Http.Json;

namespace Prog7311Final.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _http;

        private readonly string _baseUrl;
        private readonly string _clientUrl;

        public ContractsController(
            IWebHostEnvironment env,
            IHttpClientFactory factory,
            IConfiguration config)
        {
            _env = env;
            _http = factory.CreateClient();

            var apiBase = config["ApiSettings:BaseUrl"];

            _baseUrl = $"{apiBase}/api/contracts";
            _clientUrl = $"{apiBase}/api/clients";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _http.GetAsync(_baseUrl);

                if (!response.IsSuccessStatusCode)
                    return View(new List<Contract>());

                var contracts = await response.Content.ReadFromJsonAsync<List<Contract>>();

                return View(contracts ?? new List<Contract>());
            }
            catch
            {
                return View(new List<Contract>());
            }
        }

        public async Task<IActionResult> Create()
        {
            await LoadClients();
            SetCurrencies();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract)
        {
            if (!ModelState.IsValid)
            {
                await LoadClients();
                SetCurrencies();
                return View(contract);
            }

            await SaveFile(contract);

            var dto = new ContractDTO
            {
                ContractName = contract.ContractName,
                ClientID = contract.ClientId,
                Currency = contract.Currency,
                Amount = contract.Amount,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Status = contract.Status,
                FileName = contract.FileName,
                FilePath = contract.FilePath
            };

            var response = await _http.PostAsJsonAsync(_baseUrl, dto);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            await LoadClients();
            SetCurrencies();

            ModelState.AddModelError("", "API Error");
            return View(contract);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _http.GetAsync($"{_baseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var contract = await response.Content.ReadFromJsonAsync<Contract>();
            if (contract == null) return NotFound();

            await LoadClients();
            SetCurrencies();
            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (id != contract.ContractId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadClients();
                SetCurrencies();
                return View(contract);
            }

            await SaveFile(contract);

            var dto = new ContractDTO
            {
                ContractName = contract.ContractName,
                ClientID = contract.ClientId,
                Currency = contract.Currency,
                Amount = contract.Amount,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Status = contract.Status,
                FileName = contract.FileName,
                FilePath = contract.FilePath
            };

            var response = await _http.PutAsJsonAsync($"{_baseUrl}/{id}", dto);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            await LoadClients();
            SetCurrencies();

            ModelState.AddModelError("", "API Error");
            return View(contract);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _http.GetAsync($"{_baseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var contract = await response.Content.ReadFromJsonAsync<Contract>();
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _http.DeleteAsync($"{_baseUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadClients()
        {
            try
            {
                var response = await _http.GetAsync(_clientUrl);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Clients = new SelectList(new List<Client>(), "ClientId", "Name");
                    return;
                }

                var clients = await response.Content.ReadFromJsonAsync<List<Client>>();

                clients ??= new List<Client>();

                ViewBag.Clients = new SelectList(clients, "ClientId", "Name");
            }
            catch
            {
                ViewBag.Clients = new SelectList(new List<Client>(), "ClientId", "Name");
            }
        }

        private void SetCurrencies()
        {
            ViewBag.Currencies = new SelectList(new[]
            {
                "USD", "EUR", "JPY", "ZAR"
            });
        }

        private async Task SaveFile(Contract contract)
        {
            if (contract.UploadFile == null)
                return;

            string folder = Path.Combine(_env.WebRootPath, "FileServer", "Contracts");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(contract.UploadFile.FileName);
            string path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await contract.UploadFile.CopyToAsync(stream);

            contract.FileName = contract.UploadFile.FileName;
            contract.FilePath = "/FileServer/Contracts/" + fileName;
        }
    }
}