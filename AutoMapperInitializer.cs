using AutoMapper;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement {
    public class AutoMapperInitializer : Profile {
        public AutoMapperInitializer () {
            #region Poco => Poco

            CreateMap<Group, Group> ();
            CreateMap<GroupInRole, GroupInRole> ();
            CreateMap<Role, Role> ();
            CreateMap<Token, Token> ();
            CreateMap<User, User> ();
            CreateMap<UserInGroup, UserInGroup> ();
            CreateMap<UserInRole, UserInRole> ();
            CreateMap<UserMessage, UserMessage> ();

            #endregion Poco => Poco

            #region DTO => DTO

            CreateMap<GroupDTO, GroupDTO> ();
            CreateMap<GroupInRoleDTO, GroupInRoleDTO> ();
            CreateMap<RoleDTO, RoleDTO> ();
            CreateMap<TokenDTO, TokenDTO> ();
            CreateMap<UserDTO, UserDTO> ();
            CreateMap<UserInGroupDTO, UserInGroupDTO> ();
            CreateMap<UserInRoleDTO, UserInRoleDTO> ();

            #endregion DTO => DTO

            #region Poco => DTO & DTO => Poco

            CreateMap<Group, GroupDTO> ().ReverseMap ();
            CreateMap<GroupInRole, GroupInRoleDTO> ().ReverseMap ();
            CreateMap<Role, RoleDTO> ().ReverseMap ();
            CreateMap<Token, TokenDTO> ().ReverseMap ();
            CreateMap<User, UserDTO> ().ReverseMap ();
            CreateMap<UserInGroup, UserInGroupDTO> ().ReverseMap ();
            CreateMap<UserInRole, UserInRoleDTO> ().ReverseMap ();

            #endregion Poco => DTO & DTO => Poco

            #region Other

            #region GroupDTO > UserGroupDetailsModel

            CreateMap<GroupDTO, UserGroupDetailsModel> ()
                .ForMember (x => x.Roles, opt => opt.Ignore ())
                .ForMember (x => x.Users, opt => opt.Ignore ());

            #endregion GroupDTO > UserGroupDetailsModel

            #endregion Other
        }
    }
}