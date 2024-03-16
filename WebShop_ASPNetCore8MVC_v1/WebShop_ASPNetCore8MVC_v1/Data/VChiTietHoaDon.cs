using System;
using System.Collections.Generic;

namespace WebShop_ASPNetCore8MVC_v1.Data;

public partial class VChiTietHoaDon
{
    public int MaCt { get; set; }

    public int MaHd { get; set; }

    public int MaHh { get; set; }

    public double DonGia { get; set; }

    public int SoLuong { get; set; }

    public double GiamGia { get; set; }

    public string TenHh { get; set; } = null!;
}
