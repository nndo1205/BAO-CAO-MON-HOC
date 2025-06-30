using Microsoft.AspNetCore.Mvc;
using BanSach.Models;
using BanSach.Data;
using BanSach.ViewModels;

namespace BanSach.Controllers
{
    public class TaiLieuController : Controller
    {
        private readonly AppDbContext _context;

        public TaiLieuController(AppDbContext context)
        {
            _context = context;
        }

        // Trang danh sách
        public IActionResult Index()
        {
            var vm = new TaiLieuViewModel
            {
                SanPhamChinh = _context.TaiLieus
            .Where(x => x.MaDanhMuc != "DMGY")  // Bỏ tài liệu thuộc danh mục Gợi ý
            .Take(9)
            .ToList(),
                GoiY = _context.TaiLieus.Where(x => x.MaDanhMuc == "DMGY").Take(6).ToList()
            };

            return View(vm);
        }
        public IActionResult TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return RedirectToAction("Index");

            var ketQua = _context.TaiLieus
                .Where(t => t.TenTaiLieu.Contains(tuKhoa))
                .ToList();

            ViewBag.TuKhoa = tuKhoa;
            return View("KetQuaTimKiem", ketQua);
        }

        // Trang chi tiết sách
        public IActionResult ChiTiet(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var taiLieu = _context.TaiLieus.FirstOrDefault(x => x.MaTaiLieu == id);
            if (taiLieu == null) return NotFound();

            return View(taiLieu);
        }
    }
}
