using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TayinTalepAPI.Data;
using TayinTalepAPI.Models;

namespace TayinTalepAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _jwtKey = "your-256-bit-secret-key-here-minimum-16-characters";

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.SicilNo == user.SicilNo))
            {
                return BadRequest("Bu sicil numarası zaten kayıtlı.");
            }

            // Şifreyi hashle
            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Token oluştur
            var token = GenerateJwtToken(user);

            // Kullanıcı bilgilerini döndür (şifre hariç)
            var userResponse = new
            {
                user.Id,
                user.SicilNo,
                user.Ad,
                user.Soyad,
                user.Unvan,
                user.MevcutAdliye,
                user.IseBaslamaTarihi,
                user.IsAdmin
            };

            return Ok(new { token, user = userResponse });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.SicilNo == model.SicilNo);

            if (user == null || !VerifyPassword(model.Password, user.Password))
            {
                return Unauthorized("Geçersiz sicil numarası veya şifre.");
            }

            // Token oluştur
            var token = GenerateJwtToken(user);

            // Kullanıcı bilgilerini döndür (şifre hariç)
            var userResponse = new
            {
                user.Id,
                user.SicilNo,
                user.Ad,
                user.Soyad,
                user.Unvan,
                user.MevcutAdliye,
                user.IseBaslamaTarihi,
                user.IsAdmin
            };

            return Ok(new { token, user = userResponse });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.SicilNo),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }

    public class LoginModel
    {
        public required string SicilNo { get; set; }
        public required string Password { get; set; }
    }
} 