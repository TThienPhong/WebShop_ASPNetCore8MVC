using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public interface IKhachHangService
    {
        IEnumerable<KhachHangModel> GetAll(bool?taiKhoang, string? query);
        
        KhachHangModel GetById(string MaKH);
        void Add(KhachHangModel khachHang);
        void Update(KhachHangModel khachHang);
        void Delete(string MaKH);
    }
}
