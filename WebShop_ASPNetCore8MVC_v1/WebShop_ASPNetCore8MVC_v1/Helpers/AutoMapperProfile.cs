using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;
using WebShop_ASPNetCore8MVC_v1.ViewModels;

namespace WebShop_ASPNetCore8MVC_v1.Helpers
{
	public class AutoMapperProfile: Profile     
	{
       
        public AutoMapperProfile()
		{
            
            Hshop2023Context _dbContext=new Hshop2023Context();
            CreateMap<RegisterVM, KhachHang>();
            //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
            //.ReverseMap();
           
            CreateMap<MenuLoaiVM, LoaiModel>()
			.ReverseMap();

            CreateMap<ChiTietHangHoaVM, HangHoaModel>()
              .ForMember(dest => dest.MaHh, opt => opt.MapFrom(src => src.MaHh))
              .ForMember(dest => dest.TenHH, opt => opt.MapFrom(src => src.TenHH))
              .ForMember(dest => dest.Hinh, opt => opt.MapFrom(src => src.Hinh))
              .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.DonGia))
              .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan))
              //.ForMember(dest => dest.LoaiHH.TenLoai, opt => opt.MapFrom(src => src.TenLoai))
              .ForMember(dest => dest.ChiTiet, opt => opt.MapFrom(src => src.ChiTiet))
              .ForMember(dest => dest.DiemDanhGia, opt => opt.MapFrom(src => src.DiemDanhGia))
              .ForMember(dest => dest.SoLuongTon, opt => opt.MapFrom(src => src.SoLuongTon))
             .AfterMap((src, dest) =>
             {
                 // Tìm MaLoai từ TenLoai trong DBContext và gán vào dest.Loai.MaLoai
                 var maLoai = _dbContext.Loais.SingleOrDefault(l => l.TenLoai == src.TenLoai)?.MaLoai;
                 if (maLoai != null)
                 {
                     dest.LoaiHH = new LoaiModel { MaLoai = maLoai.Value, TenLoai = src.TenLoai };
                 }
             })
            .ReverseMap();
           
            //---Hang Hoá
            CreateMap<HangHoaModel, HangHoaVM_admin>()
               .ForMember(dest => dest.TenLoai, opt => opt.MapFrom(src => src.LoaiHH.TenLoai))
               .ForMember(dest => dest.MaLoai, opt => opt.MapFrom(src => src.LoaiHH.MaLoai));

            CreateMap<HangHoaVM_admin, HangHoaModel>()
                .ForMember(dest => dest.LoaiHH, opt => opt.MapFrom(src => new LoaiModel
                {
                    MaLoai = src.MaLoai,
                    TenLoai = src.TenLoai??""
                }));
            //---------------------------------------------------------
            CreateMap<Loai, LoaiModel>()
                .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.HangHoas.Count));
            CreateMap<LoaiModel, Loai>();
            CreateMap<HangHoaModel, HangHoa>()
               .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.ChiTiet ?? ""))
               .ForMember(dest => dest.MoTaDonVi, opt => opt.MapFrom(src => src.MoTaNgan ?? ""))
               .ForMember(dest => dest.MaLoai, opt => opt.MapFrom(src => src.LoaiHH.MaLoai));

            //.ReverseMap(); // Cho phép ánh xạ ngược từ HangHoaVM sang HangHoaModel
            CreateMap<HangHoa, HangHoaModel>()
               .ForMember(dest => dest.ChiTiet, opt => opt.MapFrom(src => src.MoTa ?? ""))
               .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaDonVi ?? ""))
               .ForMember(dest => dest.LoaiHH, opt => opt.MapFrom(src => src.MaLoaiNavigation));
              
        }

    }
}
