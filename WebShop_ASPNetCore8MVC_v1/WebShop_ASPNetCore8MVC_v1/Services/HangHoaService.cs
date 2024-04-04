using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public class HangHoaService : IHangHoaService
    {
        private readonly Hshop2023Context _context;
        private readonly ILoaiHangHoaService _loaiHangHoaService;
        private readonly IMapper _mapper;

        public HangHoaService(Hshop2023Context context,ILoaiHangHoaService loaiHangHoaService,IMapper mapper) 
        { 
            _context=context;
            _loaiHangHoaService=loaiHangHoaService;
            _mapper=mapper;


        }
        public void Add(HangHoaModel product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "cannot be null.");
                return;
            }

            try
            {
                var hh = _mapper.Map<HangHoa>(product);
                hh.MaHh = 0;
                _context.Add(hh);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thêm được Hàng Hoá {product.TenHH} " + ex.Message, ex);
            }
        }

        public void Delete(int productId)
        {
           /* if (productId < 0)
            {
                throw new ArgumentNullException(nameof(productId), "cannot nhỏ hơn 0 .");
                return;
            }*/

            var hanghoa = _context.HangHoas.SingleOrDefault(p => p.MaHh == productId);
            if (hanghoa != null)
            {
                try
                {
                    _context.Remove(hanghoa);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Không thể xoá Hàng Hoá có id: {productId}", ex);
                }
            }
            else
            {
                throw new ArgumentException("hangHoa not found.", nameof(productId));
            }

        }

        public IEnumerable<HangHoaModel> GetAll(int? loaiId, string? query)
        {
            var data = _context.HangHoas.AsQueryable();
            #region Filtering
            if (loaiId!=null)
            {             
                data = data.Where(item => item.MaLoai== loaiId);
            }
            if (!string.IsNullOrEmpty(query))
            {
                int number = -1;
                bool isNumber = int.TryParse(query, out number);
                data = data.Where(item => item.TenHh.Contains(query) || item.MaHh == number);
            }
            #endregion
            var result = data.Include(p=>p.MaLoaiNavigation).Select(p => _mapper.Map<HangHoaModel>(p));
            //int n=result.Count();
            return result;
        }
       /* public IEnumerable<HangHoaModel> GetAllProduc(int? loaiId, string? query)
        {
            var data = _context.HangHoas.AsQueryable();
            #region Filtering
            if (loaiId != null)
            {
                data = data.Where(item => item.MaLoai == loaiId);
            }
            if (!string.IsNullOrEmpty(query))
            {
                int number = -1;
                bool isNumber = int.TryParse(query, out number);

                data = data.Where(item => item.TenHh.Contains(query) || item.MaLoai == number);
            }
            #endregion


            var result = data.Select(p => new HangHoaModel
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                //Loai.Ten = p.MaLoaiNavigation.TenLoai

            }).OrderBy(i => i.TenHH);


            return result;
        }*/
        public HangHoaModel GetById(int id)
        {
            var hh = _context.HangHoas
               .Include(p => p.MaLoaiNavigation)// Include dữ liệu từ bảng Loái vào bảng HangHoas
               .Include(p => p.ChiTietHds)
               .SingleOrDefault(p => p.MaHh == id);
            if (hh != null)
            {
                return _mapper.Map<HangHoaModel>(hh);
               
            }
            return null;
        }

        public void Update(HangHoaModel product)
        {

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "cannot be null.");
                return;
            }

            var hanghoa = _context.HangHoas.SingleOrDefault(p => p.MaHh == product.MaHh);
            if (hanghoa != null)
            {
                try
                {
                    
                    _mapper.Map(product,hanghoa);

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Không thể cập nhật Hàng Hoá", ex);
                }             
            }
            else
            {
                throw new ArgumentException("Loai not found.", nameof(product));
            }

          
        }
    }
}
