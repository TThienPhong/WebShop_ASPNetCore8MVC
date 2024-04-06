using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;


namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public interface ILoaiHangHoaService
    {

        IEnumerable<LoaiModel> GetAll(string? query);
        LoaiModel GetById(int loaiHHId);
        void Add(LoaiModel loaiHH);
        void Update(LoaiModel loaiHH);
        void Delete(int loaiHHId);
    }
}
