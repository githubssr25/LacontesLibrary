using AutoMapper;
using Models;
using DTOs;

namespace Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mappings for base models and DTOs
        CreateMap<Genre, GenreDTO>();
        CreateMap<MaterialType, MaterialTypeDTO>();
        CreateMap<Patron, PatronDTO>();
       
       
        CreateMap<Checkout, CheckoutDTO>()
            .ForMember(dest => dest.PatronDTO, opt => opt.MapFrom(src => src.Patron)); // Map Patron to PatronDTO in CheckoutDTO
            
        // Mapping for Material and MaterialDTO
        CreateMap<Material, MaterialDTO>()
            .ForMember(dest => dest.GenreDTO, opt => opt.MapFrom(src => src.Genre)) // Map Genre to GenreDTO
            .ForMember(dest => dest.MaterialTypeDTO, opt => opt.MapFrom(src => src.MaterialType)) // Map MaterialType to MaterialTypeDTO
            .ForMember(dest => dest.Checkouts, opt => opt.Ignore()); // Ignore Checkouts for MaterialDTO as they are optional

// This btw is how you can make the navprop optional for mapping 
// CreateMap<Material, MaterialDTO>()
//     .ForMember(dest => dest.GenreDTO, opt => opt.Condition(src => src.Genre != null))
//     .ForMember(dest => dest.MaterialTypeDTO, opt => opt.Condition(src => src.MaterialType != null))
//     .ForMember(dest => dest.Checkouts, opt => opt.Ignore());






        // Mapping for SpecialMaterialDTO
        CreateMap<Material, SpecialMaterialDTO>()
            .ForMember(dest => dest.GenreDTO, opt => opt.MapFrom(src => src.Genre)) // Map Genre to GenreDTO
            .ForMember(dest => dest.MaterialTypeDTO, opt => opt.MapFrom(src => src.MaterialType)) // Map MaterialType to MaterialTypeDTO
            .ForMember(dest => dest.Checkouts, opt => opt.MapFrom(src => src.Checkouts)); // Map Checkouts to a collection of CheckoutDTO


    //NOTE we dont need to do this strictm apping with for member im doing it for practice though for this one 
    CreateMap<CreateMaterialDTO, Material>()
    .ForMember(dest => dest.MaterialTypeId, opt => opt.MapFrom(src => src.MaterialTypeId))
    .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId));
    }
}
