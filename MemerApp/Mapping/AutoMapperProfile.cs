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

        // ==================== 消費記錄 ====================
            CreateMap<ConsumptionModel, ConsumptionDto>() // Model → DTO
                .ForMember(dest => dest.Lines, opt => opt.MapFrom(src => src.Lines)); // 映射 Lines 集合
            CreateMap<ConsumptionDto, ConsumptionModel>() // DTO → Model
                .ForMember(dest => dest.ConsumptionId, opt => opt.Ignore()) // ConsumptionId 由 EF 產生
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())   // CreatedDate 由 EF 產生
                .ForMember(dest => dest.Lines, opt => opt.Ignore());        // Lines 需手動處理
            CreateMap<ConsumptionLineModel, ConsumptionLineDto>(); // Model → DTO
            CreateMap<ConsumptionLineDto, ConsumptionLineModel>()   // DTO → Model
                .ForMember(dest => dest.ConsumptionLineId, opt => opt.Ignore()); // ConsumptionLineId 由 EF 產生
    }
}