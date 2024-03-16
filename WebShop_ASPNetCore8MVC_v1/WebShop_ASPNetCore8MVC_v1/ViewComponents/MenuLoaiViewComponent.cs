using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;

        public MenuLoaiViewComponent(Hshop2023Context context) {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuLoaiVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);

            return View(data); // Default.cshtml
                               //return View("Default", data);
        }
    }
}
