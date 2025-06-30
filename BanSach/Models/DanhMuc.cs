using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
    public class DanhMuc
    {
        [Key]
        public string MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; }

        // Một danh mục có nhiều sách
        public ICollection<TaiLieu> Tailieu { get; set; }
    }
}
