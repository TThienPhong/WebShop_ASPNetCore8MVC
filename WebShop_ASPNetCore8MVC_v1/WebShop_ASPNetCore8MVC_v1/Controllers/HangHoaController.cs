using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Helpers;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly IHangHoaService _hangHoaService;
        private readonly Hshop2023Context db;
        private readonly ILoaiHangHoaService _loaiHangHoaService;
        private readonly IMapper _mapper;

        public HangHoaController(Hshop2023Context _context,IHangHoaService hangHoaService,ILoaiHangHoaService loaiHangHoaService,IMapper mapper) 
        {
            _hangHoaService=hangHoaService;
            db = _context;
            _loaiHangHoaService = loaiHangHoaService;
            _mapper = mapper;
            
        }
		#region HangHoa_KhachHang

		public IActionResult Index(int? loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();

          

            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHH = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);

        }
        
        public IActionResult Search (string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (!String.IsNullOrEmpty(query) )
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHH = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }

            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,//tính sau
                DiemDanhGia = 5,//check sau
            };
            return View(result);
        }
        #endregion

        #region HangHoa_Admin

        [HttpGet]
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        public IActionResult AdminGet(int? loai, string? query, HangHoaVM_admin? model)
        {

            try
            {
                IEnumerable<HangHoaModel> data = _hangHoaService.GetAll(loai,query);
                var result = data.Select(p => _mapper.Map<HangHoaVM_admin>(p));
                //var hangHoaMV = ViewBag.hangHoaMV as HangHoaVM_admin ;
                ViewBag.isAddError = TempData["isAddError"] as bool? ?? false;
                ViewBag.isUpdateError = TempData["isUpdateError"] as bool? ?? false;
                ViewBag.hangHoaMV = model;
                TempData["hangHoaList"] = result;               
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("loi", "Không thể gọi danh sách HangHoa ");
                TempData["Message"] = $"Không tể gọi danh sách hàng hoá ";
                return View();
            }

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        public IActionResult AdminUpdate(HangHoaVM_admin model, IFormFile? Hinh)
        {
            if (ModelState.IsValid)
            {
                var hh = _hangHoaService.GetById(model.MaHh);
                if (hh == null)
                {
                    TempData["Message"] = $"không tồn tại Hàng hoá có id:{model.MaHh}";
                    return RedirectToAction("AdminGet");
                }
               
                try
                {
                   
                    var result = _mapper.Map<HangHoaModel>(model);

                    
                    if (Hinh != null)
                    {
                        MyUtil.DeleteHinh(hh.Hinh, "HangHoa");
                        result.Hinh = MyUtil.UploadHinh(Hinh, "HangHoa");

                    }
                    else
                    {
                        result.Hinh = hh.Hinh;
                    }
                  
                 
                    _hangHoaService.Update(result);
                    TempData["Message"] = $"Đã cập nhật Product có id:{model.MaHh} ";
                    return RedirectToAction("AdminGet");


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["Message"] = $"Không thể cập nhật Hàng Hoá có id:{model.MaHh} ";             
                    return RedirectToAction("AdminGet");
                }
            }
            TempData["Message"] = $"Hàng Hoá có id:{model.MaHh}, Dữ liệu cập nhật không phù hợp, ";
            TempData["isUpdateError"] =true ;
            return RedirectToAction("AdminGet", model);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        public IActionResult AdminAdd(HangHoaVM_admin model, IFormFile Hinh)
        {

            if (Hinh == null)
            {
                TempData["Message"] = @$"Không thể thêm Hang Hoa:'{model.TenLoai}'-Bị thiếu hình ";
                TempData["isAddError"] = true;
                ModelState.AddModelError(nameof(model.Hinh), "Vui lòng chọn Hình");
                return RedirectToAction("AdminGet", model);

            }
            if (ModelState.IsValid)
            {


                int count = _hangHoaService.GetAll(null,null).Where(l => l.TenHH == model.TenHH).Count();


                if (count > 0)
                {
                    TempData["Message"] = @$"Không thể thêm Hang Hoa:'{model.TenLoai}'-Bị trùng tên trong ds hàng hoá";
                    TempData["isAddError"] = true;
                    return RedirectToAction("AdminGet", model);
                }

                try
                {
                    var result = _mapper.Map<HangHoaModel>(model);
                    result.Hinh = MyUtil.UploadHinh(Hinh, "HangHoa");// chưa sử lý hình trùng tên
                    _hangHoaService.Add(result);
                    TempData["isAddError"] = false;
                    TempData["Message"] = $"Đã thêm Hàng Hoá :'{model.TenHH}'";
                    


                    return RedirectToAction("AdminGet", model);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["Message"] = $"Không thể thêm Hàng Hoá:'{model.TenHH}'";
                    TempData["isAddError"] = true;
                    return RedirectToAction("AdminGet", model);
                }
            }
            TempData["Message"] = $"Không thể thêm HangHoa: '{model.TenHH}'-Dữ liệu không phù hợp";
            TempData["isAddError"] = true;
           
            return RedirectToAction("AdminGet", model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        public IActionResult AdminDelete(int id)
        {
            //List<MenuLoaiVM> result = new List<MenuLoaiVM>();
            try
            {
                var result = _hangHoaService.GetById(id);
                MyUtil.DeleteHinh(result.Hinh, "HangHoa");
                _hangHoaService.Delete(id);
                TempData["Message"] = $"Đã xoá Hàng Hoá có id:{id} ";
                return RedirectToAction("AdminGet");
              
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Message"] = $"Không thể xoá Hàng Hoá có id:\"{id}\"-Không tồn tại ";
                return RedirectToAction("AdminGet");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Message"] = $"Không thể xoá Hàng Hoá có id:\"{id}\"-Bị ràng buộc dữ liệu ";
                return RedirectToAction("AdminGet");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Message"] = $"Không thể xoá Hàng Hoá có id: {id} ";
                return RedirectToAction("AdminGet");
            }
        }

        #endregion
    }
}
