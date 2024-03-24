using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using AutoMapper;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Helpers;
using Microsoft.AspNetCore.Http;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
	public class AdminController : Controller
	{

		private readonly Hshop2023Context db;
		private readonly IMapper _mapper;

		public AdminController(Hshop2023Context context, IMapper mapper)
		{
			db = context;
			_mapper = mapper;
		}
		public IActionResult Index()
		{
			return View();
		}



		#region Login 
		[HttpGet]
		public async Task<IActionResult> DangNhapAsync(string? ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			//Kiểm tra Admin đã đăng nhập
			var co = await HttpContext.AuthenticateAsync("AdminCookieAuthenticationScheme");
			if (co.Succeeded)
			{
				
				return RedirectToAction("Profile", "Admin");
			}
			

			return View();

		}
		[HttpPost]
		public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
		{

			ViewBag.ReturnUrl = ReturnUrl;
			if (ModelState.IsValid)
			{
				var admin = db.NhanViens.SingleOrDefault(am => am.MaNv == model.UserName);
				if (admin == null)
				{
					ModelState.AddModelError("loi", "Không có Admin này");
				}
				else
				{
					if (admin.MatKhau != model.Password.ToMd5Hash(admin.RandomKey))
					{
						ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
					}
					else
					{
						var claims = new List<Claim> {
								new Claim(ClaimTypes.Email, admin.Email),
								new Claim(ClaimTypes.Name, admin.HoTen),
								new Claim("CustomerID", admin.MaNv),

								//claim - role động
								new Claim(ClaimTypes.Role, "NhanVien")
							};

						var claimsIdentity = new ClaimsIdentity(claims, "AdminCookieAuthenticationScheme");
						//var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
						var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
						//Đăng xuất tất cả các phiên đăng nhập
						//await HttpContext.SignOutAsync();
						await HttpContext.SignOutAsync("AdminCookieAuthenticationScheme");
						await HttpContext.SignOutAsync();

						await HttpContext.SignInAsync("AdminCookieAuthenticationScheme",claimsPrincipal);

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
			return View();
		}

		#endregion

		[Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
		public IActionResult Profile()
		{
			return View();
		}

		[Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
		public async Task<IActionResult> DangXuat()
		{
			await HttpContext.SignOutAsync("AdminCookieAuthenticationScheme");
			return Redirect("/");
		}


	}
}
