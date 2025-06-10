using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TayinTalepAPI.Data;
using TayinTalepAPI.Models;

namespace TayinTalepAPI.Controllers
{
    [Route("api/tayin")]
    [ApiController]
    public class TayinController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public class TayinTalebiRequest
        {
            public required string TalepEdilenAdliye { get; set; }
            public required string TalepTuru { get; set; }
            public required string Aciklama { get; set; }
        }

        public TayinController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTayinTalebi([FromBody] TayinTalebiRequest request)
        {
            var sicilNo = User.Identity?.Name;
            if (string.IsNullOrEmpty(sicilNo))
            {
                return Unauthorized();
            }

            var tayinTalebi = new TayinTalebi
            {
                SicilNo = sicilNo,
                TalepEdilenAdliye = request.TalepEdilenAdliye,
                TalepTuru = request.TalepTuru,
                Aciklama = request.Aciklama,
                BasvuruTarihi = DateTime.Now,
                TalepDurumu = "Beklemede"
            };

            _context.TayinTalepleri.Add(tayinTalebi);
            await _context.SaveChangesAsync();

            return Ok(tayinTalebi);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyTayinTalepleri()
        {
            var sicilNo = User.Identity?.Name;
            if (string.IsNullOrEmpty(sicilNo))
            {
                return Unauthorized();
            }

            var talepler = await _context.TayinTalepleri
                .Where(t => t.SicilNo == sicilNo)
                .OrderByDescending(t => t.BasvuruTarihi)
                .Select(t => new
                {
                    t.Id,
                    t.TalepEdilenAdliye,
                    t.TalepTuru,
                    t.BasvuruTarihi,
                    t.Aciklama,
                    t.TalepDurumu,
                    t.DegerlendirilmeTarihi,
                    t.DegerlendirmeNotu,
                    t.IsOnaylandi
                })
                .ToListAsync();

            return Ok(talepler);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTayinTalepleri()
        {
            var sicilNo = User.Identity?.Name;
            if (string.IsNullOrEmpty(sicilNo))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.SicilNo == sicilNo);
            if (user == null || !user.IsAdmin)
            {
                return Forbid();
            }

            var talepler = await _context.TayinTalepleri
                .Include(t => t.User)
                .OrderByDescending(t => t.BasvuruTarihi)
                .Select(t => new
                {
                    t.Id,
                    t.SicilNo,
                    t.TalepEdilenAdliye,
                    t.TalepTuru,
                    t.BasvuruTarihi,
                    t.Aciklama,
                    t.TalepDurumu,
                    t.DegerlendirilmeTarihi,
                    t.DegerlendirmeNotu,
                    t.IsOnaylandi,
                    Personel = new
                    {
                        t.User.Ad,
                        t.User.Soyad,
                        t.User.Unvan,
                        t.User.MevcutAdliye
                    }
                })
                .ToListAsync();

            return Ok(talepler);
        }

        [HttpPut("{id}/degerlendirme")]
        public async Task<IActionResult> DegerlendirTayinTalebi(int id, [FromBody] DegerlendirmeRequest request)
        {
            var sicilNo = User.Identity?.Name;
            if (string.IsNullOrEmpty(sicilNo))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.SicilNo == sicilNo);
            if (user == null || !user.IsAdmin)
            {
                return Forbid();
            }

            var tayinTalebi = await _context.TayinTalepleri.FindAsync(id);
            if (tayinTalebi == null)
            {
                return NotFound();
            }

            tayinTalebi.TalepDurumu = "Değerlendirildi";
            tayinTalebi.DegerlendirilmeTarihi = DateTime.Now;
            tayinTalebi.DegerlendirmeNotu = request.DegerlendirmeNotu;
            tayinTalebi.IsOnaylandi = request.IsOnaylandi;

            await _context.SaveChangesAsync();

            return Ok(tayinTalebi);
        }
    }

    public class DegerlendirmeRequest
    {
        public required string DegerlendirmeNotu { get; set; }
        public bool IsOnaylandi { get; set; }
    }
} 