using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly Hshop2023Context _context;
        //private readonly ILoaiHangHoaService _loaiHangHoaService;
        private readonly IMapper _mapper;

        public KhachHangService(Hshop2023Context context, IMapper mapper)
        {
            _context = context;
            //_loaiHangHoaService=loaiHangHoaService;
            _mapper = mapper;


        }
        public void Add(KhachHangModel khachHang)
        {
            if (khachHang == null)
            {
                throw new ArgumentNullException(nameof(khachHang), "cannot be null.");
                return;
            }

            try
            {
                var hh = _mapper.Map<KhachHang>(khachHang);
                
                _context.Add(hh);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thêm được Khách hàng: {khachHang.MaKh} " + ex.Message, ex);
            }
        }

        public void Delete(string MaKH)//hạn chế dùng
        {
            if (string.IsNullOrEmpty(MaKH))
            {
                throw new ArgumentNullException(nameof(MaKH), "cannot be Null or Empty.");
                return;
            }
            var kh = _context.KhachHangs.SingleOrDefault(p => p.MaKh == MaKH);
            if (kh != null)
            {
                try
                {
                    _context.Remove(kh);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Không thể xoá KhachHang có id: {MaKH}", ex);
                }
            }
            else
            {
                throw new ArgumentException("KhachHangs not found.", nameof(MaKH));
            }
        }

        public IEnumerable<KhachHangModel> GetAll(bool?taiKhoang, string? query)
        {
            #region Filtering
            var data = _context.KhachHangs.AsQueryable();
            if (taiKhoang.HasValue)
            {
                /*int number = -1;
                bool isNumber = int.TryParse(query, out number);*/
                data = data.Where(item => item.HieuLuc== taiKhoang);
            }

            if (!string.IsNullOrEmpty(query))
            {
                /*int number = -1;
                bool isNumber = int.TryParse(query, out number);*/
                data = data.Where(item => item.HoTen.Contains(query) || item.MaKh == query);
            }
            #endregion
            var result = data.Select(p => _mapper.Map<KhachHangModel>(p));
            //int n=result.Count();
            return result;
        }

        public KhachHangModel GetById(string MaKH)
        {
            if(string.IsNullOrEmpty(MaKH))
            {
                return null;
            }    
            var kh = _context.KhachHangs.SingleOrDefault(p => p.MaKh == MaKH);
            if (kh != null)
            {
                return _mapper.Map<KhachHangModel>(kh);

            }
            return null;
        }

        public void Update(KhachHangModel khachHang)
        {
            if (khachHang == null)
            {
                throw new ArgumentNullException(nameof(khachHang), "cannot be null.");
                return;
            }

            var kh = _context.KhachHangs.SingleOrDefault(p => p.MaKh == khachHang.MaKh);
            if (kh != null)
            {
                try
                {

                    _mapper.Map(khachHang, kh);

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Không thể cập nhật KhachHang", ex);
                }
            }
            else
            {
                throw new ArgumentException("Loai not found.", nameof(khachHang));
            }

        }
    }
}
