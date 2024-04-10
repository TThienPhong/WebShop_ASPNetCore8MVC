using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly Hshop2023Context _context;
        
        private readonly IMapper _mapper;

        public HoaDonService(Hshop2023Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;


        }
        public void Add(HoaDonModel hoaDon)
        {
            throw new NotImplementedException();
        }

        public void Delete(int maHD)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HoaDonModel> GetAll(int? maHD, string? maKH, int? maTrangThai, DateTime? from, DateTime? to)
        {
            var data = _context.HoaDons.AsQueryable();
           
            #region Filtering
            if (maHD != null)
            {
                data = data.Where(item => item.MaHd == maHD);
            }
            if (maKH != null)
            {
                data = data.Where(item => item.MaKh == maKH);
            }
            if (maTrangThai != null)
            {
                data = data.Where(item => item.MaTrangThai == maTrangThai);
            }

            if (from!=null)
            {
                
                data = data.Where(item => item.NgayDat.Date >= from.Value.Date);
            }
            if (to != null)
            {

                data = data.Where(item => item.NgayDat.Date <= to.Value.Date);
            }
            #endregion
            data = data.OrderByDescending(hd => hd.NgayDat);

            var result = data
                .Include(p => p.MaTrangThaiNavigation)
                .Include(p => p.ChiTietHds)
                    .ThenInclude(ct => ct.MaHhNavigation)
                .Select(p => _mapper.Map<HoaDonModel>(p));
            
            
            //int n=result.Count();
            return result;
        }

        public HoaDonModel GetById(int maHD)
        {
            throw new NotImplementedException();
        }

        public void Update(HoaDonModel hoaDon)
        {
            throw new NotImplementedException();
        }
    }
}
