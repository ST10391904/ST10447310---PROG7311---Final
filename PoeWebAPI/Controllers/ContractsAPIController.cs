using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoeWebAPI.Models;
using PoeWebAPI.Services;
using PoeWebAPI.Data;

namespace PoeWebAPI
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractsControllerAPI : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ApiService _currencyService;

        public ContractsControllerAPI(AppDbContext context, ApiService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _context.Contracts
                .Include(c => c.Client)
                .ToListAsync();

            return Ok(contracts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

[HttpPost]
public async Task<IActionResult> Create([FromBody] ContractDTO dto)
{
    try
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var contract = new ContractAPI
        {
            ContractName = dto.ContractName,
            ClientId = dto.ClientID,
            Currency = dto.Currency,
            Amount = dto.Amount,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            FileName = dto.FileName,
            FilePath = dto.FilePath
        };

        contract.AmountInZAR = await Convert(contract);

        _context.Contracts.Add(contract);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { id = contract.ContractId },
            contract);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.ToString());
    }
}
[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, [FromBody] ContractDTO dto)
{
    var contract = await _context.Contracts.FindAsync(id);

    if (contract == null)
        return NotFound();

    contract.ContractName = dto.ContractName;
    contract.ClientId = dto.ClientID;
    contract.Currency = dto.Currency;
    contract.Amount = dto.Amount;
    contract.StartDate = dto.StartDate;
    contract.EndDate = dto.EndDate;
    contract.Status = dto.Status;
    contract.FileName = dto.FileName;
    contract.FilePath = dto.FilePath;

    contract.AmountInZAR = await Convert(contract);

    await _context.SaveChangesAsync();

    return NoContent();
}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       private async Task<decimal> Convert(ContractAPI contract)
{
    try
    {
        return await _currencyService.ConvertToZAR(
            contract.Currency,
            contract.Amount
        );
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return contract.Amount;
    }
}
}
}