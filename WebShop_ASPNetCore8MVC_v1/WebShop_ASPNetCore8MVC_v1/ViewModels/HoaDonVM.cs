using WebShop_ASPNetCore8MVC_v1.Models;

namespace WebShop_ASPNetCore8MVC_v1.ViewModels
{
    public class HoaDonVM
    {
        public int MaHd { get; set; }

        public string MaKh { get; set; } = null!;

        public DateTime NgayDat { get; set; }

        public DateTime? NgayCan { get; set; }

        public DateTime? NgayGiao { get; set; }

        public string? HoTen { get; set; }

        public string DiaChi { get; set; } = null!;

        public string CachThanhToan { get; set; } = null!;

        public string CachVanChuyen { get; set; } = null!;

        public double PhiVanChuyen { get; set; }

        public int MaTrangThai { get; set; }

        //public string? MaNv { get; set; }

        public string? GhiChu { get; set; }

        public string? DienThoai { get; set; }

        public ICollection<ChiTietHDVM> ChiTietHds { get; set; } = new List<ChiTietHDVM>();

        public KhachHangVM KhachHang { get; set; } = null!;
        public TrangThaiVM TrangThai { get; set; } = null!;
    }
}
