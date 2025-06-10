using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TayinTalepAPI.Data;
using TayinTalepAPI.Models;

namespace TayinTalepAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.SicilNo == model.SicilNo);
            
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // Sadece ad ve soyad güncellenebilir
            user.Ad = model.Ad;
            user.Soyad = model.Soyad;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Profil başarıyla güncellendi." });
        }
    }
} 