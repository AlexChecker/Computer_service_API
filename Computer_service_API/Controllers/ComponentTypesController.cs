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
    public class ComponentTypesController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public ComponentTypesController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/ComponentTypes
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ComponentType>>> GetComponentTypes(int page)
        {
          if (_context.ComponentTypes == null)
          {
              return NotFound();
          }
            if (_context.ComponentTypes.Count() < 10)
                return await _context.ComponentTypes.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.ComponentTypes.Count()) return NoContent();
                List<ComponentType> acs = new List<ComponentType>();

                for (int i = (int)(page * 10); i < _context.ComponentTypes.Count(); i++)
                {
                    acs.Add(_context.ComponentTypes.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/ComponentTypes/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ComponentType>> GetComponentType(int? id)
        {
          if (_context.ComponentTypes == null)
          {
              return NotFound();
          }
            var componentType = await _context.ComponentTypes.FindAsync(id);

            if (componentType == null)
            {
                return NotFound();
            }

            return componentType;
        }

        // PUT: api/ComponentTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutComponentType(int? id, ComponentType componentType)
        {
            if (id != componentType.TypeId)
            {
                return BadRequest();
            }

            _context.Entry(componentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentTypeExists(id))
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

        // POST: api/ComponentTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<ComponentType>> PostComponentType(ComponentType componentType)
        {
          if (_context.ComponentTypes == null)
          {
              return Problem("Entity set 'Computer_serviceContext.ComponentTypes'  is null.");
          }
            _context.ComponentTypes.Add(componentType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComponentType", new { id = componentType.TypeId }, componentType);
        }

        // DELETE: api/ComponentTypes/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteComponentType(int? id)
        {
            if (_context.ComponentTypes == null)
            {
                return NotFound();
            }
            var componentType = await _context.ComponentTypes.FindAsync(id);
            if (componentType == null)
            {
                return NotFound();
            }

            _context.ComponentTypes.Remove(componentType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComponentTypeExists(int? id)
        {
            return (_context.ComponentTypes?.Any(e => e.TypeId == id)).GetValueOrDefault();
        }
    }
}
