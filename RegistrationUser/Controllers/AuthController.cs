using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RegistrationUser.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace RegistrationUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        public AuthController(IConfiguration Configuration)
        {
            _config = Configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody]User user)
        {
            bool resultado = UserValidate(user);
            if (resultado)
            {
                var tokenString = GeneratingTokenJWT();
                return Ok(new { token = tokenString });
            }
            else
            {
                return BadRequest(new ValidationResult($"Usuário: {user.UserName} não autorizado"));
            }
        }

        private string GeneratingTokenJWT()
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        private bool UserValidate(User user)
        {
            if (user.UserName == "Teste" && user.Password == "1323")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}