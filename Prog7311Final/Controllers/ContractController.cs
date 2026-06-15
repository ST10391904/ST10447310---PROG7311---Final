using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prog7311Final.Models;
using System.Net.Http.Json;
using System;

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
            var contracts = await _http.GetFromJsonAsync<List<Contract>>(_baseUrl);
            return View(contracts);
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

    var error = await response.Content.ReadAsStringAsync();
    ModelState.AddModelError("", $"API Error: {error}");

    await LoadClients();
    SetCurrencies();
    return View(contract);
}
        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _http.GetFromJsonAsync<Contract>($"{_baseUrl}/{id}");
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

    var error = await response.Content.ReadAsStringAsync();
    ModelState.AddModelError("", $"API Error: {error}");

    await LoadClients();
    SetCurrencies();
    return View(contract);
}

        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _http.GetFromJsonAsync<Contract>($"{_baseUrl}/{id}");
            return contract == null ? NotFound() : View(contract);
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
            var clients = await _http.GetFromJsonAsync<List<Client>>(_clientUrl);

            ViewBag.Clients = new SelectList(
                clients ?? new List<Client>(),
                "ClientID",
                "ClientName"
            );
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