using System.ComponentModel.DataAnnotations;

namespace WebShop_ASPNetCore8MVC_v1.Models
{
    public class LoaiModel
    {
       
        public int MaLoai { get; set; } 

        public string TenLoai { get; set; } = null!;
        public int SoLuong { get; set; }
    }
}
