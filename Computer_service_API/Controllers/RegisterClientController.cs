using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Computer_service_API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Computer_service_API.Controllers
{

    [Route("api/[controller]")]
    public class RegisterClientController : ControllerBase
    {
        private readonly Computer_serviceContext _context;
        //Я не буду модифицировать бд для уникальной соли, так что солью будет этот uuid
        private readonly string salt = "972db1d5-5b7f-43f6-ae66-a610e71c78af";

        public RegisterClientController(Computer_serviceContext context)
        {
            _context = context;
        }

        private string saltPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested:512/8
                ));

        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterClientModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request");
            }

            var client = await _context.Clients.FirstOrDefaultAsync(p => (p.Login == model.login));
            if (client != null)
            {
                return BadRequest("Invalid data");
            }

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My absolutely secret key"));
            var signCr = new SigningCredentials(secret, SecurityAlgorithms.HmacSha512);

            var options = new JwtSecurityToken(
                    issuer: model.login,
                    audience: "https://localhost:7253",
                    claims: new List<Claim>(),
                    expires: DateTime.MaxValue,
                    signingCredentials: signCr
                );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(options);
            var newcl = new Client();
            newcl.Deleted = false;
            newcl.Password = saltPassword(model.password);
            newcl.Login = model.login;
            newcl.Token = tokenStr;
            _context.Clients.Add(newcl);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
