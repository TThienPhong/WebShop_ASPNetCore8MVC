using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public interface IHangHoaService
    {
        IEnumerable<HangHoaModel> GetAll(int? loai, string? query);//Admin xem
        //IEnumerable<HangHoaModel> GetAllProduc(int? loai, string? query);//Khách hàng xem
        HangHoaModel GetById(int id);
        void Add(HangHoaModel product);
        void Update(HangHoaModel product);
        void Delete(int productId);
    }
}
