using AutoMapper;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.DTOs;

namespace OnlineSurveyTool.Server.Services.Commons
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserRegisterDTO, User>();
        }
    }
}
