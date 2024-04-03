using System.ComponentModel.DataAnnotations;
using WebShop_ASPNetCore8MVC_v1.Models;

namespace WebShop_ASPNetCore8MVC_v1.ViewModels
{
    public class HangHoaVM_admin
    {
        [Display(Name = "Mã hàng hoá")]
        public int MaHh { get; set; }
        [Display(Name = "Tên hàng hoá")]
        [Required(ErrorMessage = "Vui lòng nhập tên hàng hoá")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string TenHH { get; set; }

        [Display(Name = "Hình")]
        public string? Hinh { get; set; }
        [Display(Name = "Đơn giá")] // Hiển thị tên "Đơn giá" thay vì tên của thuộc tính
        [Required(ErrorMessage = "Vui lòng nhập đơn giá")] // Yêu cầu phải nhập giá trị cho đơn giá
        [Range(0, float.MaxValue, ErrorMessage = "Đơn giá phải là số không âm")] // Đơn giá phải là số không âm
        public double DonGia { get; set; }

        [Display(Name = "Mô tả ngắn")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string? MoTaNgan { get; set; }

        [Display(Name = "Loại")] 
        [Required(ErrorMessage = "Vui lòng chọn loại")]  
        public int MaLoai { get; set; }
        public string? TenLoai { get; set; }

        [Display(Name = "Chi tiết hàng hoá")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")]
        public string? ChiTiet { get; set; }
        public int DiemDanhGia { get; set; } = 5;// Chưa xử lý
        public int SoLuongTon { get; set; } = 100;// Chưa xử lý
        public DateTime NgaySx { get; set; } =DateTime.Now.Date;// Chưa xử lý
        public double GiamGia { get; set; } = 0;// Chưa xử lý
        public int SoLanXem { get; set; } = 15;// Chưa xử lý
        public string MaNcc { get; set; } = "KK";//// Chưa xử lý


    }
}
