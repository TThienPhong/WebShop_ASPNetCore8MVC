using Microsoft.AspNetCore.Mvc;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using WebShop_ASPNetCore8MVC_v1.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace WebShop_ASPNetCore8MVC_v1.Controllers
{
    public class CartController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly Hshop2023Context db;
    

        public CartController(Hshop2023Context context, PaypalClient paypalClient)
        {
            _paypalClient=paypalClient;
            db = context;
          
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
                    GiongKhachHang = false,
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
        public IActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                /* var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
                 var khachHang = new KhachHang();
                 if (model.GiongKhachHang)
                 {
                     khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
                 }*/

               
/*
                var hoadon = new HoaDon
                {
                    MaKh = customerId,
                    HoTen = model.HoTen ?? khachHang.HoTen,
                    DiaChi = model.DiaChi ?? khachHang.DiaChi,
                    DienThoai = model.DienThoai ?? khachHang.DienThoai,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "COD",
                    CachVanChuyen = "GRAB",
                    MaTrangThai = 0,
                    GhiChu = model.GhiChu
                };*/

                db.Database.BeginTransaction();
                try
                {

                    var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;


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
                    foreach (var item in model.CartItems)
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
                HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());          
                var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

                return Ok(response);
            }
            catch (Exception ex)
            {
                db.Database.RollbackTransaction();
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

                // Lưu database đơn hàng của mình


                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        #endregion
    }
}
