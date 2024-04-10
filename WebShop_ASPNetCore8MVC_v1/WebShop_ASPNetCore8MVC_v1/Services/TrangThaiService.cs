using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System;
using WebShop_ASPNetCore8MVC_v1.Data;
using WebShop_ASPNetCore8MVC_v1.Models;

namespace WebShop_ASPNetCore8MVC_v1.Services
{
    public class TrangThaiService : ITrangThaiService
    {
        private readonly Hshop2023Context _context;
        private readonly IMapper _mapper;

        public TrangThaiService(Hshop2023Context context, IMapper mapper) 
        {
            _context = context;          
            _mapper = mapper;
        }
        public void Add(TrangThaiModel trangThai)
        {
            throw new NotImplementedException();
        }

        public void Delete(int maTrangThai)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrangThaiModel> GetAll()
        {
            var data = _context.TrangThais.AsQueryable();
           
            var result = data
                .Select(p => _mapper.Map<TrangThaiModel>(p));
            return result;
        }

        public TrangThaiModel GetById(int maTrangThai)
        {
            throw new NotImplementedException();
        }

        public void Update(TrangThaiModel trangThai)
        {
            throw new NotImplementedException();
        }
    }
}
