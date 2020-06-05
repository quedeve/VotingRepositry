using System;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VotingPlatformDomain;
using VotingPlatformDomain.Request;
using VotingPlatformFacade;
using VotingPlatformModel.Model;
using Xunit;

namespace VotingPlatformTest
{
    public class UserProfile: DBVotingPlatform
    {
        UserProfileFacade facade;
        UserProfileRequest request;
        public UserProfile()
        {
            facade = new UserProfileFacade(connectionString, kunciRahasiaku);
            request = new UserProfileRequest();
        }
        [Fact]
        public void Add()
        {
            
            request.Email = "Om@boy.simamora";
            request.Password ="Omboy123";
            request.FirstName = "Boy";
            request.LastName = "Simamora";
            request.Gender = 1;
            request.Age = 24;
            request.RoleID = 1;
            request.CurrentLogin = "test@gmail.com";
            var isValid = facade.Add(request);

            Assert.True(isValid.Result.IsSuccess);
        }
        [Fact]
        public void Authenticate()
        {
            request.Email = "Om@boy.simamora";
            request.Password = "Omboy123";

            var isValid = facade.Authenticate(request);

            Assert.True(isValid.Result.IsSuccess);
        }
        [Fact]
        public void Register()
        {
            request.Email = "victor@boy.simamora";
            request.Password = "Omboy123";
            request.FirstName = "Boy";
            request.LastName = "Simamora";
            request.Gender = 1;
            request.Age = 24;
            request.Role = "Admin";

            var isValid = facade.Register(request);

            Assert.True(isValid.Result.IsSuccess);
        }
        [Fact]
        public void GetByMail()
        {
            var isValid =facade.GetByEmail("victor@boy.simamora");

            Assert.True(isValid.Result.IsSuccess);
        }
    }
}
