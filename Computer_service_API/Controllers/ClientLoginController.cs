using Computer_service_API.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Computer_service_API.Controllers
{

    [Route("api/[controller]")]
    public class ClientLoginController:ControllerBase
    {

        private readonly Computer_serviceContext _context;
        //Я не буду модифицировать бд для уникальной соли, так что солью будет этот uuid
        private readonly string salt = "972db1d5-5b7f-43f6-ae66-a610e71c78af";

        public ClientLoginController(Computer_serviceContext context)
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
                numBytesRequested: 512 / 8
                ));

        }

        private string genJWTToken(string login, TokenType type)
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
                    new Claim(ClaimTypes.Role, "Client"),
                    new Claim(ClaimTypes.UserData, type.ToString())
                }),
                Expires = expireTime,
                SigningCredentials = signCr
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        [HttpGet, Route("login")]
        public async Task<ActionResult<string>> Login(RegisterClientModel model) 
        {
            if (model == null)
            {
                return BadRequest("Invalid request");
            }

            Client client = await _context.Clients.FirstOrDefaultAsync(p => (p.Login == model.login)&&(saltPassword(model.password)==p.Password));
            if (client == null)
            {
                return BadRequest("Invalid data");
            }
            return Ok(new { Access = genJWTToken(client.Login,TokenType.Access), Refresh = genJWTToken(client.Login,TokenType.Refresh) });
        }
    }
}
