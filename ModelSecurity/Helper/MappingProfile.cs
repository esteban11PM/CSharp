using AutoMapper;
using Entity.DTOs;
using Entity.Model;

namespace Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<Rol, RolDTO>().ReverseMap();
            CreateMap<Form, FormDTO>().ReverseMap();
            CreateMap<Module, ModuleDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();

            // CreateMap<User, UserCreateDTO>().ReverseMap();
            
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person.Name))
                .ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();

            CreateMap<FormModule, FormModuleDTO>()
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form.Name))
                .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.Module.Name))
                .ReverseMap();
            CreateMap<FormModule, FormModuleCreateDTO>().ReverseMap();

            CreateMap<RolUser, RolUserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ReverseMap();
            CreateMap<RolUser, RolUserCreateDTO>().ReverseMap();

            CreateMap<RolFormPermission, RolFormPermissionDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Rol.Name))
                .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name))
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form.Name))
                .ReverseMap();
            CreateMap<RolFormPermission, RolFormPermissionCreateDTO>().ReverseMap();
        }
    }
}


