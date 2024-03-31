using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
	public class LoaiController : Controller
	{
		private readonly ILoaiHangHoaService _loaiService;

		public LoaiController(ILoaiHangHoaService loaiService) 
		{
			_loaiService = loaiService;
		}

		[HttpGet]
		[Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
		public IActionResult Index(string? query)
		{
			List<MenuLoaiVM> result = new List<MenuLoaiVM>();
			try
			{
                result = _loaiService.GetAll(query);
                TempData["loaiList"] = result;

                return View();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				ModelState.AddModelError("loi", "Không thể gọi danh sách loại ");
                
				return View(result);
			}			
		}

		[HttpGet]
		[Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
		public IActionResult Delete(int id)
		{
			

            List<MenuLoaiVM> result = new List<MenuLoaiVM>();
			try
			{
                int count = _loaiService.GetById(id).SoLuong;
                if (count == 0)
                {
                    _loaiService.Delete(id);
                    TempData["Message"] = $"Đã xoá loai có id:{id} ";
                    return RedirectToAction("Index");
				}
				else
				{
                    TempData["Message"] = $" loại có id: {id} vẫn còn có sản phẩm, không thể xoá ";
                    return RedirectToAction("Index"); 
                }
               
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				TempData["Message"] = $"Không thể xoá loại có id: {id} ";
				return RedirectToAction("Index");
			}
		}

        [HttpGet]
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        public IActionResult GetById(int id)
        {
            MenuLoaiVM result = new MenuLoaiVM();
            try
            {
                result=_loaiService.GetById(id);
                TempData["Message"] = $"{result.MaLoai}-{result.TenLoai} có {result.SoLuong} ";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Message"] = $"Không có loại có id: {id} ";
                return View();
            }
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        public IActionResult Update(MenuLoaiVM model)
        {
            if (ModelState.IsValid)
            {
                MenuLoaiVM result = model;
                try
                {
                    _loaiService.Update(model);
                    TempData["Message"] = $"Đã cập nhật Loại có id:{model.MaLoai} ";
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["Message"] = $"Không thể cập nhật Loại có id:{model.MaLoai} ";
                    return RedirectToAction("Index");
                }
            }
            TempData["Message"] = $"Loại có id:{model.MaLoai}, Dữ liệu cập nhật không phù hợp, ";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        public IActionResult Add(MenuLoaiVM model)
        {
            if (ModelState.IsValid)
            {
                 int count = _loaiService.GetAll(null).Where(l=>l.TenLoai== model.TenLoai).Count();


                if (count>0)
                {
                    TempData["Message"] = @$"Loại:'{model.TenLoai}'đã tồn tại ";
                    return RedirectToAction("Index");
                }                   

                try
                {
                    _loaiService.Add(model);
                    TempData["Message"] = $"Đã thêm Loại :'{model.TenLoai}'";
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["Message"] = $"Không thể thêm loại:'{model.TenLoai}'";
                    return RedirectToAction("Index");
                }
            }
            TempData["Message"] = $"Không thể thêm loại: '{model.TenLoai}'";
            return RedirectToAction("Index");
        }
    }
}
