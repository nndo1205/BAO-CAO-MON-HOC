using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
    public class DonHang
    {
        [Key]
        public string MaDonHang { get; set; }
        public DateTime NgayDat { get; set; }

        [Precision(18, 2)]
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }

        public string MaNguoiDung { get; set; }

        [ForeignKey("MaNguoiDung")]
        public NguoiDung NguoiDung { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
