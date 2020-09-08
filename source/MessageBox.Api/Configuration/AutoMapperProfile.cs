using AutoMapper;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;

namespace MessageBox.Api.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();

            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
