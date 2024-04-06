using AutoMapper;
using Humanizer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public class LoaiHangHoaService : ILoaiHangHoaService
	{
		private readonly Hshop2023Context _context;
        private readonly IMapper _mapper;
        public LoaiHangHoaService(Hshop2023Context context,IMapper  mapper)
		{
			_context = context;
			_mapper = mapper;

        }

		public IEnumerable<LoaiModel> GetAll(string? query)
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


			/*var result = data.Select(lo => new LoaiModel
			{
				MaLoai = lo.MaLoai,
				TenLoai = lo.TenLoai,
				SoLuong = lo.HangHoas.Count
			}).OrderBy(i => i.TenLoai);*/

			var result = data.Include(lo=>lo.HangHoas).Select(lo => _mapper.Map<LoaiModel>(lo));

			/*var result=new List<LoaiModel>();

            foreach ( var item in data)
			{
				var l = _mapper.Map<LoaiModel>(item);
				result.Add(l);

            }
			//result = result.OrderBy(i => i.TenLoai);*/


			return result;
		}

		public LoaiModel GetById(int loaiHHId)
		{
            var loai = _context.Loais
				.Include(lo => lo.HangHoas) // Include dữ liệu từ bảng HangHoa vào bảng Loais
				.SingleOrDefault(lo => lo.MaLoai == loaiHHId);
            if (loai != null)
			{
				return new LoaiModel
                {
					MaLoai = loai.MaLoai,
					TenLoai = loai.TenLoai,
					SoLuong=loai.HangHoas.Count,
				};
			}
			return null;
		}
		public void Add(LoaiModel loaiHH)
		{
			if (loaiHH == null)
			{
				throw new ArgumentNullException(nameof(loaiHH), "cannot be null.");
			}
			else
			{
                try
                {

					Loai newLoai = _mapper.Map<Loai>(loaiHH);
					newLoai.MaLoai = 0;// trong csdl tự tăng
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


		public void Update(LoaiModel loaiHH)
		{
			if (loaiHH == null)
			{
				throw new ArgumentNullException(nameof(loaiHH), "cannot be null.");
				return;

            }
            var loai = _context.Loais.SingleOrDefault(lo => lo.MaLoai == loaiHH.MaLoai);

            if (loai != null)
			{
                _mapper.Map(loaiHH, loai);

                _context.SaveChanges();
			}
			else { 
				throw new ArgumentException("Loai not found.", nameof(loaiHH)); 
			}
           
        }
	}
}
