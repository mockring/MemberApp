using AutoMapper;
using MemerApp.Data;
using MemerApp.Dtos;
using MemerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MemerApp.Services
{
    public class CouponService : ICouponService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CouponService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ---------- 讀取 ----------
        public async Task<IEnumerable<CouponDto>> GetAllAsync()
        {
            var list = await _context.Coupons.AsNoTracking().ToListAsync();
            return _mapper.Map<List<CouponDto>>(list);
        }

        public async Task<CouponDto?> GetByIdAsync(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            return coupon == null ? null : _mapper.Map<CouponDto>(coupon);
        }

        // ---------- 建立 ----------
        public async Task CreateAsync(CouponDto dto)
        {
            var coupon = _mapper.Map<CouponModel>(dto);
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            // 回傳時帶上產生的 Id
            dto.CouponId = coupon.CouponId;
        }

        // ---------- 更新 ----------
        public async Task UpdateAsync(CouponDto dto)
        {
            var coupon = await _context.Coupons.FindAsync(dto.CouponId);
            if (coupon == null)
                throw new KeyNotFoundException($"Coupon with Id={dto.CouponId} not found.");

            // 把 Dto 的屬性覆蓋到 Model
            _mapper.Map(dto, coupon);

            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
        }

        // ---------- 刪除 ----------
        public async Task DeleteAsync(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null) return;    // 如果找不到直接返回

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
        }

        // ---------- 存在性 ----------
        public async Task<bool> ExistsAsync(int id)
            => await _context.Coupons.AnyAsync(e => e.CouponId == id);
    }
}