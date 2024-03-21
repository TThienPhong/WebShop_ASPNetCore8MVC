using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Helpers;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class KhachHangController : Controller 
    {
		private readonly Hshop2023Context db;
		private readonly IMapper _mapper;

		public KhachHangController(Hshop2023Context context, IMapper mapper)
		{
			db = context;
			_mapper = mapper;
		}

	

		[HttpGet]
		public IActionResult DangKy()
        {

            return View();
        }

		[HttpPost]
		public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
		{

			if (ModelState.IsValid)
			{
                var existingUser = db.KhachHangs.FirstOrDefault(kh => kh.MaKh == model.MaKh || kh.Email == model.Email);
				if (existingUser != null)
				{
                    // Nếu tài khoản đã tồn tại, hiển thị thông báo lỗi và trả về lại view đăng ký
                    // Nếu tài khoản đã tồn tại, hiển thị thông báo lỗi và trả về lại view đăng ký
                    if (existingUser.MaKh == model.MaKh)
                    {
                        ModelState.AddModelError(nameof(model.MaKh), "Tên đăng nhập đã được sử dụng. Vui lòng chọn một tên khác.");
                    }
                    if (existingUser.Email == model.Email)
                    {
                        ModelState.AddModelError(nameof(model.Email), "Địa chỉ email đã được sử dụng. Vui lòng chọn một địa chỉ email khác.");
                    }
                    return View(model);
                }
                try
				{
					var khachHang = _mapper.Map<KhachHang>(model);
					khachHang.RandomKey = MyUtil.GenerateRamdomKey();
					khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
					khachHang.HieuLuc = true;//nếu có thời gian dùng Mail để active
					khachHang.VaiTro = 0;

					if (Hinh != null)
					{
                        
						khachHang.MaKh.ToString();
                        khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
					}

					db.Add(khachHang);
					db.SaveChanges();
					return RedirectToAction("Index", "HangHoa");
				}
				catch (Exception ex)
				{
					var mess = $"{ex.Message} shh";
				}
			}
			return View();
		}
	}
}
