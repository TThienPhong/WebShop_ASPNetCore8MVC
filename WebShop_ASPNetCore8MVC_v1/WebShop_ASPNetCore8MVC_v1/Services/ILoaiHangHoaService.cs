using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
	public interface ILoaiHangHoaService
	{
		
		List<MenuLoaiVM> GetAll();
		MenuLoaiVM GetById(int loaiHHId);
		void Add(MenuLoaiVM loaiHH);
		void Update(MenuLoaiVM loaiHH);
		void Delete(int loaiHHId);
	}
}
