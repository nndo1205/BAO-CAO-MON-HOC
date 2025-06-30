namespace BanSach.Models
{
    public class GioHangItem
    {
        public string MaTaiLieu { get; set; }
        public string TenTaiLieu { get; set; }
        public string AnhBia { get; set; }
        public decimal GiaKhuyenMai { get; set; }
        public int SoLuong { get; set; }

        public decimal ThanhTien => SoLuong * GiaKhuyenMai;
    }
}
