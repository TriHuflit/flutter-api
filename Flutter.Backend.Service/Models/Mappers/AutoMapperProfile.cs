using AutoMapper;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;


namespace Flutter.Backend.Service.Models.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, DtoProduct>(MemberList.Destination)
                .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(l => l.CategoryID, opt => opt.MapFrom(x => x.CategoryId.ToString()))
                .ForMember(l => l.BrandID, opt => opt.MapFrom(x => x.BrandId.ToString()));

            CreateMap<Product, DtoProductDetail>(MemberList.Destination)
               .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()))
               .ForMember(l => l.CategoryID, opt => opt.MapFrom(x => x.CategoryId.ToString()))
               .ForMember(l => l.WaterProofId, opt => opt.MapFrom(x => x.WaterProofId.ToString()))
               .ForMember(l => l.BrandID, opt => opt.MapFrom(x => x.BrandId.ToString()));

            CreateMap<ClassifyProduct, DtoClassifyProduct>(MemberList.Destination)
               .ForMember(l => l.ClassifyProductId, opt => opt.MapFrom(x => x.Id.ToString()))
               .ForMember(l => l.ProductId, opt => opt.MapFrom(x => x.ProductId.ToString()));
        }
    }
}
