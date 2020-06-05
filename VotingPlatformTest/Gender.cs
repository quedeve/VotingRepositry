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
    public class Gender : DBVotingPlatform
    {
        private GenderFacade facade;
        public Gender()
        {
            facade = new GenderFacade(connectionString);
        }
        [Fact]
        public void GetAllGender()
        {
            var isValid = facade.GetAllGender();

            Assert.True(isValid.Result.IsSuccess);
        }
    }
}
