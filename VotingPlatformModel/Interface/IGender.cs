using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Interface
{
    public interface IGender
    {
        Task<List<Gender>> GetAllGender();
    }
}
