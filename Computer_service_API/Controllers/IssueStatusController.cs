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
    public class IssueStatusController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public IssueStatusController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/IssueStatus
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<IssueStatus>>> GetIssueStatuses(int page)
        {
          if (_context.IssueStatuses == null)
          {
              return NotFound();
          }
            if (_context.IssueStatuses.Count() < 10)
                return await _context.IssueStatuses.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.IssueStatuses.Count()) return NoContent();
                List<IssueStatus> acs = new List<IssueStatus>();

                for (int i = (int)(page * 10); i < _context.IssueStatuses.Count(); i++)
                {
                    acs.Add(_context.IssueStatuses.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/IssueStatus/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<IssueStatus>> GetIssueStatus(int? id)
        {
          if (_context.IssueStatuses == null)
          {
              return NotFound();
          }
            var issueStatus = await _context.IssueStatuses.FindAsync(id);

            if (issueStatus == null)
            {
                return NotFound();
            }

            return issueStatus;
        }

        // PUT: api/IssueStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutIssueStatus(int? id, IssueStatus issueStatus)
        {
            if (id != issueStatus.StatusId)
            {
                return BadRequest();
            }

            _context.Entry(issueStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueStatusExists(id))
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

        // POST: api/IssueStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<IssueStatus>> PostIssueStatus(IssueStatus issueStatus)
        {
          if (_context.IssueStatuses == null)
          {
              return Problem("Entity set 'Computer_serviceContext.IssueStatuses'  is null.");
          }
            _context.IssueStatuses.Add(issueStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssueStatus", new { id = issueStatus.StatusId }, issueStatus);
        }

        // DELETE: api/IssueStatus/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteIssueStatus(int? id)
        {
            if (_context.IssueStatuses == null)
            {
                return NotFound();
            }
            var issueStatus = await _context.IssueStatuses.FindAsync(id);
            if (issueStatus == null)
            {
                return NotFound();
            }

            _context.IssueStatuses.Remove(issueStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssueStatusExists(int? id)
        {
            return (_context.IssueStatuses?.Any(e => e.StatusId == id)).GetValueOrDefault();
        }
    }
}
