using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;


namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public interface IChiTietHDService
    {

        IEnumerable<ChiTietHDModel> GetAll(string? query, int? MaDH);
        LoaiModel GetById(int maChiTiet);
        void Add(ChiTietHDModel chiTietHD);
        void Update(ChiTietHDModel chiTietHD);
        void Delete(int maChiTiet);
        void DeleteHD(int maHD);
    }
}
