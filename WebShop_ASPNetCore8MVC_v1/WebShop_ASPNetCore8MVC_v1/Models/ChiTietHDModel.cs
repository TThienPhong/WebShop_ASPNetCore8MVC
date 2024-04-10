using WebShop_ASPNetCore8MVC_v1.Data;

namespace WebShop_ASPNetCore8MVC_v1.Models
{
    public class ChiTietHDModel
    {
        public int MaCt { get; set; }

        public int MaHd { get; set; }

        public int MaHh { get; set; }

        public double DonGia { get; set; }

        public int SoLuong { get; set; }

        public double GiamGia { get; set; }
        public HangHoaModel HangHoa { get; set; } = null!;
    }
}
