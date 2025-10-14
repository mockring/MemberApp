using AutoMapper;
using MemerApp.Data;
using MemerApp.Dtos;
using MemerApp.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace MemerApp.Services
{
    public class ConsumptionService : IConsumptionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ConsumptionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all Consumption
        public async Task<IEnumerable<ConsumptionDto>> GetAllAsync()
            => await _context.Consumptions
                .ProjectTo<ConsumptionDto>(_mapper.ConfigurationProvider)      // AutoMapper 或手寫映射
                .ToListAsync();

        // Get single Consumption
        public async Task<ConsumptionDto?> GetByIdAsync(int id)
            => await _context.Consumptions
                .Where(m => m.ConsumptionId == id)
                .ProjectTo<ConsumptionDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        // Delete Consumption
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Consumptions.FindAsync(id);
            if (entity == null) return false;

            _context.Consumptions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update existing Consumption
        public async Task<bool> UpdateAsync(int id, ConsumptionDto dto)
        {
            if (id != dto.ConsumptionId) return false;

            var entity = await _context.Consumptions.FindAsync(id);
            if (entity == null) return false;

            // Business rule: check phone duplication
            if (await _context.Consumptions.AnyAsync(m => m.MemberPhone == dto.MemberPhone && m.ConsumptionId != id))
                return false;

            entity.MemberName = dto.MemberName;
            entity.MemberPhone = dto.MemberPhone;
            // Keep CreatedDate unchanged

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ConsumptionExistsAsync(id)) throw;
            }

            return true;
        }

        // Helper
        private async Task<bool> ConsumptionExistsAsync(int id)
            => await _context.Consumptions.AnyAsync(e => e.ConsumptionId == id);

        public async Task<IEnumerable<CouponDto>> GetAllCouponsAsync()
        {
            return await _context.Coupons
                .AsNoTracking()
                .Select(c => new CouponDto
                {
                    CouponId = c.CouponId,
                    CouponName = c.CouponName,
                    CalculationMethod = c.CalculationMethod,
                    CouponValue = c.CouponValue,
                    Remark = c.Remark
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByKeywordAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return Enumerable.Empty<ProductDto>();

            var pattern = $"%{keyword}%";

            return await _context.Products
                .AsNoTracking()
                .Where(p => EF.Functions.Like(p.ProductName, pattern))
                .OrderBy(p => p.ProductName)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    PurchasePrice = p.PurchasePrice,
                    SuggestedPrice = p.SuggestedPrice,
                    CreatedDate = p.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var p = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == productId);
            if (p == null) return null;
            return new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SuggestedPrice = p.SuggestedPrice,
                CreatedDate = p.CreatedDate
            };
        }

        public async Task<MemberDto?> FindMemberByPhoneAsync(string phone)
        {
            var m = await _context.Members.AsNoTracking().FirstOrDefaultAsync(x => x.Phone == phone);
            if (m == null) return null;
            return new MemberDto
            {
                MemberId = m.MemberId,
                MemberName = m.MemberName,
                Phone = m.Phone,
                Birthday = m.Birthday,
                CreatedDate = m.CreatedDate
            };
        }

        public async Task<ConsumptionDto> CreateConsumptionAsync(ConsumptionDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Lines == null || !dto.Lines.Any()) throw new ArgumentException("至少需要一筆商品。");

            // 計算每一行：subtotal、coupon、line total
            decimal totalBefore = 0m;
            decimal totalAfter = 0m;

            var entity = new ConsumptionModel
            {
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
                MemberPhone = dto.MemberPhone,
                CreatedDate = dto.CreatedDate
            };

            foreach (var lineDto in dto.Lines)
            {
                var subtotal = Math.Round(lineDto.UnitPrice * lineDto.Quantity, 2);
                lineDto.LineSubtotal = subtotal;

                decimal lineTotal = subtotal;
                decimal discountAmount = 0m;

                if (lineDto.CouponId.HasValue && lineDto.CouponValue.HasValue && lineDto.CouponMethod.HasValue)
                {
                    // Apply coupon according to CalculationMethod
                    switch (lineDto.CouponMethod.Value)
                    {
                        case CalculationMethod.減:
                            lineTotal = subtotal - lineDto.CouponValue.Value;
                            discountAmount = Math.Max(0m, subtotal - lineTotal);
                            break;
                        case CalculationMethod.乘:
                            // e.g. 0.8 => 20% off => multiply by 0.8
                            lineTotal = Math.Round(subtotal * lineDto.CouponValue.Value, 2);
                            discountAmount = Math.Max(0m, subtotal - lineTotal);
                            break;
                        default:
                            break;
                    }
                }

                // Ensure non-negative
                if (lineTotal < 0) lineTotal = 0;

                lineDto.LineTotal = lineTotal;
                lineDto.DiscountAmount = discountAmount;

                // Add to entity lines
                var lineEntity = new ConsumptionLineModel
                {
                    ProductId = lineDto.ProductId,
                    ProductName = lineDto.ProductName,
                    UnitPrice = lineDto.UnitPrice,
                    Quantity = lineDto.Quantity,
                    LineSubtotal = lineDto.LineSubtotal,
                    CouponId = lineDto.CouponId,
                    CouponName = lineDto.CouponName,
                    CouponMethod = lineDto.CouponMethod,
                    CouponValue = lineDto.CouponValue,
                    DiscountAmount = lineDto.DiscountAmount,
                    LineTotal = lineDto.LineTotal
                };

                entity.Lines.Add(lineEntity);

                totalBefore += subtotal;
                totalAfter += lineTotal;
            }

            entity.TotalBeforeDiscount = Math.Round(totalBefore, 2);
            entity.TotalAfterDiscount = Math.Round(totalAfter, 2);

            // 儲存到 DB
            _context.Consumptions.Add(entity);
            await _context.SaveChangesAsync();

            // 回傳 DTO（含計算後的 totals）
            dto.TotalBeforeDiscount = entity.TotalBeforeDiscount;
            dto.TotalAfterDiscount = entity.TotalAfterDiscount;

            return dto;
        }

        public async Task<ConsumptionDto?> GetConsumptionAsync(int consumptionId)
        {
            var c = await _context.Consumptions
                .Include(x => x.Lines)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ConsumptionId == consumptionId);

            if (c == null) return null;

            var dto = new ConsumptionDto
            {
                MemberId = c.MemberId,
                MemberName = c.MemberName,
                MemberPhone = c.MemberPhone ?? string.Empty,
                CreatedDate = c.CreatedDate,
                TotalBeforeDiscount = c.TotalBeforeDiscount,
                TotalAfterDiscount = c.TotalAfterDiscount,
                Lines = c.Lines.Select(l => new ConsumptionLineDto
                {
                    ProductId = l.ProductId,
                    ProductName = l.ProductName,
                    UnitPrice = l.UnitPrice,
                    Quantity = l.Quantity,
                    CouponId = l.CouponId,
                    CouponName = l.CouponName,
                    CouponMethod = l.CouponMethod,
                    CouponValue = l.CouponValue,
                    LineSubtotal = l.LineSubtotal,
                    LineTotal = l.LineTotal,
                    DiscountAmount = l.DiscountAmount
                }).ToList()
            };

            return dto;
        }

        public async Task<IEnumerable<ConsumptionDto>> GetConsumptionsByMemberAsync(int memberId)
        {
            var list = await _context.Consumptions
                .Include(x => x.Lines)
                .AsNoTracking()
                .Where(x => x.MemberId == memberId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return list.Select(c => new ConsumptionDto
            {
                MemberId = c.MemberId,
                MemberName = c.MemberName,
                MemberPhone = c.MemberPhone ?? string.Empty,
                CreatedDate = c.CreatedDate,
                TotalBeforeDiscount = c.TotalBeforeDiscount,
                TotalAfterDiscount = c.TotalAfterDiscount,
                Lines = c.Lines.Select(l => new ConsumptionLineDto
                {
                    ProductId = l.ProductId,
                    ProductName = l.ProductName,
                    UnitPrice = l.UnitPrice,
                    Quantity = l.Quantity,
                    CouponId = l.CouponId,
                    CouponName = l.CouponName,
                    CouponMethod = l.CouponMethod,
                    CouponValue = l.CouponValue,
                    LineSubtotal = l.LineSubtotal,
                    LineTotal = l.LineTotal,
                    DiscountAmount = l.DiscountAmount
                }).ToList()
            });
        }
    }
}