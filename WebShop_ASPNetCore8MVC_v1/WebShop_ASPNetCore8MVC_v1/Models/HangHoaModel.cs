namespace WebShop_ASPNetCore8MVC_v1.Models
{
    public class HangHoaModel
    {
        public int MaHh { get; set; } 
        public string TenHH { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public LoaiModel LoaiHH { get; set; } = null!;
        public string ChiTiet { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }

        public DateTime NgaySx { get; set; }

        public double GiamGia { get; set; }

        public int SoLanXem { get; set; }

        public string MaNcc { get; set; } = null!;


    }
}
