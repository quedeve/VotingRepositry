using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingPlatformDomain;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Interface
{
    public interface IVoting :ICRUD
    {
        Task<IQueryable<VotingViewModel>> GetAll();
    }
}
