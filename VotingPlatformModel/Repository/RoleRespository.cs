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
    public class RoleRespository : IRole
    {
        private VotingPlatformContext ctx;
        public RoleRespository(VotingPlatformContext _ctx)
        {
            ctx = _ctx;
        }

        public async Task<Role> GetRoleByEmail(string email)
        {
            return await (from role in ctx.Role
                          join usrProfile in ctx.UserProfile
                          on role.RoleId equals usrProfile.RoleId
                          where role.RowStatus == true && usrProfile.RowStatus == true && usrProfile.Email.ToLower() == email.ToLower()
                          select role).FirstOrDefaultAsync();
        }

        public async Task<Role> GetRoleByName(string role)
        {
            return await ctx.Role.Where(x => x.RowStatus == true && x.Name.ToLower().Equals(role.ToLower())).FirstAsync();
        }
    }
}
