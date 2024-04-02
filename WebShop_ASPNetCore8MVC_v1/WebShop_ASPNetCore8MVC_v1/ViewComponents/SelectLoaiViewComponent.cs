using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.ViewComponents
{
    public class SelectLoaiViewComponent: ViewComponent
    {
        private readonly ILoaiHangHoaService _loaiHangHoaService;
        private readonly IMapper _mapper;

        public SelectLoaiViewComponent(ILoaiHangHoaService loaiHangHoaService,IMapper mapper )
        {
            _loaiHangHoaService=loaiHangHoaService;
            _mapper=mapper;
        }
        public IViewComponentResult Invoke()
        {
            /*var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();*/
            var loaiVM = _loaiHangHoaService.GetAll(null).Select(l=>_mapper.Map<MenuLoaiVM>(l));
            return View("SelectLoai", loaiVM);
        }
    }
}
