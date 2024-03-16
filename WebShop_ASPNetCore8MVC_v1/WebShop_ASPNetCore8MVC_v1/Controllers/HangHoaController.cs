using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Data;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;

        public HangHoaController(Hshop2023Context _context) 
        {            
            db = _context;
        }
        
        public IActionResult Index(int? loai)
        {
            return View();
        }
    }
}
