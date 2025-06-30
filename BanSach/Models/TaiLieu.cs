using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanSach.Models
{
    [Table("TaiLieu")]
    public class TaiLieu
    {
        [Key]
        public string MaTaiLieu { get; set; }
        public string TenTaiLieu { get; set; }

        [Column("GiaSoc")]
        public decimal GiaSoc { get; set; }
        public decimal GiaKhuyenMai { get; set; }
        public int LuotBan { get; set; }
        public string AnhBia { get; set; }
        public string MoTa { get; set; }
        public DateTime NgayTao { get; set; }

        public string MaDanhMuc { get; set; }

        [ForeignKey("MaDanhMuc")]
        public DanhMuc DanhMuc { get; set; }
    }
}
