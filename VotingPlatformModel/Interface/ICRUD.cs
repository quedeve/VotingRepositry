using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VotingPlatformModel.Interface
{
    public interface ICRUD
    {
        Task<List<T1>> GetAll<T1>();
        Task<bool> Add<T1>(T1 Model);
        Task<T1> GetByID<T1>(int ID)where T1:class;
        Task<bool> Delete(int ID);
        Task<bool> Update<T1>(T1 Model);

    }
}
