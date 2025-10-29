using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MemberApp.Data;
using MemberApp.Models;
using MemberApp.Dtos;
using MemberApp.Interface;

namespace MemberApp.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var entities = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Products.FindAsync(id);
            return entity == null ? null : _mapper.Map<ProductDto>(entity);
        }

        public async Task AddAsync(ProductDto dto)
        {
            var entity = _mapper.Map<ProductModel>(dto);
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var existing = await _context.Products.FindAsync(dto.ProductId);
            if (existing == null) throw new KeyNotFoundException("Product not found");

            // Copy only the updatable fields
            existing.ProductName = dto.ProductName;
            existing.PurchasePrice = dto.PurchasePrice;
            existing.SuggestedPrice = dto.SuggestedPrice;
            // CreatedDate stays unchanged

            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) return;

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}