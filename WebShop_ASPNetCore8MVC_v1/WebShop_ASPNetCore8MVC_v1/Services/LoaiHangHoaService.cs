using Microsoft.EntityFrameworkCore;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
	public class LoaiHangHoaService : ILoaiHangHoaService
	{
		private readonly Hshop2023Context _context;

		public LoaiHangHoaService(Hshop2023Context context)
		{
			_context = context;
		}

		public List<MenuLoaiVM> GetAll()
		{
			var data = _context.Loais.Select(lo => new MenuLoaiVM
			{
				MaLoai = lo.MaLoai,
				TenLoai = lo.TenLoai,
				SoLuong = lo.HangHoas.Count
			}).OrderBy(p => p.TenLoai);

			return data.ToList();
		}

		public MenuLoaiVM GetById(int loaiHHId)
		{
			var loai = _context.Loais.SingleOrDefault(lo => lo.MaLoai == loaiHHId);
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

			var loai = _context.Loais.SingleOrDefault(lo => lo.MaLoai == loaiHH.MaLoai);
			if (loai != null)
			{
				loai.TenLoai = loaiHH.TenLoai;
				_context.SaveChanges();
			}
			else
			{
				throw new ArgumentException("Loai not found.", nameof(loaiHH));
			}
		}

		public void Delete(int loaiHHId)
		{
			

			var loai = _context.Loais.SingleOrDefault(lo => lo.MaLoai == loaiHHId);
			if (loai != null)
			{
				_context.Remove(loai);
				_context.SaveChanges();
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
