using AutoMapper;
using RoofstockFullStackSample.Models;

namespace RoofstockFullStackSample
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PropertyRawModel, Property>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address.address1 + ", " + src.address.city + ", " + src.address.country))
                .ForMember(dest => dest.YearBuilt, opt => opt.MapFrom(src => src.physical.yearBuilt))
                .ForMember(dest => dest.ListPrice, opt => opt.MapFrom(src => src.financial.listPrice))
                .ForMember(dest => dest.MonthlyRent, opt => opt.MapFrom(src => src.financial.monthlyRent));
        }
    }
}
