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
    public class OrdersController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public OrdersController(Computer_serviceContext context)
        {
            _context = context;
        }
        //GET: /api/Orders/filtered/Type1
        [HttpGet("filtered/{type}"),Authorize]
        public IActionResult FilteredOrders(string type)
        {
            if (_context.Orders == null) return NotFound();

            List<Order> filtered = new List<Order>();
            foreach (var ord in _context.Orders)
            {
                if (ord.Type == type) filtered.Add(ord);
            }

            return Ok(new
            {
                Count = filtered.Count,
                Orders = filtered
            });
        }

        [HttpGet("search/{user}"),Authorize]
        public IActionResult SearchByUser(string user, int page)
        {
            if (_context.Orders == null) return NotFound();

            List<Order> found = new List<Order>();
            foreach (var ord in _context.Orders)
            {
                if (ord.Client == user) found.Add(ord);
            }

            if (found.Count > 10)
            {
                List<Order> paged = new List<Order>();
                for (int i = page * 10; i < page * 10 + 10; i++)
                {
                    try
                    {
                        paged.Add(found[i]);
                    }
                    catch
                    {
                        break;
                    }
                }
                return Ok(paged);
            }
            return Ok(found);

        }

        // GET: api/Orders
        [HttpGet,Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int page)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            if (_context.Orders.Count() < 10)
                return await _context.Orders.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.Orders.Count()) return NoContent();
                List<Order> acs = new List<Order>();

                for (int i = (int)(page * 10); i < page * 10 + 10; i++)
                {
                    try
                    {
                        acs.Add(_context.Orders.ToArray()[i]);
                    }
                    catch { break; }
                }
                return acs;
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Order>> GetOrder(int? id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutOrder(int? id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Client")]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'Computer_serviceContext.Orders'  is null.");
          }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int? id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
