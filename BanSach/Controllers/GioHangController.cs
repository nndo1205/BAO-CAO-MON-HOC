using BanSach.Models;
using Microsoft.AspNetCore.Mvc;
using BanSach.Data;
using BanSach.Helpers;

namespace BanSach.Controllers
{
    public class GioHangController : Controller
    {
        private readonly AppDbContext _context;

        public GioHangController(AppDbContext context)
        {
            _context = context;
        }

        public List<GioHangItem> LayGioHang()
        {
            var gio = HttpContext.Session.GetObject<List<GioHangItem>>("giohang");
            if (gio == null)
            {
                gio = new List<GioHangItem>();
            }
            return gio;
        }

        public IActionResult Index()
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new List<GioHangItem>();
            return View(gioHang);
        }

        public IActionResult ThemVaoGio(string id)
        {
            var sach = _context.TaiLieus.FirstOrDefault(x => x.MaTaiLieu == id);
            if (sach == null) return NotFound();

            var gioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new List<GioHangItem>();

            var sp = gioHang.FirstOrDefault(x => x.MaTaiLieu == id);
            if (sp != null)
                sp.SoLuong++;
            else
                gioHang.Add(new GioHangItem
                {
                    MaTaiLieu = sach.MaTaiLieu,
                    TenTaiLieu = sach.TenTaiLieu,
                    GiaKhuyenMai = sach.GiaKhuyenMai,
                    AnhBia = sach.AnhBia,
                    SoLuong = 1
                });

            HttpContext.Session.SetObject("GioHang", gioHang);
            TempData["ThongBao"] = "Đã thêm vào giỏ hàng.";
            return RedirectToAction("ChiTiet", "TaiLieu", new { id = id });
        }


        public IActionResult Xoa(string maTaiLieu)
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new List<GioHangItem>();
            gioHang.RemoveAll(x => x.MaTaiLieu == maTaiLieu);
            HttpContext.Session.SetObject("GioHang", gioHang);
            return RedirectToAction("Index");
        }

        public IActionResult CapNhatSoLuong(string maTaiLieu, string action)
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new List<GioHangItem>();
            var item = gioHang.FirstOrDefault(x => x.MaTaiLieu == maTaiLieu);

            if (item != null)
            {
                if (action == "tang")
                    item.SoLuong++;
                else if (action == "giam" && item.SoLuong > 1)
                    item.SoLuong--;
            }

            HttpContext.Session.SetObject("GioHang", gioHang);
            return RedirectToAction("Index");
        }
    }
}
