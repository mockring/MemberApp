using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.QueryableExtensions;
using MemberApp.Data;
using MemberApp.Dtos;
using MemberApp.Interface;
using MemberApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MemberApp.Services
{
    public class MemberService : IMemberService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MemberService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all members
        public async Task<IEnumerable<MemberDto>> GetAllAsync()
            => await _context.Members
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)      // AutoMapper 或手寫映射
                .ToListAsync();

        // Get single member
        public async Task<MemberDto?> GetByIdAsync(int id)
            => await _context.Members
                .Where(m => m.MemberId == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        // Create new member
        public async Task<bool> CreateAsync(MemberDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // Business rule: phone must be unique (example)
            if (await _context.Members.AnyAsync(m => m.Phone == dto.Phone))
                return false;   // 代表失敗，controller 再轉回 BadRequest

            var entity = new MemberModel
            {
                MemberName = dto.MemberName,
                Phone = dto.Phone,
                Birthday = dto.Birthday,
                CreatedDate = DateTime.UtcNow
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update existing member
        public async Task<bool> UpdateAsync(int id, MemberDto dto)
        {
            if (id != dto.MemberId) return false;

            var entity = await _context.Members.FindAsync(id);
            if (entity == null) return false;

            // Business rule: check phone duplication
            if (await _context.Members.AnyAsync(m => m.Phone == dto.Phone && m.MemberId != id))
                return false;

            entity.MemberName = dto.MemberName;
            entity.Phone = dto.Phone;
            entity.Birthday = dto.Birthday;
            // Keep CreatedDate unchanged

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MemberExistsAsync(id)) throw;
            }

            return true;
        }

        // Delete member
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Members.FindAsync(id);
            if (entity == null) return false;

            _context.Members.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Helper
        private async Task<bool> MemberExistsAsync(int id)
            => await _context.Members.AnyAsync(e => e.MemberId == id);
    }
}