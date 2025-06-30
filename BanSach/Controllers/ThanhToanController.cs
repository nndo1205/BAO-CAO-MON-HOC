using Microsoft.AspNetCore.Mvc;
using BanSach.ViewModels;
using BanSach.Models;
using BanSach.Data;
using BanSach.Helpers;

namespace BanSach.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly AppDbContext _context;

        public ThanhToanController(AppDbContext context)
        {
            _context = context;
        }

        // ===== GIAO DIỆN THANH TOÁN =====
        [HttpGet]
        public IActionResult Index(string[]? selectedIds, string? id)
        {
            var hoTen = HttpContext.Session.GetString("HoTen");
            var email = HttpContext.Session.GetString("Email");

            // ✅ 1. Nếu truyền id sách → thanh toán 1 sách trực tiếp
            if (!string.IsNullOrEmpty(id))
            {
                var sach = _context.TaiLieus.FirstOrDefault(t => t.MaTaiLieu == id);
                if (sach == null) return NotFound();

                var model = new ThanhToanViewModel
                {
                    HoTen = hoTen,
                    Email = email,
                    Sach = sach
                };
                return View(model);
            }

            // ✅ 2. Nếu truyền selectedIds → thanh toán các sản phẩm được chọn trong giỏ
            var gioHangGoc = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new();

            List<GioHangItem> gioHangChon = gioHangGoc;

            if (selectedIds != null && selectedIds.Any())
            {
                gioHangChon = gioHangGoc.Where(g => selectedIds.Contains(g.MaTaiLieu)).ToList();
            }

            if (gioHangChon == null || !gioHangChon.Any())
                return RedirectToAction("Index", "GioHang");

            var viewModel = new ThanhToanViewModel
            {
                HoTen = hoTen,
                Email = email,
                GioHang = gioHangChon
            };

            return View(viewModel);
        }

        // ===== XỬ LÝ ĐẶT HÀNG =====
        [HttpPost]
        public IActionResult DatHang(ThanhToanViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetString("MaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDung))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var gioHangSession = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new();
            List<GioHangItem> gioHang = gioHangSession;

            // Nếu không có giỏ hàng và có sách riêng → tạo đơn sách trực tiếp
            if ((gioHang == null || !gioHang.Any()) && model.Sach != null)
            {
                gioHang = new List<GioHangItem>
                {
                    new GioHangItem
                    {
                        MaTaiLieu = model.Sach.MaTaiLieu,
                        TenTaiLieu = model.Sach.TenTaiLieu,
                        GiaKhuyenMai = model.Sach.GiaKhuyenMai,
                        AnhBia = model.Sach.AnhBia,
                        SoLuong = 1
                    }
                };
            }

            if (gioHang == null || !gioHang.Any())
            {
                TempData["ThongBao"] = "Không có sản phẩm nào để thanh toán.";
                return RedirectToAction("Index", "GioHang");
            }

            var donHang = new DonHang
            {
                MaDonHang = Guid.NewGuid().ToString("N").Substring(0, 10),
                MaNguoiDung = maNguoiDung,
                NgayDat = DateTime.Now,
                TongTien = gioHang.Sum(x => x.SoLuong * x.GiaKhuyenMai),
                TrangThai = "Chờ xử lý"
            };
            _context.DonHangs.Add(donHang);

            foreach (var item in gioHang)
            {
                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaChiTietDonHang = Guid.NewGuid().ToString("N").Substring(0, 10),
                    MaDonHang = donHang.MaDonHang,
                    MaTaiLieu = item.MaTaiLieu,
                    SoLuong = item.SoLuong,
                    Gia = item.GiaKhuyenMai
                });
            }

            _context.SaveChanges();

            // ❗ Chỉ xoá các mặt hàng đã thanh toán khỏi session nếu không phải thanh toán từng sách
            if (model.Sach == null)
            {
                var gioHangConLai = gioHangSession
                    .Where(g => !gioHang.Any(p => p.MaTaiLieu == g.MaTaiLieu))
                    .ToList();

                HttpContext.Session.SetObject("GioHang", gioHangConLai);
            }

            return RedirectToAction("HoanTat");
        }

        public IActionResult HoanTat()
        {
            return View();
        }
    }
}
