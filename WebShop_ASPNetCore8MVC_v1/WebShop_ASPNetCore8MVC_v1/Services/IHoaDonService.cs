using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public interface IHoaDonService
    {
        IEnumerable<HoaDonModel> GetAll(int? maHD, string? maKH, int? maTrangThai, DateTime? from, DateTime? to);
        //IEnumerable<HangHoaModel> GetAllProduc(int? loai, string? query);//Khách hàng xem
        HoaDonModel GetById(int maHD);
        void Add(HoaDonModel hoaDon);
        void Update(HoaDonModel hoaDon);
        void Delete(int maHD);
    }
}
