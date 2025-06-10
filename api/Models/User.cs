using System;
using System.ComponentModel.DataAnnotations;

namespace TayinTalepAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public required string SicilNo { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        [StringLength(50)]
        public required string Ad { get; set; }

        [Required]
        [StringLength(50)]
        public required string Soyad { get; set; }

        [Required]
        [StringLength(100)]
        public required string Unvan { get; set; }

        [Required]
        [StringLength(100)]
        public required string MevcutAdliye { get; set; }

        [Required]
        public DateTime IseBaslamaTarihi { get; set; }

        public bool IsAdmin { get; set; }
    }
} 