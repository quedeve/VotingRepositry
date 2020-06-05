using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingPlatformDomain.Request;
using VotingPlatformDomain.Response;
using VotingPlatformModel.Interface;
using VotingPlatformModel.Model;
using VotingPlatformModel.Repository;

namespace VotingPlatformFacade
{
    public class UserVoteFacade
    {
        private VotingPlatformContext ctx;
        private IUserVote iUserVote;
        private IUserProfile iUserProfile;
        private IVoting iVoting;
        public UserVoteFacade(string connectionString)
        {
            var optionBuilder = new DbContextOptionsBuilder<VotingPlatformContext>();
            optionBuilder.UseSqlServer(connectionString);

            ctx = new VotingPlatformContext(optionBuilder.Options);
            this.iUserVote = new UserVoteRepository(ctx);
            this.iUserProfile = new UserProfileRepository(ctx);
            
        }

        public async Task<UserVoteResponse> Add(UserVoteRequest request)
        {
            UserVoteResponse response = new UserVoteResponse();
            try
            {
                if (!await iUserVote.DuplicateVote(request.VotingID, request.CurrentLogin))
                {
                    if (! await iUserVote.IsVoteExpired(request.VotingID))
                    {
                        UserVote userVote = new UserVote();

                        userVote.UserProfileId = (await iUserProfile.GetUserProfile(request.CurrentLogin)).UserId;
                        userVote.VotingId = request.VotingID;
                        userVote.Created = DateTime.Now;
                        userVote.CreatedBy = request.CurrentLogin;
                        userVote.RowStatus = true;
                        if (await iUserVote.Add<UserVote>(userVote))
                        {
                            return response;
                        }
                        response.IsSuccess = false;
                        response.Message = "Failed to Add";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Vote time has been expired!!!";
                    }
                    
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Cannot Vote this item more than 1 time";
                }
               
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something Error in Our System : " + ex.Message;
            }
            return response;
        }

        //public async Task<UserVoteResponse> Delete(UserVoteRequest request)
        //{
        //    UserVoteResponse response = new UserVoteResponse();
        //    try
        //    {
        //        if (await iUserVote.Delete(request.UserVoteID))
        //        {
        //            return response;
        //        }
        //        response.IsSuccess = false;
        //        response.Message = "Failed to Delete Vote";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Something Error in Our System : " + ex.Message;
        //    }
        //    return response;
        //}
    }
}
