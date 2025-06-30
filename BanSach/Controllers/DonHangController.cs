using Microsoft.AspNetCore.Mvc;
using BanSach.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BanSach.Controllers
{
    public class DonHangController : Controller
    {
        private readonly AppDbContext _context;

        public DonHangController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DanhSach()
        {
            var maNguoiDung = HttpContext.Session.GetString("MaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDung))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var donHangs = await _context.DonHangs
                .Where(d => d.MaNguoiDung == maNguoiDung)
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.TaiLieu)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            return View(donHangs);
        }
    }
}
