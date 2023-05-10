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
    public class EmployeesController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public EmployeesController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int page)
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            if (_context.Employees.Count() < 10)
            {
                List<Employee> clients = new List<Employee>();
                foreach (var c in await _context.Employees.ToListAsync())
                {
                    if (c.Deleted == false) clients.Add(c);
                }
                return clients;
            }
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Employees.Count()) return NoContent();
                List<Employee> acs = new List<Employee>();

                for (int i = (int)(page * 10); i < _context.Employees.Count(); i++)
                {
                    if (_context.Employees.ToArray()[i].Deleted == false)
                        acs.Add(_context.Employees.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutEmployee(string id, Employee employee)
        {
            if (id != employee.ServiceId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
          if (_context.Employees == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Employees'  is null.");
          }
            _context.Employees.Add(employee);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employee.ServiceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee", new { id = employee.ServiceId }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            //_context.Employees.Remove(employee);
            employee.Deleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpDelete,Route("multiple/delete")]
        public async Task<IActionResult> DeleteMultiple(string[] logins)
        {
            foreach (var login in logins)
            {
                var user = await _context.Employees.FirstOrDefaultAsync(p => (p.Login == login));
                if (user != null) user.Deleted = true;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost,Route("multiple/restore")]
        public async Task<IActionResult> RestoreMultiple(string[] logins)
        {
            foreach (var login in logins)
            {
                var user = await _context.Clients.FirstOrDefaultAsync(p => (p.Login == login));
                if (user != null) user.Deleted = false;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost,Route("restore"), Authorize]
        public async Task<IActionResult> RestoreEmployee(string login)
        {

            var employee = await _context.Employees.FindAsync(login);
            if (employee != null)
            {
                employee.Deleted = false;
                await _context.SaveChangesAsync();
                return Ok("Employee restored!");
            }
            return NotFound();
        }

        private bool EmployeeExists(string id)
        {
            return (_context.Employees?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
