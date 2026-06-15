using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoeWebAPI.Data;
using PoeWebAPI.Models;
using PoeWebAPI.Services;

namespace PoeWebAPI.Controllers
{
    [ApiController]
    [Route("api/servicerequests")]
    public class ServiceRequestAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ApiService _currencyService;

        public ServiceRequestAPIController(
            AppDbContext context,
            ApiService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltered(
            string? clientName,
            ContractStatusAPI? status,
            DateOnly? startDate,
            DateOnly? endDate)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(clientName))
            {
                query = query.Where(c =>
                    c.Client.Name.Contains(clientName));
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(c =>
                    c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c =>
                    c.EndDate <= endDate.Value);
            }

            var contracts = await query.ToListAsync();

            var result = new List<ServiceRequestDTO>();

            foreach (var contract in contracts)
            {
                decimal zar = 0;

                try
                {
                    zar = await _currencyService.ConvertToZAR(
                        contract.Currency,
                        contract.Amount);
                }
                catch
                {
                    zar = 0;
                }

                result.Add(new ServiceRequestDTO
                {
                    ContractId = contract.ContractId,
                    ContractName = contract.ContractName,
                    Name = contract.Client?.Name,
                    Status = contract.Status,
                    Amount = contract.Amount,
                    Currency = contract.Currency,
                    AmountInZAR = zar,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate
                });
            }

            return Ok(result);
        }

        [HttpGet("sla/{id}")]
        public IActionResult RequestSLA(int id)
        {
            return Ok($"SLA requested for contract {id}");
        }
    }
}