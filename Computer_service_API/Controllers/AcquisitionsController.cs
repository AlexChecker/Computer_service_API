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
    public class AcquisitionsController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public AcquisitionsController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Acquisitions
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Acquisition>>> GetAcquisitions(int page)
        {
          if (_context.Acquisitions == null)
          {
              return NotFound();
          }
            if (_context.Acquisitions.Count() < 10)
                return await _context.Acquisitions.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if(page*10 > _context.Acquisitions.Count()) return NoContent();
                List<Acquisition> acs = new List<Acquisition>();
                
                for (int i = (int)(page * 10); i < page*10+10; i++)
                {
                    try
                    {
                        acs.Add(_context.Acquisitions.ToArray()[i]);
                    }
                    catch
                    {
                        break;
                    }
                }
                return acs;
            }
        }

        // GET: api/Acquisitions/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Acquisition>> GetAcquisition(int? id)
        {
          if (_context.Acquisitions == null)
          {
              return NotFound();
          }
            var acquisition = await _context.Acquisitions.FindAsync(id);

            if (acquisition == null)
            {
                return NotFound();
            }

            return acquisition;
        }

        // PUT: api/Acquisitions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutAcquisition(int? id, Acquisition acquisition)
        {
            if (id != acquisition.AcqId)
            {
                return BadRequest();
            }

            _context.Entry(acquisition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcquisitionExists(id))
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

        // POST: api/Acquisitions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Acquisition>> PostAcquisition(Acquisition acquisition)
        {
          if (_context.Acquisitions == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Acquisitions'  is null.");
          }
            _context.Acquisitions.Add(acquisition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAcquisition", new { id = acquisition.AcqId }, acquisition);
        }

        // DELETE: api/Acquisitions/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteAcquisition(int? id)
        {
            if (_context.Acquisitions == null)
            {
                return NotFound();
            }
            var acquisition = await _context.Acquisitions.FindAsync(id);
            if (acquisition == null)
            {
                return NotFound();
            }

            _context.Acquisitions.Remove(acquisition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AcquisitionExists(int? id)
        {
            return (_context.Acquisitions?.Any(e => e.AcqId == id)).GetValueOrDefault();
        }
    }
}
