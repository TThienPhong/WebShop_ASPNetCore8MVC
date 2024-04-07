using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Helpers;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WebShop_ASPNetCore8MVC_v1.Services;
using WebShop_ASPNetCore8MVC_v1.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class KhachHangController : Controller 
    {
		private readonly Hshop2023Context db;
		private readonly IMapper _mapper;
        private readonly IKhachHangService _khachHangService;

        public KhachHangController(Hshop2023Context context, IMapper mapper,IKhachHangService khachHangService)
		{
			db = context;
			_mapper = mapper;
			_khachHangService = khachHangService;


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
                    var khachHang = _mapper.Map<KhachHangModel>(model);
					/*khachHang.RandomKey = MyUtil.GenerateRamdomKey();
					khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
					khachHang.HieuLuc = true;//nếu có thời gian dùng Mail để active
					khachHang.VaiTro = 0;
*/
					if (Hinh != null)
                    {

                        //khachHang.MaKh.ToString();
                        khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                    }
                    else
                    {
                        khachHang.Hinh=Img.ImgAvatarNull;
                    }

                    /* db.Add(khachHang);
                     db.SaveChanges();*/
                    _khachHangService.Add(khachHang);
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
        public async Task<IActionResult> DangNhapAsync(string? ReturnUrl)
        {
			
			var co = await HttpContext.AuthenticateAsync();
			if (co.Succeeded)
			{
				return RedirectToAction("Profile", "KhachHang");
			}
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
			string maKH=User.FindFirstValue("CustomerID")??"";
			if (string.IsNullOrEmpty(maKH))
			{
                ModelState.AddModelError("loi", "Đăng nhập để xem thông tinh của mình");
				return Redirect("/");

            }
			var kh = _mapper.Map<RegisterVM>(_khachHangService.GetById(maKH));
            if (kh==null)
            {
                ModelState.AddModelError("loi", "Khách hàng không tồn tại trong hệ thống");
                return Redirect("/");

            }
            if (string.IsNullOrEmpty(kh.Hinh))
            {
                kh.Hinh = "avatarNull.jpg";
            }
            //var kh = _mapper.Map<RegisterVM>();
            return View(kh);
		}

        [Authorize]
        public IActionResult Update(RegisterVM khachHang,IFormFile? Hinh)
        {
            string maKH = User.FindFirstValue("CustomerID") ?? "";
            if (string.IsNullOrEmpty(maKH))
            {
                ModelState.AddModelError("loi", "Đăng nhập để xem thông tinh của mình");
                return Redirect("/");

            }
            var khM = _khachHangService.GetById(maKH);
            if (khM == null)
            {
                ModelState.AddModelError("loi", "Khách hàng không tồn tại trong hệ thống");
                return Redirect("/");

            }

            if(khachHang==null)
            {
                ModelState.AddModelError("loi", "Cần xem lại thông tin cập nhật");
                return Redirect("/");
            }
            if(ModelState.IsValid)
            {
                if (khachHang.MaKh != maKH)
                {
                    ModelState.AddModelError("loi", "Chỉ được cập nhật thông tin của mình");
                    return Redirect("/");
                }
                if (khachHang.MatKhau.ToMd5Hash(khM.RandomKey) != khM.MatKhau)
                {
                    ModelState.AddModelError("loi", "Mật khẩu xác nhận không chính xác");
                    return RedirectToAction("Profile", khachHang);
                }
                if (Hinh != null)
                {
                    if (khM.Hinh != "avatarNull.jpg" && !string.IsNullOrEmpty(khM.Hinh))
                    { MyUtil.DeleteHinh(khM.Hinh, "KhachHang"); }

                    khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");

                }
                else
                {
                    khachHang.Hinh = khM.Hinh;
                }

                _khachHangService.Update(_mapper.Map<KhachHangModel>(khachHang));
                TempData["Message"] = "Cập nhật thông tin thành công";
                //var kh = _mapper.Map<RegisterVM>();
                return RedirectToAction("Profile", khachHang);
            }
            TempData["Message"] = "Không thể cập nhật thông tin";
            //var kh = _mapper.Map<RegisterVM>();
            return RedirectToAction("Profile", khachHang);

        }

        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme,Cookies")]//nếu chưa đăng nhập chỉ đến Cookies
		public IActionResult ProfileKhachOrAdmin()
		{
			return View();
		}

		[Authorize ]
		public async Task<IActionResult> DangXuat()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}


        #region AdminQLKhachHang
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        [HttpGet]
        public IActionResult AdminGetAll(bool? taiKhoang, string? query)
        {


            try
            {
                IEnumerable<KhachHangModel> data = _khachHangService.GetAll(taiKhoang, query);
                var result = data.Select(p => _mapper.Map<KhachHangVM>(p));
                //var hangHoaMV = ViewBag.hangHoaMV as HangHoaVM_admin ;
               /* ViewBag.isAddError = TempData["isAddError"] as bool? ?? false;
                ViewBag.isUpdateError = TempData["isUpdateError"] as bool? ?? false;
                ViewBag.hangHoaMV = model;*/
                TempData["khachHangList"] = result;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("loi", "Không thể gọi danh sách KhachHang ");
                TempData["Message"] = $"Không tể gọi danh sách KhachHang";
                return View();
            }
        }


        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        [HttpGet]
        public IActionResult AdminVoHieu(string MaKH)
        {
           
            try 
            {
                var kh = _khachHangService.GetById(MaKH);
                kh.HieuLuc = false;
                _khachHangService.Update(kh);
                TempData["Message"] = $"Đã vô hiệu tài khoảng: {MaKH}";
                //var query = MaKH;
                return RedirectToAction("AdminGetAll", new { query = MaKH });
            }
            catch
            {
                //var query = MaKH;
                TempData["Message"] = $"Không thể vô hiệu tài khoảng: {MaKH}";
                return RedirectToAction("AdminGetAll", new { query = MaKH });
            }

        }
        [Authorize(AuthenticationSchemes = "AdminCookieAuthenticationScheme")]
        [HttpGet]
        public IActionResult AdminKichHoat(string MaKH)
        {

            try
            {
                var kh = _khachHangService.GetById(MaKH);
                kh.HieuLuc = true;
                _khachHangService.Update(kh);
                TempData["Message"] = $"Đã Kích hoạt tài khoảng: {MaKH}";
                var query = MaKH;
                return RedirectToAction("AdminGetAll", new { query = MaKH });
            }
            catch
            {
                TempData["Message"] = $"Không thể Kích hoạt tài khoảng: {MaKH}";
                var query = MaKH;
                
                return RedirectToAction("AdminGetAll", new { query = MaKH });
            }

        }

        #endregion

    }
}
