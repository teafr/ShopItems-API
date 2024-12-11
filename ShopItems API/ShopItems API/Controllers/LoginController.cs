using ShopItems_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopItems_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly List<User> _users = new List<User>()
        {
            new User() { UserName = "Someone", Password = "passw", Role = "User" },
            new User() { UserName = "Honcharova", Password = "adminPassw", Role = "Admin" }
        };

        public LoginController(IConfiguration configuration) { _configuration = configuration; }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] User userFromBody)
        {
            var user = Authenticate(userFromBody);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.UserName), new Claim(ClaimTypes.Role, user.Role) };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"], 
                _configuration["Jwt:Audience"], 
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(User userFromBody)
        {            
            return _users.FirstOrDefault(u => u.UserName.ToLower() == userFromBody.UserName.ToLower() && u.Password == userFromBody.Password)!;
        }
    }
}
