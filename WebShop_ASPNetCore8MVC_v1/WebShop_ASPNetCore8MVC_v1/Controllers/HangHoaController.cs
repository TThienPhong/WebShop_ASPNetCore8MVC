using Microsoft.AspNetCore.Mvc;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class HangHoaController : Controller
    {
        
        public IActionResult Index(int? loai)
        {
            return View();
        }
    }
}
