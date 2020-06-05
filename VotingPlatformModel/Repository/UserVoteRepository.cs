using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingPlatformModel.Interface;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Repository
{
    public class UserVoteRepository : IUserVote
    {
        private VotingPlatformContext ctx;
        public UserVoteRepository(VotingPlatformContext _ctx)
        {
            ctx = _ctx;
        }
        public async Task<bool> Add<T1>(T1 Model)
        {

            var request = Model as UserVote;
            ctx.UserVote.Add(request);
            ctx.SaveChanges();
            return true;


        }

        public async Task<bool> Delete(int ID)
        {
  
                var userVote = ctx.UserVote.Where(x => x.RowStatus == true && x.UserVoteId == ID).FirstOrDefault();
                if (userVote != null)
                {
                    ctx.UserVote.Add(userVote);
                    ctx.SaveChanges();
                    return true;
                }

            return false;
        }

        public async Task<bool> DuplicateVote(int id, string email)
        {
            return (from userVote in ctx.UserVote
                    join userProfile in ctx.UserProfile
                    on userVote.UserProfileId equals userProfile.UserId
                    where userProfile.RowStatus == true && userVote.RowStatus == true
                    && userVote.VotingId == id && userProfile.Email.ToLower() == email.ToLower()
                    select userVote).Count() > 0 ? true : false;
        }

        public Task<List<T1>> GetAll<T1>()
        {
            throw new NotImplementedException();
        }

        public async Task<T1> GetByID<T1>(int ID) where T1 : class
        {
 
            return await ctx.UserVote.Where(x => x.RowStatus == true && x.UserVoteId == ID).FirstOrDefaultAsync() as T1;

        }

        public async Task<bool> IsVoteExpired(int id)
        {
            DateTime now = DateTime.Now;
            return ctx.Voting.Any(x => x.VotingId == id && x.DueDate.Date < now.Date);
        }

        public Task<bool> Update<T1>(T1 Model)
        {
            throw new NotImplementedException();
        }
    }
}
