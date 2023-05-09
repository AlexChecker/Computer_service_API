using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Computer_service_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Computer_service_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public ClientsController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients(int page)
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }

            if (_context.Clients.Count() < 10)
            {
                List<Client> clients = new List<Client>();
                foreach (var c in await _context.Clients.ToListAsync())
                {
                    if (c.Deleted == false) clients.Add(c);
                }
                return clients;
            }
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Clients.Count()) return NoContent();
                List<Client> acs = new List<Client>();

                for (int i = (int)(page * 10); i < _context.Clients.Count(); i++)
                {
                    if (_context.Clients.ToArray()[i].Deleted == false)
                    acs.Add(_context.Clients.ToArray()[i]);
                }
                return acs;
            }


        }

        // GET: api/Clients/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutClient(string id, Client client)
        {
            if (id != client.Login)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
          if (_context.Clients == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Clients'  is null.");
          }
            _context.Clients.Add(client);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.Login))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClient", new { id = client.Login }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteClient(string id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            //_context.Clients.Remove(client);
            client.Deleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost, Route("restore"), Authorize]
        public async Task<IActionResult> RestoreClient(string login)
        {
            Client client = await _context.Clients.FindAsync(login);
            if (client != null)
            {
                client.Deleted = false;
                await _context.SaveChangesAsync();
                return Ok("Client restored!");
            }
            return NotFound();

        }


        private bool ClientExists(string id)
        {
            return (_context.Clients?.Any(e => e.Login == id)).GetValueOrDefault();
        }
    }
}
