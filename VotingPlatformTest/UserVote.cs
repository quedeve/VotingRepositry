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
    public class UserVote:DBVotingPlatform
    {
        private UserVoteFacade facade;
        private UserVoteRequest request;
        public UserVote()
        {
            facade = new UserVoteFacade(connectionString);
            request = new UserVoteRequest();
        }
        [Fact]
        public void Add()
        {
            request.UserProfileID = 1003;
            request.VotingID = 8;
            request.CurrentLogin = "victor@info.com";
            var isValid = facade.Add(request);

            Assert.True(isValid.Result.IsSuccess);
        }
        [Fact]
        public void AddExpiredVoting()
        {
            request.UserProfileID = 1003;
            request.VotingID = 18;
            request.CurrentLogin = "victor@info.com";
            var isValid = facade.Add(request);

            Assert.False(isValid.Result.IsSuccess);
        }

    }
}
