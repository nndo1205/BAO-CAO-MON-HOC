using Microsoft.AspNetCore.Mvc;
using BanSach.Models;
using BanSach.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BanSach.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly AppDbContext _context;

        public TaiKhoanController(AppDbContext context)
        {
            _context = context;
        }

        // ==== GET: Đăng nhập ====
        [HttpGet]
        public IActionResult DangNhap()
        {
            if (HttpContext.Session.GetString("MaNguoiDung") != null)
            {
                return RedirectToAction("Index", "TaiLieu");
            }
            return View();
        }

        // ==== POST: Đăng nhập ====
        [HttpPost]
        public async Task<IActionResult> DangNhap(NguoiDung model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.MatKhau))
            {
                ViewBag.ThongBao = "Vui lòng nhập đầy đủ thông tin.";
                return View(model);
            }

            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.Email.Trim() == model.Email.Trim() &&
                                          u.MatKhau.Trim() == model.MatKhau.Trim());

            if (nguoiDung == null)
            {
                ViewBag.ThongBao = "Email hoặc mật khẩu không đúng.";
                return View(model);
            }

            HttpContext.Session.SetString("MaNguoiDung", nguoiDung.MaNguoiDung);
            HttpContext.Session.SetString("HoTen", nguoiDung.HoTen);
            HttpContext.Session.SetString("VaiTro", nguoiDung.VaiTro);

            return RedirectToAction("Index", "TaiLieu");
        }


        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(NguoiDung model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra email trùng
            var existed = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Email.Trim() == model.Email.Trim());

            if (existed != null)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(model);
            }

            // Gán mã người dùng và vai trò
            model.MaNguoiDung = "ND" + DateTime.Now.ToString("yyyyMMddHHmmss");
            model.VaiTro = "KhachHang";

            try
            {
                _context.NguoiDungs.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("DangNhap");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi khi lưu dữ liệu: " + ex.Message);
                return View(model);
            }
        }

        // ==== Đăng xuất ====
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "TaiLieu");
        }
    }
}
