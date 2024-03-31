using Humanizer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
	public class LoaiHangHoaService : ILoaiHangHoaService
	{
		private readonly Hshop2023Context _context;

		public LoaiHangHoaService(Hshop2023Context context)
		{
			_context = context;
		}

		public List<MenuLoaiVM> GetAll(string? query)
		{
			var data= _context.Loais.AsQueryable();
            #region Filtering
            if (!string.IsNullOrEmpty(query))
            {
				int number=-1;
                bool isNumber = int.TryParse(query, out number);

                data = data.Where(item => item.TenLoai.Contains(query)|| item.MaLoai==number);
            }          
            #endregion


            var result = data.Select(lo => new MenuLoaiVM
			{
				MaLoai = lo.MaLoai,
				TenLoai = lo.TenLoai,
				SoLuong = lo.HangHoas.Count
			}).OrderBy(i => i.TenLoai);


			return result.ToList();
		}

		public MenuLoaiVM GetById(int loaiHHId)
		{
            var loai = _context.Loais
				.Include(lo => lo.HangHoas) // Include dữ liệu từ bảng HangHoa vào bảng Loais
				.SingleOrDefault(lo => lo.MaLoai == loaiHHId);
            if (loai != null)
			{
				return new MenuLoaiVM
				{
					MaLoai = loai.MaLoai,
					TenLoai = loai.TenLoai,
					SoLuong=loai.HangHoas.Count,
				};
			}
			return null;
		}
		public void Add(MenuLoaiVM loaiHH)
		{
			if (loaiHH == null)
			{
				throw new ArgumentNullException(nameof(loaiHH), "cannot be null.");
			}
			else
			{
                try
                {
					
                    Loai newLoai = new Loai
                    {
                        TenLoai = loaiHH.TenLoai

                    };
                    _context.Add(newLoai);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Loai not found.", nameof(loaiHH), ex);
                }              
            }
        }

		public void Delete(int loaiHHId)
		{
			

			var loai = _context.Loais.SingleOrDefault(lo => lo.MaLoai == loaiHHId);
			if (loai != null)
			{
				
				try 
				{
                    _context.Remove(loai);
                    _context.SaveChanges();
                    
                }
				catch(Exception ex)
				{
                    throw new InvalidOperationException($"{loai.TenLoai}-{loai.MaLoai} Không thể bị xoá", ex);

                }
			}
			else
			{
				throw new ArgumentException("Loai not found.", nameof(loaiHHId));
			}
		}


		public void Update(MenuLoaiVM loaiHH)
		{
			if (loaiHH == null)
			{
				throw new ArgumentNullException(nameof(loaiHH), "cannot be null.");
			}
			
			var loai = _context.Loais.SingleOrDefault(lo => lo.MaLoai == loaiHH.MaLoai);
			if (loai != null)
			{
				loai.TenLoai= loaiHH.TenLoai;
				_context.SaveChanges();
			}
			else { 
				throw new ArgumentException("Loai not found.", nameof(loaiHH)); 
			}
		}
	}
}
