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
    public class DepartmentsController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public DepartmentsController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments(int page)
        {
          if (_context.Departments == null)
          {
              return NotFound();
          }
            if (_context.Departments.Count() < 10)
                return await _context.Departments.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Departments.Count()) return NoContent();
                List<Department> acs = new List<Department>();

                for (int i = (int)(page * 10); i < page * 10 + 10; i++)
                {
                    try
                    {
                        acs.Add(_context.Departments.ToArray()[i]);
                    }
                    catch { break; }
                }
                return acs;
            }
        }

        // GET: api/Departments/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Department>> GetDepartment(int? id)
        {
          if (_context.Departments == null)
          {
              return NotFound();
          }
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutDepartment(int? id, Department department)
        {
            if (id != department.DepId)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
          if (_context.Departments == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Departments'  is null.");
          }
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.DepId }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteDepartment(int? id)
        {
            if (_context.Departments == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int? id)
        {
            return (_context.Departments?.Any(e => e.DepId == id)).GetValueOrDefault();
        }
    }
}
