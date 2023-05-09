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
    public class ComponentUsagesController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public ComponentUsagesController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/ComponentUsages
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ComponentUsage>>> GetComponentUsages(int page)
        {
          if (_context.ComponentUsages == null)
          {
              return NotFound();
          }
            if (_context.Acquisitions.Count() < 10)
                return await _context.ComponentUsages.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.ComponentUsages.Count()) return NoContent();
                List<ComponentUsage> acs = new List<ComponentUsage>();

                for (int i = (int)(page * 10); i < _context.ComponentUsages.Count(); i++)
                {
                    acs.Add(_context.ComponentUsages.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/ComponentUsages/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ComponentUsage>> GetComponentUsage(int? id)
        {
          if (_context.ComponentUsages == null)
          {
              return NotFound();
          }
            var componentUsage = await _context.ComponentUsages.FindAsync(id);

            if (componentUsage == null)
            {
                return NotFound();
            }

            return componentUsage;
        }

        // PUT: api/ComponentUsages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutComponentUsage(int? id, ComponentUsage componentUsage)
        {
            if (id != componentUsage.UsageId)
            {
                return BadRequest();
            }

            _context.Entry(componentUsage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentUsageExists(id))
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

        // POST: api/ComponentUsages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<ComponentUsage>> PostComponentUsage(ComponentUsage componentUsage)
        {
          if (_context.ComponentUsages == null)
          {
              return Problem("Entity set 'Computer_serviceContext.ComponentUsages'  is null.");
          }
            _context.ComponentUsages.Add(componentUsage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComponentUsage", new { id = componentUsage.UsageId }, componentUsage);
        }

        // DELETE: api/ComponentUsages/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteComponentUsage(int? id)
        {
            if (_context.ComponentUsages == null)
            {
                return NotFound();
            }
            var componentUsage = await _context.ComponentUsages.FindAsync(id);
            if (componentUsage == null)
            {
                return NotFound();
            }

            _context.ComponentUsages.Remove(componentUsage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComponentUsageExists(int? id)
        {
            return (_context.ComponentUsages?.Any(e => e.UsageId == id)).GetValueOrDefault();
        }
    }
}
