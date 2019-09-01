using AutoMapper;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement
{
    public class AutoMapperInitializer : Profile
    {
        public AutoMapperInitializer()
        {
            #region Poco => Poco

            CreateMap<Group, Group>()
                .ForMember(x => x.GroupInRole, opt => opt.Ignore())
                .ForMember(x => x.UserInGroup, opt => opt.Ignore());
            CreateMap<GroupInRole, GroupInRole>()
                .ForMember(x => x.Group, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore());
            CreateMap<Role, Role>()
                .ForMember(x => x.GroupInRole, opt => opt.Ignore())
                .ForMember(x => x.UserInRole, opt => opt.Ignore());
            CreateMap<Token, Token>()
                .ForMember(x => x.User, opt => opt.Ignore());
            CreateMap<User, User>()
                .ForMember(x => x.Token, opt => opt.Ignore())
                .ForMember(x => x.UserInRole, opt => opt.Ignore())
                .ForMember(x => x.UserInGroup, opt => opt.Ignore());
            CreateMap<UserInGroup, UserInGroup>()
                .ForMember(x => x.Group, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());
            CreateMap<UserInRole, UserInRole>()
                .ForMember(x => x.Role, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());
            CreateMap<UserMessage, UserMessage>()
                .ForMember(x => x.FromUser, opt => opt.Ignore())
                .ForMember(x => x.ToUser, opt => opt.Ignore());

            #endregion Poco => Poco

            #region DTO => DTO

            CreateMap<GroupDTO, GroupDTO>();
            CreateMap<GroupInRoleDTO, GroupInRoleDTO>();
            CreateMap<RoleDTO, RoleDTO>();
            CreateMap<TokenDTO, TokenDTO>();
            CreateMap<UserDTO, UserDTO>();
            CreateMap<UserInGroupDTO, UserInGroupDTO>();
            CreateMap<UserInRoleDTO, UserInRoleDTO>();
            CreateMap<UserMessageDTO, UserMessageDTO>();

            #endregion DTO => DTO

            #region Poco => DTO & DTO => Poco

            CreateMap<Group, GroupDTO>().ReverseMap();
            CreateMap<GroupInRole, GroupInRoleDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Token, TokenDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserInGroup, UserInGroupDTO>().ReverseMap();
            CreateMap<UserInRole, UserInRoleDTO>().ReverseMap();
            CreateMap<UserMessage, UserMessageDTO>().ReverseMap();

            #endregion Poco => DTO & DTO => Poco

            #region Other

            #region UserForCurrentUser

            CreateMap<User, UserForCurrentUser>()
               .ForMember(x => x.LastMessage, opt => opt.Ignore())
               .ForMember(x => x.Messages, opt => opt.Ignore());
            CreateMap<UserForCurrentUser, User>().ReverseMap();
            CreateMap<UserDTO, UserForCurrentUser>()
               .ForMember(x => x.LastMessage, opt => opt.Ignore())
               .ForMember(x => x.Messages, opt => opt.Ignore());
            CreateMap<UserForCurrentUser, UserDTO>().ReverseMap();

            #endregion UserForCurrentUser

            #region GroupDTO > UserGroupDetailsModel

            CreateMap<GroupDTO, UserGroupDetailsModel>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.Users, opt => opt.Ignore());

            #endregion GroupDTO > UserGroupDetailsModel

            #endregion Other
        }
    }
}