using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
	public interface IHangHoaService
	{
		List<HangHoaVM> GetAll(int? loai, string? query);
        HangHoaVM GetById(int id);
        void AddProduct(HangHoaVM product);
		void UpdateProduct(HangHoaVM product);
		void DeleteProduct(int productId);
	}
}
