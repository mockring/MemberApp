using MemberApp.Dtos;

namespace MemberApp.Interface
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponDto>> GetAllAsync();
        Task<CouponDto?> GetByIdAsync(int id);
        Task CreateAsync(CouponDto dto);
        Task UpdateAsync(CouponDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}