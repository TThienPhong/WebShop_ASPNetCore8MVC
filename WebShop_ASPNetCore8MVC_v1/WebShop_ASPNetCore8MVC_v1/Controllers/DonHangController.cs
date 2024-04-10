using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class DonHangController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHoaDonService _donHangService;

        public DonHangController(IMapper mapper, IHoaDonService donHangService)
        {
            _mapper = mapper;
            _donHangService= donHangService;
        }
        /* public IActionResult Index()
         {
             return View();
         }*/


        [HttpGet]
        [Authorize]
        public IActionResult CustomerGetAll(int? maTrangThai)
        {
            string maKH = User.FindFirstValue("CustomerID") ?? "";
            if (string.IsNullOrEmpty(maKH))
            {
                ModelState.AddModelError("loi", "Đăng nhập để xem đơn hàn mình");
                return Redirect("/");

            }
            var donHangs = _donHangService.GetAll(null,maKH, maTrangThai,null, null).Select(p=>_mapper.Map<HoaDonVM>(p));
            /* if (donHangs.Count()==0)
             {
                 ModelState.AddModelError("loi", "Khách hàng không tồn tại trong hệ thống");
                 return Redirect("/");

             }*/

            TempData["DonHangList"] = donHangs;
            return View();
           
        }

    }
}
