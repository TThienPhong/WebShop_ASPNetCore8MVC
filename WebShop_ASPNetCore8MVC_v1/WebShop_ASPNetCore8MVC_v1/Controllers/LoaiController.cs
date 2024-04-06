using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class LoaiController : Controller
	{
		private readonly ILoaiHangHoaService _loaiService;
        private readonly IMapper _mapper;

        public LoaiController(ILoaiHangHoaService loaiService,IMapper mapper) 
		{
			_loaiService = loaiService;
            _mapper = mapper;
        }

		[HttpGet]
		[Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
		public IActionResult Index(string? query)
		{
            IEnumerable<MenuLoaiVM> result = new List<MenuLoaiVM>();
            try
			{
                IEnumerable<LoaiModel> data= _loaiService.GetAll(query);
                result= data.Select(l => _mapper.Map<MenuLoaiVM>(l));
                TempData["loaiList"] = result;

                return View();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				ModelState.AddModelError("loi", "Không thể gọi danh sách loại ");
                
				return View();
			}			
		}

		[HttpGet]
		[Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
		public IActionResult Delete(int id)
		{
            //List<MenuLoaiVM> result = new List<MenuLoaiVM>();
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
                result = _mapper.Map<MenuLoaiVM>(_loaiService.GetById(id));
                   
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
                
                try
                {
                    var result = _mapper.Map<LoaiModel>(model);
                    _loaiService.Update(result);
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
                    var result = _mapper.Map<LoaiModel>(model);
                    _loaiService.Add(result);
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
