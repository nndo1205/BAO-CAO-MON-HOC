using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public string MaChiTietDonHang { get; set; }

        [Required]
        public string MaDonHang { get; set; }

        [Required]
        public string MaTaiLieu { get; set; }
        public int SoLuong { get; set; }

        [Precision(18, 2)]
        public decimal Gia { get; set; }

        [ForeignKey("MaDonHang")]
        public DonHang DonHang { get; set; }
        [ForeignKey("MaTaiLieu")]
        public TaiLieu TaiLieu { get; set; }
    }
}
