using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Computer_service_API.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Computer_service_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public ComponentsController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Components
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Component>>> GetComponents(int page)
        {
          if (_context.Components == null)
          {
              return NotFound();
          }
            if (_context.Components.Count() < 10)
                return await _context.Components.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Components.Count()) return NoContent();
                List<Component> acs = new List<Component>();

                for (int i = (int)(page * 10); i < _context.Components.Count(); i++)
                {
                    acs.Add(_context.Components.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/Components/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Component>> GetComponent(string id)
        {
          if (_context.Components == null)
          {
              return NotFound();
          }
            var component = await _context.Components.FindAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            return component;
        }

        // PUT: api/Components/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutComponent(string id, Component component)
        {
            if (id != component.ArticleNum)
            {
                return BadRequest();
            }

            _context.Entry(component).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentExists(id))
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

        // POST: api/Components
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Component>> PostComponent(Component component)
        {
          if (_context.Components == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Components'  is null.");
          }
            _context.Components.Add(component);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ComponentExists(component.ArticleNum))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetComponent", new { id = component.ArticleNum }, component);
        }

        // DELETE: api/Components/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteComponent(string id)
        {
            if (_context.Components == null)
            {
                return NotFound();
            }
            var component = await _context.Components.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }

            _context.Components.Remove(component);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComponentExists(string id)
        {
            return (_context.Components?.Any(e => e.ArticleNum == id)).GetValueOrDefault();
        }
    }
}
