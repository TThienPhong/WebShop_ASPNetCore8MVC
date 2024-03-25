using System.ComponentModel.DataAnnotations;
using WebShop_ASPNetCore8MVC_v1.Data;

namespace WebShop_ASPNetCore8MVC_v1.ViewModels
{
    public class CheckoutVM
    {
        public bool GiongKhachHang { get; set; }


        [Display(Name = "Họ tên người nhận")]
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string? HoTen { get; set; }

        [Display(Name = "Địa chỉ người người nhận")]
        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        public string? DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        [MaxLength(24, ErrorMessage = "Tối đa 24 kí tự")]
        [RegularExpression(@"^(03[2-9]|05[6-9]|07[06-9]|08[1-689]|09[0-46-9])\d{7}$", ErrorMessage = "Chưa đúng định dạng di động Việt Nam")]
        [Required(ErrorMessage = "Vui lòng nhập số điện tho")]
        public string? DienThoai { get; set; }

        [Display(Name = "Ghi chú")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        public string? GhiChu { get; set; }

        public ICollection<CartItem> CartItems { get; set; }= new List<CartItem>();
    }
}
