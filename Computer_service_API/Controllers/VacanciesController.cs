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
    public class VacanciesController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public VacanciesController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Vacancies
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Vacancy>>> GetVacancies(int page)
        {
          if (_context.Vacancies == null)
          {
              return NotFound();
          }
            if (_context.Vacancies.Count() < 10)
                return await _context.Vacancies.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Vacancies.Count()) return NoContent();
                List<Vacancy> acs = new List<Vacancy>();

                for (int i = (int)(page * 10); i < page * 10 + 10; i++)
                {
                    try
                    {
                        acs.Add(_context.Vacancies.ToArray()[i]);
                    }
                    catch { break; }
                }
                return acs;
            }
        }

        // GET: api/Vacancies/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Vacancy>> GetVacancy(int? id)
        {
          if (_context.Vacancies == null)
          {
              return NotFound();
          }
            var vacancy = await _context.Vacancies.FindAsync(id);

            if (vacancy == null)
            {
                return NotFound();
            }

            return vacancy;
        }

        // PUT: api/Vacancies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutVacancy(int? id, Vacancy vacancy)
        {
            if (id != vacancy.VacId)
            {
                return BadRequest();
            }

            _context.Entry(vacancy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacancyExists(id))
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

        // POST: api/Vacancies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Vacancy>> PostVacancy(Vacancy vacancy)
        {
          if (_context.Vacancies == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Vacancies'  is null.");
          }
            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVacancy", new { id = vacancy.VacId }, vacancy);
        }

        // DELETE: api/Vacancies/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteVacancy(int? id)
        {
            if (_context.Vacancies == null)
            {
                return NotFound();
            }
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }

            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VacancyExists(int? id)
        {
            return (_context.Vacancies?.Any(e => e.VacId == id)).GetValueOrDefault();
        }
    }
}
