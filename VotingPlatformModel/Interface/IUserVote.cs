using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VotingPlatformModel.Interface
{
    public interface IUserVote : ICRUD
    {
        Task<bool> DuplicateVote(int id, string email);
        Task<bool> IsVoteExpired(int id);
    }
}
