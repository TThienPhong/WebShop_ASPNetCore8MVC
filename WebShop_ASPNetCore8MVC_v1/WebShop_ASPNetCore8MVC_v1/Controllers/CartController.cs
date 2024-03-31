using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using WebShop_ASPNetCore8MVC_v1.Helpers;
using Microsoft.AspNetCore.Authorization;
using WebShop_ASPNetCore8MVC_v1.Services;


namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class CartController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly Hshop2023Context db;
        private  static CheckoutVM _checkoutVM ;
        private readonly IVnPayService _vnPayservice;

        public CartController(Hshop2023Context context, PaypalClient paypalClient, IVnPayService vnPayservice)
        {
            _paypalClient=paypalClient;
            db = context;
            _vnPayservice = vnPayservice;
			CheckoutVM _checkoutVM=new CheckoutVM();



		}

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        #region GioHang

       
        public IActionResult Index()
        {

            return View(Cart);
        }
            

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Index");    

        }

        

        public IActionResult RemoveCartItem(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateQuantity(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.SoLuong = quantity;
                }

                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }   
            return RedirectToAction("Index");
        }
        #endregion

        #region Checkout
       


        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return RedirectToAction("Index");
            }
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
            if (khachHang != null)
            {
                var model = new CheckoutVM(){
                    CollectedMoney = false,
                    HoTen = khachHang.HoTen,
                    DiaChi = khachHang.DiaChi??"",
                    DienThoai = khachHang.DienThoai??"",
                    CartItems = Cart
                };
                ViewBag.PaypalClientdId = _paypalClient.ClientId;
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model, string payment = "COD")
        {
;
			if (ModelState.IsValid)
            {
				var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
                _checkoutVM = new CheckoutVM() {
                    HoTen = model.HoTen,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,
                    GhiChu = model.GhiChu,
                    CartItems = Cart.ToList()
				} ;
                
				if (payment == "Thanh toán VNPay")
				{
                    
					var vnPayModel = new VnPaymentRequestModel
					{
						Amount = Cart.Sum(p => p.ThanhTien),
						CreatedDate = DateTime.Now,
						Description = $"{model.HoTen} {model.DienThoai}",
						FullName = model.HoTen,
						OrderId = new Random().Next(1000, 100000)
					};
					return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
				}
				db.Database.BeginTransaction();
				try
				{
					var hoadon = new HoaDon
					{
						MaKh = customerId,
						HoTen = model.HoTen,
						DiaChi = model.DiaChi,
						DienThoai = model.DienThoai,
						NgayDat = DateTime.Now,
						CachThanhToan = "COD",
						CachVanChuyen = "GRAB",

						MaTrangThai = 0,
						GhiChu = model.GhiChu
					};
					
					db.Add(hoadon);
					db.SaveChanges();

					var cthds = new List<ChiTietHd>();
					foreach (var item in _checkoutVM.CartItems)
					{
						cthds.Add(new ChiTietHd
						{
							MaHd = hoadon.MaHd,
							SoLuong = item.SoLuong,
							DonGia = item.DonGia,
							MaHh = item.MaHh,
							GiamGia = 0
						});
					}
					db.AddRange(cthds);
					db.SaveChanges();
            
                    db.Database.CommitTransaction();
                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());                   
                    return View("Success");
                }
                catch
                {
                    db.Database.RollbackTransaction();
                }
            }

            return View(model);
        }
       
        #endregion
        [Authorize]
        public IActionResult PaymentSuccess()
        {
            //db.Database.CommitTransaction();
            return View("Success");
        }
        #region Paypal payment
        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            // Thông tin đơn hàng gửi qua Paypal
            var tongTien = Cart.Sum(p => p.ThanhTien).ToString();
            var donViTienTe = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();
            
            try
            {               
                        
                var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

                return Ok(response);
            }
            catch (Exception ex)
            {
                //db.Database.RollbackTransaction();
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [Authorize]
        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                
                //db.Database.CommitTransaction();

                return Ok(response);
            }
            catch (Exception ex)
            {
                //db.Database.RollbackTransaction();
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        #endregion

        [Authorize]
        [HttpGet]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
				//db.Database.RollbackTransaction();
				TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }


			// Lưu đơn hàng vô database
			db.Database.BeginTransaction();
			try
			{

				var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;


				var hoadon = new HoaDon
				{
					MaKh = customerId,
					HoTen = _checkoutVM.HoTen,
					DiaChi = _checkoutVM.DiaChi,
					DienThoai = _checkoutVM.DienThoai,
					NgayDat = DateTime.Now,
					CachThanhToan = PaymentType.VNPAY,
					CachVanChuyen = "GRAB",

					MaTrangThai = 1,
					GhiChu = _checkoutVM.GhiChu
				};
				/*if (_checkoutVM.CollectedMoney == true)
				{ hoadon.MaTrangThai = 1; }*/
				db.Add(hoadon);
				db.SaveChanges();

				var cthds = new List<ChiTietHd>();
				foreach (var item in _checkoutVM.CartItems)
				{
					cthds.Add(new ChiTietHd
					{
						MaHd = hoadon.MaHd,
						SoLuong = item.SoLuong,
						DonGia = item.DonGia,
						MaHh = item.MaHh,
						GiamGia = 0
					});
				}
				db.AddRange(cthds);
				db.SaveChanges();


				db.Database.CommitTransaction();
				HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

				
			}
			catch
			{

				db.Database.RollbackTransaction();
				TempData["Message"] = $"Đơn hàng không đặt được, liên hệ HShop để hoàn tiền {response.VnPayResponseCode}";
				return RedirectToAction("PaymentFail");
			}
			TempData["Message"] = $"Thanh toán VNPay thành công";
            return RedirectToAction("PaymentSuccess");
        }
    }
}
