using AutoMapper;
using Potato.DbContext.Models.Entity;
using Potato.ViewModels.Account;

namespace Potato.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, User>()
        /*.ForMember(x => x.UserName, opt => opt.MapFrom(c => c.FirstName))*/
        .ForMember(x => x.Email, opt => opt.MapFrom(c => c.EmailReg));

            CreateMap<LoginViewModel, User>();
            /*CreateMap<LoginViewModel, User>()
        .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email));
            CreateMap<UserEditViewModel, User>().ReverseMap();*/
            CreateMap<User, UserWithFriendExt>().ReverseMap();
        }
    }
}
