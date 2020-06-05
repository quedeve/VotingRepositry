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
    public class GenderRepository : IGender
    {
        VotingPlatformContext ctx;
        public GenderRepository(VotingPlatformContext _ctx)
        {
            ctx = _ctx;
        }
        public async Task<List<Gender>> GetAllGender()
        {
            return await ctx.Gender.Where(x => x.RowStatus == true).ToListAsync();
        }
    }
}
