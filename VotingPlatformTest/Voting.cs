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
    public class Voting: DBVotingPlatform
    {
        private VotingFacade facade;
        private VotingRequest request;
        public Voting()
        {
            facade = new VotingFacade(connectionString);
            request = new VotingRequest();
        }

        [Fact]
        public void GetAll()
        {
            var isValid = facade.GetAll();

            Assert.True(isValid.Result.IsSuccess);
        }

        [Fact]
        public void GetAllPagination()
        {
            var isValid = facade.GetAllPagination("", 1, "1", "asc", 0, 10);

            Assert.Equal(true, isValid.Result.IsSuccess);
        }
        [Fact]
        public void GetByID()
        {
            request.CategoryID = 1;
            var isValid = facade.GetByID(request);

            Assert.True(isValid.Result.IsSuccess);
        }

        [Fact]
        public void Add()
        {
            request.VotingName = "aerosmith 3";
            request.VotingDescription = "Singer";
            request.DueDateString = "12-12-2020";
            request.CategoryID = 2;
            request.CurrentLogin = "test@gmail.com";

            var isValid = facade.Add(request);

            Assert.True(isValid.Result.IsSuccess);
        }

        [Fact]
        public void Delete()
        {
            request.VotingID = 18;
           

            var isValid = facade.Delete(request);

            Assert.True(isValid.Result.IsSuccess);
        }

        [Fact]
        public void Update()
        {
            request.VotingID = 17;
            request.VotingName = "aerosmith 4-2";
            request.VotingDescription = "Singer";
            request.DueDateString = "12-12-2020";
            request.CategoryID = 2;
            request.CurrentLogin = "test@gmail.com";

            var isValid = facade.Add(request);

            Assert.True(isValid.Result.IsSuccess);
        }
    }
}
