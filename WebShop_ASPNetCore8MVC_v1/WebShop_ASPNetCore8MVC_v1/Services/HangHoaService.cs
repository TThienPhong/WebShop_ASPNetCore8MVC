using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public class HangHoaService : IHangHoaService
    {
        private readonly Hshop2023Context _context;

        public HangHoaService(Hshop2023Context context ) { 
            _context=context;
        }
        public void AddProduct(ChiTietHangHoaVM product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public List<ChiTietHangHoaVM> GetAll(int? loaiId, string? query)
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

                data = data.Where(item => item.TenHh.Contains(query) || item.MaLoai == number);
            }
            #endregion


            var result = data.Select(p => new ChiTietHangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai

            }).OrderBy(i => i.TenHH);


            return result.ToList();
        }
        public List<HangHoaVM> GetAllProduc(int? loai, string? query)
        {
            List<HangHoaVM>  result =new List<HangHoaVM>();
            return null;
        }
        public ChiTietHangHoaVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(ChiTietHangHoaVM product)
        {
            throw new NotImplementedException();
        }
    }
}
