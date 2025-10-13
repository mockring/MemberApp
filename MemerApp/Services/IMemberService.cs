using MemerApp.Dtos;

namespace MemerApp.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllAsync();
        Task<MemberDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(MemberDto dto);
        Task<bool> UpdateAsync(int id, MemberDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
