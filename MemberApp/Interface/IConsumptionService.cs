using MemberApp.Dtos;
using System.Threading.Tasks;

namespace MemberApp.Interface
{
    public interface IConsumptionService
    {
        Task<IEnumerable<ConsumptionDto>> GetAllAsync();
        Task<ConsumptionDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, ConsumptionDto dto);
        Task<ConsumptionDto> CreateConsumptionAsync(ConsumptionDto dto);
        Task<ConsumptionDto?> GetConsumptionAsync(int consumptionId);
        Task<IEnumerable<ConsumptionDto>> GetConsumptionsByMemberAsync(int memberId);
        Task<MemberDto?> FindMemberByPhoneAsync(string phone);
        Task<IEnumerable<CouponDto>> GetAllCouponsAsync();
        Task<IEnumerable<ProductDto>> GetProductsByKeywordAsync(string keyword);
        Task<ProductDto?> GetProductByIdAsync(int productId);
    }
}