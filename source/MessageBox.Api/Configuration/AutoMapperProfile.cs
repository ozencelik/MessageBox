using AutoMapper;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;

namespace MessageBox.Api.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region UserMappers
            CreateMap<User, UserModel>();

            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            #endregion

            #region LogMappers
            CreateMap<Log, LogModel>();
            #endregion
        }
    }
}
