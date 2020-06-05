using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingPlatformDomain;
using VotingPlatformModel.Interface;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Repository
{
    public class VotingRepository : IVoting
    {
        private VotingPlatformContext ctx;
        public VotingRepository(VotingPlatformContext _ctx)
        {
            ctx = _ctx;
        }
        public async Task<bool> Add<T1>(T1 Model)
        {

                var voting = Model as Voting;
                ctx.Voting.Add(voting);
                ctx.SaveChanges();
                return true;

        }

        public async Task<bool> Delete(int ID)
        {

                var voting = ctx.Voting.Where(x => x.RowStatus == true && x.VotingId == ID).FirstOrDefault();
                if (voting !=null)
                {
                    voting.RowStatus = false;
                    ctx.SaveChanges();
                    return true;
                }

            return false;
        }

        public async Task<List<T1>> GetAll<T1>()
        {
  
                return await ctx.Voting.Where(x => x.RowStatus == true).ToListAsync() as List<T1>;

        }

        public async Task<IQueryable<VotingViewModel>> GetAll()
        {
            return (from voting in ctx.Voting
                    join category in ctx.Category
                    on voting.CategoryId equals category.CategoryId
                    where voting.RowStatus == true && category.RowStatus == true
                    select new VotingViewModel
                    {
                        VotingId = voting.VotingId,
                        VotingName = voting.VotingName,
                        VotingDescription = voting.VotingDescription,
                        CategoryId = voting.CategoryId,
                        CategoryName = category.CategoryName,
                        Created = voting.Created,
                        CreatedBy = voting.CreatedBy,
                        DueDate = voting.DueDate,
                        SupportersCount = (from userVote in ctx.UserVote
                                           where userVote.RowStatus == true && userVote.VotingId == voting.VotingId
                                           select userVote.UserVoteId).Count()
                    });
        }

        public async Task<T1> GetByID<T1>(int ID) where T1 : class
        {

                return await ctx.Voting.Where(x => x.RowStatus == true && x.VotingId == ID).Select(x=>new VotingViewModel
                {
                    VotingId = x.VotingId,
                    VotingName = x.VotingName,
                    VotingDescription = x.VotingDescription,
                    CategoryId = x.CategoryId,
                    Created = x.Created,
                    DueDate = x.DueDate
                }).FirstOrDefaultAsync() as T1;

        }

        public async Task<bool> Update<T1>(T1 Model)
        {

            var request = Model as Voting;
            var voting = ctx.Voting.Where(x => x.RowStatus == true && x.VotingId == request.VotingId).FirstOrDefault();
            if (voting != null)
            {
                voting.VotingName = request.VotingName;
                voting.VotingDescription = request.VotingDescription;
                voting.CategoryId = request.CategoryId;
                voting.DueDate = request.DueDate;
                ctx.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
