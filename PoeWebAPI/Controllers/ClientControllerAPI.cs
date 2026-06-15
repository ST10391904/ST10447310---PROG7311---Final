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

        // Get clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Client.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _context.Client.FindAsync(id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        //Post Clients
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientAPI client)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Client.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = client.ClientId }, client);
        }

        //Update Clients
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientAPI client)
        {
            if (id != client.ClientId)
                return BadRequest();

            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Delete Clients
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Client.FindAsync(id);

            if (client == null)
                return NotFound();

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}