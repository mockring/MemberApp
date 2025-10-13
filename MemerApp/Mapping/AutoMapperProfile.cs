using AutoMapper;
using MemerApp.Models;
using MemerApp.Dtos;

namespace MemerApp.Mapping;
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
        // ===================== 會員 =====================
            // Model → DTO
            CreateMap<MemberModel, MemberDto>();

            // DTO → Model（如果需要的話）
            CreateMap<MemberDto, MemberModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()); // CreatedDate 由 EF 產生

        // ===================== 產品 =====================
            CreateMap<ProductModel, ProductDto>();   // Model → DTO

        // DTO → Model（若需要，忽略 CreatedDate）
            CreateMap<ProductDto, ProductModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()); // CreatedDate 由 Model 建構子/EF 產生
        // ==================== 折扣券 ====================
            CreateMap<CouponModel, CouponDto>();          // Model → DTO
            CreateMap<CouponDto, CouponModel>();          // DTO → Model（無需特別忽略欄位）
        }
    }