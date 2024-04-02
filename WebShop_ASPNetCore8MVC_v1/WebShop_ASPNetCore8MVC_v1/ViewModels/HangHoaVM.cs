using System.ComponentModel.DataAnnotations;

namespace WebShop_ASPNetCore8MVC_v1.ViewModels
{
    public class HangHoaVM
    {
        public int MaHH { get; set; }
        public string TenHH { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public string TenLoai { get; set; }


    }
    public class ChiTietHangHoaVM
    {
        
        public int MaHh { get; set; }

       
        public string TenHH { get; set; }

       
        public string Hinh { get; set; }
       
        public double DonGia { get; set; }

        public string MoTaNgan { get; set; }

        [Display(Name = "TenLoai")]
        [Required(ErrorMessage = "Vui lòng chọn tên loại")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string TenLoai { get; set; }
        
        [Display(Name = "TenLoai")]
        [Required(ErrorMessage = "Vui lòng chọn tên loại")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string ChiTiet { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }
    }
}
