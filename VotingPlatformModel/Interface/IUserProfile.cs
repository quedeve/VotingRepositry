using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Interface
{
    public interface IUserProfile : ICRUD, IDuplicate
    {
        Task<UserProfile> GetUserProfile(string Email, String Password);
        Task<UserProfile> GetUserProfile(string Email);
    }
}
