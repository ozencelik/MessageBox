using AutoMapper;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;

namespace MessageBox.Api.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, LogModel>();

            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
