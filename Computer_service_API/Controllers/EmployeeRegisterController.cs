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
    public class EmployeeRegisterController:ControllerBase
    {
        private readonly Computer_serviceContext _context;
        //Я не буду модифицировать бд для уникальной соли, так что солью будет этот uuid
        private readonly string salt = "972db1d5-5b7f-43f6-ae66-a610e71c78af";

        public EmployeeRegisterController(Computer_serviceContext context)
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
                    new Claim(ClaimTypes.Role, "Employee"),
                    new Claim("token_type", type.ToString())
                }),
                Expires = expireTime,
                SigningCredentials = signCr
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        [HttpPost, Route("register")]

        public async Task<IActionResult> RegisterEmployee(EmployeeModel model)
        {
            var empl = await _context.Employees.FirstOrDefaultAsync(p => (p.Login == model.login));
            if (empl != null)
            {
                //Читай "Я - чайник"
                return StatusCode(418);//BadRequest("Employee exists!");
            }
            var dep = await _context.Departments.FirstOrDefaultAsync(p => (p.DepId == model.department));
            if (dep == null) return BadRequest("No such department");
            Employee employee = new Employee();
            employee.Login = model.login;
            employee.Password = saltPassword(model.password);
            employee.Department = model.department;
            employee.FirstName = model.first_name;
            employee.SecondName = model.last_name;
            employee.ServiceId = Guid.NewGuid().ToString();

            employee.Deleted = false;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return Ok();


        }

        [HttpPost, Route("login")]
        public async Task<ActionResult<string>> LogInEmployee(RegisterClientModel model)
        {
            if (model == null) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(p => (p.Login == model.login) && (p.Password == saltPassword(model.password)));
            if (employee == null) return BadRequest();
            return Ok(new { Access = genJWTToken(employee.Login,TokenType.Access), Refresh = genJWTToken(employee.Login,TokenType.Refresh)});
        }
    }
}
