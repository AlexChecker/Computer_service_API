using Computer_service_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Computer_service_API.Controllers
{
    public class TokenRefreshController : ControllerBase
    {

        private readonly Computer_serviceContext _context;

        public TokenRefreshController(Computer_serviceContext context)
        {
            _context = context;
        }
        private string genJWTToken(string login, TokenType type, string role)
        {
            DateTime expireTime;

            if (type == TokenType.Access)
            {
                expireTime = DateTime.UtcNow.AddMinutes(30);
            }
            else
            {
                expireTime = DateTime.UtcNow.AddHours(1);
            }

            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My absolutely secret key"));
            var signCr = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("token_type", type.ToString())
                }),
                Expires = expireTime,
                SigningCredentials = signCr
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        [HttpGet("[action]/{login}"), Authorize(Roles = "Employee")]
        public async Task<IActionResult> refreshEmployeeToken(string login)
        {
            Employee emp = await _context.Employees.FirstOrDefaultAsync(p => (p.Login == login));
            if (emp == null) return StatusCode(418);
            return Ok(new { Access = genJWTToken(emp.Login,TokenType.Access,"Employee"), Refresh = genJWTToken(emp.Login,TokenType.Refresh,"Employee") });
        }

        [HttpGet("[action]/{login}"), Authorize(Roles = "Client")]
        public async Task<IActionResult> refreshClientToken(string login)
        {
            Client cl = await _context.Clients.FirstOrDefaultAsync(p => (p.Login == login));
            if (cl == null) return Problem("I'm a teapot!",statusCode:418, title: "I'm a teapot!");
            return Ok(new { Access = genJWTToken(cl.Login, TokenType.Access, "Client"), Refresh = genJWTToken(cl.Login, TokenType.Refresh, "Client") });
        }

    }
}
