using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
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
            /*var data = db.Loais.Select(lo => new MenuLoaiVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);*/

            var data = from loai in db.Loais
                       select new MenuLoaiVM
                       {
                           MaLoai = loai.MaLoai,
                           TenLoai = loai.TenLoai,
                           SoLuong = loai.HangHoas.Count
                       };

            return View(data); // Default.cshtml
                               //return View("Default", data);
        }
    }
}
