using AutoMapper;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;

namespace MessageBox.Api.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region User
            CreateMap<User, UserModel>();

            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            #endregion

            #region Log
            CreateMap<Log, LogModel>();
            #endregion

            #region Message
            CreateMap<Message, MessageModel>();
            #endregion
        }
    }
}
