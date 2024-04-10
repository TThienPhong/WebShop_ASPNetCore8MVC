using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;


namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public interface ITrangThaiService
    {

        IEnumerable<TrangThaiModel> GetAll();
        TrangThaiModel GetById(int maTrangThai);
        void Add(TrangThaiModel trangThai);
        void Update(TrangThaiModel trangThai);
        void Delete(int maTrangThai);
    }
}
