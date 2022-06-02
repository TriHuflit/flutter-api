using AutoMapper;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using MongoDB.Bson;

namespace Flutter.Backend.Service.Models.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, DtoProduct>(MemberList.Destination)
                .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(l => l.CategoryId, opt => opt.MapFrom(x => x.CategoryId.ToString()))
                .ForMember(l => l.BrandId, opt => opt.MapFrom(x => x.BrandId.ToString()));

            CreateMap<Product, DtoProductDetail>(MemberList.Destination)
               .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()))
               .ForMember(l => l.CategoryId, opt => opt.MapFrom(x => x.CategoryId.ToString()))
               .ForMember(l => l.BrandId, opt => opt.MapFrom(x => x.BrandId.ToString()));

            CreateMap<ClassifyProduct, DtoClassifyProduct>(MemberList.Destination)
               .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()))
               .ForMember(l => l.ProductId, opt => opt.MapFrom(x => x.ProductId.ToString()));

            CreateMap<Category, DtoCategory>(MemberList.Destination)
                .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()));

            CreateMap<Brand, DtoBrand>(MemberList.Destination)
                .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()));

            CreateMap<Order, DtoOrder>(MemberList.Destination)
                .ForMember(l => l.Id, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(l => l.UserId, opt => opt.MapFrom(x => x.UserId.ToString()))
                .ForMember(l => l.VoucherId, opt => opt.MapFrom(x => x.VoucherId.ToString()));



            CreateMap<OrderDetail, DtoOrderDetail>(MemberList.Destination)
                .ForMember(l => l.ClassifyProductId, opt => opt.MapFrom(x => x.ClassifyProductId.ToString()));

            CreateMap<CreateOrderDetailRequest, OrderDetail>(MemberList.Destination)
                .ForMember(l => l.ClassifyProductId, opt => opt.MapFrom(x => ObjectId.Parse(x.ClassifyProductId)));
        }
    }
}
