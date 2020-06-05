using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Interface
{
    public interface IRole
    {
        Task<Role> GetRoleByName(string role);
        Task<Role> GetRoleByEmail(string email);
    }
}
