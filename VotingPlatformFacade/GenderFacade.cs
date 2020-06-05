using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingPlatformDomain;
using VotingPlatformDomain.Response;
using VotingPlatformModel.Interface;
using VotingPlatformModel.Model;
using VotingPlatformModel.Repository;

namespace VotingPlatformFacade
{
    public class GenderFacade
    {
        private VotingPlatformContext ctx;
        private IGender iGender;
        public GenderFacade(string connectionString)
        {
            var optionBuilder = new DbContextOptionsBuilder<VotingPlatformContext>();
            optionBuilder.UseSqlServer(connectionString);

            ctx = new VotingPlatformContext(optionBuilder.Options);
            this.iGender = new GenderRepository(ctx);
        }
        

        public async Task<GenderResponse> GetAllGender()
        {
            GenderResponse response = new GenderResponse();
            try
            {
                var query = await iGender.GetAllGender();
                response.ListGender = (from q in query
                                       select new GenderViewModel
                                       {
                                           GenderId = q.GenderId,
                                           Name = q.Name
                                       }).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something Error in Our System : " + ex.Message;
            }
            return response;
        }
        
    }
}
