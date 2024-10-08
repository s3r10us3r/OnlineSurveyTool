﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Utils;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper, ILogger<BaseService> logger) : base(logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IResult<UserDTO, UserCreationFailure>> CreateUser(UserRegisterDTO userDTO)
        {
            var validationResult = ValidateUserRegisterDTO(userDTO, out var validationMessage);
            if (!validationResult)
            {
                return Result<UserDTO, UserCreationFailure>.Failure(validationMessage, UserCreationFailure.InvalidData);
            }

            if (await _userRepo.GetOne(userDTO.Login) is not null)
            {
                return Result<UserDTO, UserCreationFailure>.Failure("User with this login already exists!", UserCreationFailure.NameConflict);
            }
            
            var user = _mapper.Map<User>(userDTO);
            
            var passwordHash = HashPassword(userDTO.Password);
            user.PasswordHash = passwordHash;
            try
            {
                await _userRepo.Add(user);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error occured while adding user to db.");
                throw new Exception("Error adding user to the database.", e);
            }

            Logger.LogInformation("User {uId}, {uLogin} has been added to the database", user.Id, user.Login);
            return Result<UserDTO, UserCreationFailure>.Success(new UserDTO { Id = user.Id, Login = user.Login, Email = user.EMail });
        }

        public async Task<User?> GetUserFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            var login = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (login is null)
            {
                return null;
            }

            var user = await _userRepo.GetOne(login);
            return user;
        }

        
        public async Task<bool> CheckIfLoginExists(string login)
        {
            return await _userRepo.GetOne(login) is not null;
        }

        public async Task<User?> GetUserFromIdentityClaims(ClaimsIdentity claimsIdentity)
        {
            var userLogin = claimsIdentity.FindFirst("sub")?.Value;
            return userLogin is null ? null : await _userRepo.GetOne(userLogin);
        }

        private bool ValidateUserRegisterDTO(UserRegisterDTO userRegisterDTO, out string message)
        {
            if (!Validator.ValidateLogin(userRegisterDTO.Login, out message))
            {
                return false;
            }
            if (!Validator.ValidateEMail(userRegisterDTO.EMail, out message))
            {
                return false;
            }
            if (!Validator.ValidatePassword(userRegisterDTO.Password, out message))
            {
                return false;
            }

            message = "";
            return true;
        }

        private string HashPassword(string password)
        {
            string salt = Crypt.GenerateSalt();
            string hashedPassword = Crypt.HashPassword(password, salt);
            return hashedPassword;
        }
    }

    public enum UserCreationFailure
    {
        InvalidData, NameConflict
    }
}
