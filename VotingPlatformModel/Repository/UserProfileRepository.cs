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
    public class UserProfileRepository : IUserProfile
    {
        private VotingPlatformContext ctx;
        public UserProfileRepository(VotingPlatformContext _ctx)
        {
            ctx = _ctx;
        }

        public async Task<bool> Add<T1>(T1 Model)
        {

            UserProfile user = Model as UserProfile;
            ctx.UserProfile.Add(user);
            ctx.SaveChanges();

            return true;
            
        }

        public async Task<bool> Delete(int ID)
        {

            var userProfile = ctx.UserProfile.Where(x => x.RowStatus == true && x.UserId == ID).FirstOrDefault();
            if (userProfile != null)
            {
                userProfile.RowStatus = false;
                ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public Task<List<T1>> GetAll<T1>()
        {
            throw new NotImplementedException();
        }

        public Task<T1> GetByID<T1>(int ID)where T1:class
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> GetUserProfile(string email, string password)
        {
            
                return await ctx.UserProfile.Where(x => x.RowStatus == true && x.Email.ToLower() == email.ToLower() && x.Password == password).FirstOrDefaultAsync();
           
        }

        public async Task<UserProfile> GetUserProfile(string Email)
        {
            return await ctx.UserProfile.Where(x => x.RowStatus == true && x.Email.ToLower() == Email.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<bool> IsDuplicate(string Email)
        {

                return  ctx.UserProfile.Any(x => x.Email == Email);
            
        }

        public async Task<bool> IsDuplicate<T1>(T1 Model) where T1 : class
        {
                var request = Model as UserProfile;
                return ctx.UserProfile.Any(x => x.Email == request.Email);
  
        }

        public async Task<bool> Update<T1>(T1 Model)
        {
                var request = Model as UserProfile;
                var userProfile = ctx.UserProfile.Where(x => x.RowStatus == true && x.UserId == request.UserId).FirstOrDefault();
                if (userProfile != null)
                {
                    userProfile = request;
                    ctx.SaveChanges();
                    return true;
                }
            return false;
        }


    }
}
