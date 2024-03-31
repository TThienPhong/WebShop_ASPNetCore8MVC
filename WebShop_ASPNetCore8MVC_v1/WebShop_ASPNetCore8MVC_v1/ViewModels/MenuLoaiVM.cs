using System.ComponentModel.DataAnnotations;

namespace WebShop_ASPNetCore8MVC_v1.ViewModels
{
    public class MenuLoaiVM
    {

        [Display(Name = "Mã Loại")]
        public int MaLoai { get; set; }

        [Display(Name = "Tên Loại")]
        [Required(ErrorMessage = "Chưa nhập tên")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string TenLoai { get; set; }
        public int SoLuong { get; set; }
    }
}
