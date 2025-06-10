using System.ComponentModel.DataAnnotations;

namespace TayinTalepAPI.Models
{
    public class UpdateProfileModel
    {
        [Required(ErrorMessage = "Sicil numarası zorunludur.")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Sicil numarası 7 karakter olmalıdır.")]
        public string SicilNo { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
        public string Soyad { get; set; }
    }
} 