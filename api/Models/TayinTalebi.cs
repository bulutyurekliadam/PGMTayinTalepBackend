using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayinTalepAPI.Models
{
    public class TayinTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string SicilNo { get; set; }

        [Required]
        [StringLength(100)]
        public required string TalepEdilenAdliye { get; set; }

        [Required]
        public DateTime BasvuruTarihi { get; set; }

        [Required]
        public required string Aciklama { get; set; }

        [Required]
        [StringLength(20)]
        public required string TalepDurumu { get; set; } = "Beklemede";

        public DateTime? DegerlendirilmeTarihi { get; set; }

        public string? DegerlendirmeNotu { get; set; }

        public bool IsOnaylandi { get; set; }

        [ForeignKey("SicilNo")]
        public User? User { get; set; }
    }
} 