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
    public class ReviewsController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public ReviewsController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int page)
        {
          if (_context.Reviews == null)
          {
              return NotFound();
          }
            if (_context.Reviews.Count() < 10)
                return await _context.Reviews.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Reviews.Count()) return NoContent();
                List<Review> acs = new List<Review>();

                for (int i = (int)(page * 10); i < _context.Reviews.Count(); i++)
                {
                    acs.Add(_context.Reviews.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/Reviews/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Review>> GetReview(int? id)
        {
          if (_context.Reviews == null)
          {
              return NotFound();
          }
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutReview(int? id, Review review)
        {
            if (id != review.RevId)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
          if (_context.Reviews == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Reviews'  is null.");
          }
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.RevId }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteReview(int? id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int? id)
        {
            return (_context.Reviews?.Any(e => e.RevId == id)).GetValueOrDefault();
        }
    }
}
