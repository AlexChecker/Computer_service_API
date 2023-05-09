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
    public class IssuesController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public IssuesController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Issues
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues(int page)
        {
          if (_context.Issues == null)
          {
              return NotFound();
          }
            if (_context.Issues.Count() < 10)
                return await _context.Issues.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Issues.Count()) return NoContent();
                List<Issue> acs = new List<Issue>();

                for (int i = (int)(page * 10); i < _context.Issues.Count(); i++)
                {
                    acs.Add(_context.Issues.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/Issues/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Issue>> GetIssue(int? id)
        {
          if (_context.Issues == null)
          {
              return NotFound();
          }
            var issue = await _context.Issues.FindAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            return issue;
        }

        // PUT: api/Issues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutIssue(int? id, Issue issue)
        {
            if (id != issue.IssId)
            {
                return BadRequest();
            }

            _context.Entry(issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(id))
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

        // POST: api/Issues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
          if (_context.Issues == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Issues'  is null.");
          }
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssue", new { id = issue.IssId }, issue);
        }

        // DELETE: api/Issues/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteIssue(int? id)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssueExists(int? id)
        {
            return (_context.Issues?.Any(e => e.IssId == id)).GetValueOrDefault();
        }
    }
}
