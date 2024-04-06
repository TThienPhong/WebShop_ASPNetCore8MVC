﻿namespace WebShop_ASPNetCore8MVC_v1.Models
{
    public class KhachHangModel
    {
        public string MaKh { get; set; } = null!;

        public string? MatKhau { get; set; }

        public string HoTen { get; set; } = null!;

        public bool GioiTinh { get; set; }

        public DateTime NgaySinh { get; set; }

        public string? DiaChi { get; set; }

        public string? DienThoai { get; set; }

        public string Email { get; set; } = null!;

        public string? Hinh { get; set; }

        public bool HieuLuc { get; set; }

        public int VaiTro { get; set; }

        public string? RandomKey { get; set; }
    }
}
