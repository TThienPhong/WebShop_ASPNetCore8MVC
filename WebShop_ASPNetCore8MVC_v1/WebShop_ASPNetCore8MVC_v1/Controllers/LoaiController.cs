using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
	public class LoaiController : Controller
	{
		private readonly ILoaiHangHoaService _loaiService;

		public LoaiController(ILoaiHangHoaService loaiService) 
		{
			_loaiService = loaiService;
		}
		public IActionResult Index()
		{
			List<MenuLoaiVM> result = new List<MenuLoaiVM>();
			try
			{
                result = _loaiService.GetAll();

				return View(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				ModelState.AddModelError("loi", "Không thể gọi danh sách loại ");
				return View(result);
			}

			
		}
	}
}
