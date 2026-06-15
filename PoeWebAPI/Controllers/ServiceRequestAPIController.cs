using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoeWebAPI.Data;
using PoeWebAPI.Models;

namespace PoeWebAPI.Controllers
{
    [ApiController]
    [Route("api/servicerequests")]
    public class ServiceRequestAPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceRequestAPIController(AppDbContext context)
        {
            _context = context;
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
                    c.Client != null &&
                    c.Client.Name.Contains(clientName));
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDate.Value);
            }

            var data = await query.ToListAsync();

            return Ok(data);
        }

        [HttpGet("sla/{id}")]
        public IActionResult RequestSLA(int id)
        {
            return Ok($"SLA requested for contract {id}");
        }
    }
}