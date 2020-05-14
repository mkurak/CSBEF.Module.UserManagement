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

            CreateMap<Group, Group>();
            CreateMap<GroupInRole, GroupInRole>();
            CreateMap<Role, Role>();
            CreateMap<Token, Token>();
            CreateMap<User, User>();
            CreateMap<UserInGroup, UserInGroup>();
            CreateMap<UserInRole, UserInRole>();
            CreateMap<UserMessage, UserMessage>();

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