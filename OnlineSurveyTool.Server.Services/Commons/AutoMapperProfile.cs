using AutoMapper;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

namespace OnlineSurveyTool.Server.Services.Commons
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserRegisterDTO, User>();
            CreateMap<ChoiceOptionDTO, ChoiceOption>();
        }
    }
}
