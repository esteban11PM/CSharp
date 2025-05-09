using Entity.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _context     = context;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _context.User
                .Include(u => u.RolUsers)
                .ThenInclude(ru => ru.Role)
                .FirstOrDefaultAsync(u => u.Username == login.Username && u.Password == login.Password && u.State);

            if (user == null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var rol = user.RolUsers.FirstOrDefault()?.Role?.Name ?? "User";

            var token = GenerateJwtToken(user.Username);

            return Ok(new
            {
                token,
                username = user.Username,
                userId = user.Id,
                rol
            });
        }

        private string GenerateJwtToken(string username)
        {
            var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
            };

            var token = new JwtSecurityToken(
                issuer:    _configuration["Jwt:Issuer"],
                audience:  _configuration["Jwt:Audience"],
                claims:    claims,
                expires:   DateTime.UtcNow.AddMinutes(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // DTO para recibir login
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
