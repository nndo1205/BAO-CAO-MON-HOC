using BanSach.Models;

namespace BanSach.ViewModels
{
    public class ThanhToanViewModel
    {
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string NganHang { get; set; }
        public string SoTaiKhoan { get; set; }
        public TaiLieu? Sach { get; set; }
        public List<GioHangItem>? GioHang { get; set; } /// Sách hiện tại nếu thanh toán nhanh

    }
}
