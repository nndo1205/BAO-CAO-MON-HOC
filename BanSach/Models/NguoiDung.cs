using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
    public class NguoiDung
    {
        [Key]
        [Column(TypeName = "nchar(10)")]
        public string MaNguoiDung { get; set; }

        [Required]
        [MaxLength(255)]
        public string HoTen { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string MatKhau { get; set; }

        [MaxLength(100)]
        public string VaiTro { get; set; }  // Mặc định là khách hàng

        public ICollection<DonHang> DonHang { get; set; }
    }
}
