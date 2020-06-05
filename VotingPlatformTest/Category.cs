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
    public class DBVotingPlatform
    {
        public string connectionString { get; set; }
        public string kunciRahasiaku { get; set; }
        public DBVotingPlatform()
        {
            connectionString = "Server=GONNA-BE-GOOD\\SQLEXPRESS_2017;Database=VotingPlatform;Trusted_Connection=True;";
            kunciRahasiaku = "a3TeQa5On2XypwmQ";

        }

    }
    public class Category : DBVotingPlatform
    {
        CategoryFacade facade;
        CategoryRequest request;
        public Category()
        {
            facade = new CategoryFacade(connectionString);
            request = new CategoryRequest();
        }
        
        [Fact]
        public void GetAll()
        {
            var isValid = facade.GetAll();

            Assert.Equal(true, isValid.Result.IsSuccess);
        }
        [Fact]
        public void Add()
        {
            request.CategoryID = 0;
            request.CategoryName = "Gadget";
            request.CurrentLogin = "test@gmail.com";
            request.Description = "Gadget Mania";

            var isValid = facade.Add(request);

            Assert.Equal(true, isValid.Result.IsSuccess);
        }

        [Fact]
        public void Update()
        {
            request.CategoryID = 7;
            request.CategoryName = "Gadget2";
            request.CurrentLogin = "test@gmail.com";
            request.Description = "Gadget Mania Update";

            var isValid = facade.Update(request);

            Assert.Equal(true, isValid.Result.IsSuccess);

        }

        [Fact]
        public void Delete()
        {
            request.CategoryID = 2;
            var isValid = facade.Delete(request);

            Assert.Equal(true, isValid.Result.IsSuccess);


        }

        [Fact]
        public void GetAllPagination()
        {
            var isValid =facade.GetAllPagination("", 1, "1", "asc", 0, 10);

            Assert.Equal(true, isValid.Result.IsSuccess);
        }
    }
}
