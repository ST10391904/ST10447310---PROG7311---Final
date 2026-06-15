using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoeWebAPI.Models;
using PoeWebAPI.Data;

namespace PoeWebAPI.Controllers
{
   [ApiController]
[Route("api/clients")]
public class ClientControllerApi : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientControllerApi(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Clients.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClientAPI client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = client.ClientId }, client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ClientAPI client)
    {
        if (id != client.ClientId)
            return BadRequest();

        _context.Entry(client).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
}