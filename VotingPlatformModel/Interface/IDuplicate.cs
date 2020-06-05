using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VotingPlatformModel.Interface
{
    public interface IDuplicate
    {
        Task<bool> IsDuplicate(string Key);
        Task<bool> IsDuplicate<T1>(T1 Model)where T1: class;
    }
}
