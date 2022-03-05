using AutoMapper;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;


namespace Flutter.Backend.Service.Models.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product,DtoProduct>(MemberList.Destination)
                .ForMember(l => l.Id,opt => opt.MapFrom(x=>x.Id.ToString()));
        }
    }
}
