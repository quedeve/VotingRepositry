using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Interface
{
    public interface ICategory:ICRUD,IDuplicate
    {
        Task<IQueryable<Category>> GetAll();
    }
}
