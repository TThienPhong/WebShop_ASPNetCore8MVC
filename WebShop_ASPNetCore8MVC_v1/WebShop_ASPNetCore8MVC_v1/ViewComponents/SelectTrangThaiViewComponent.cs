using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.ViewComponents
{
    public class SelectTrangThaiViewComponent: ViewComponent
    {
        private readonly ITrangThaiService _trangThaiService;
        private readonly IMapper _mapper;

        public SelectTrangThaiViewComponent(ITrangThaiService trangThaiService,IMapper mapper )
        {
            _trangThaiService = trangThaiService;
            _mapper=mapper;
        }
        public IViewComponentResult Invoke(int? selectedValue = null)
        {
            /*var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();*/
            var trangThaiVM = _trangThaiService.GetAll().Select(l=>_mapper.Map<TrangThaiVM>(l));
            ViewBag.SelectedTrangThai = selectedValue;
            return View("SelectTrangThai", trangThaiVM);
        }
    }
}
