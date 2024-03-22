using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Helpers;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using Microsoft.AspNetCore.Authorization;

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

        #region  Register

        [HttpGet]
        public IActionResult DangKy()
        {

            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile? Hinh=null)
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
        #endregion

        #region Login 
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model,string? ReturnUrl)
        {

			ViewBag.ReturnUrl = ReturnUrl;
			if (ModelState.IsValid)
			{
				var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
				if (khachHang == null)
				{
					ModelState.AddModelError("loi", "Không có khách hàng này");
				}
				else
				{
					if (!khachHang.HieuLuc)
					{
						ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
					}
					else
					{
						if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
						{
							ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
						}
						else
						{
							var claims = new List<Claim> {
								new Claim(ClaimTypes.Email, khachHang.Email),
								new Claim(ClaimTypes.Name, khachHang.HoTen),
								new Claim("CustomerID", khachHang.MaKh),

								//claim - role động
								new Claim(ClaimTypes.Role, "Customer")
							};

							var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
							var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
							await HttpContext.SignOutAsync();
							await HttpContext.SignOutAsync("AdminCookieAuthenticationScheme");
							await HttpContext.SignInAsync(claimsPrincipal);

							if (Url.IsLocalUrl(ReturnUrl))
							{
								return Redirect(ReturnUrl);
							}
							else
							{
								return Redirect("/");
							}
						}
					}
				}
			}
			return View();
		}

		#endregion

		[Authorize]
		public IActionResult Profile()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> DangXuat()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}




	}
}
