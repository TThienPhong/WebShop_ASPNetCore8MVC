using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class KhachHangController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DangKy()
        {

            return View();
        }

		[HttpPost]
		public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
		{

			return View();
		}
	}
}
