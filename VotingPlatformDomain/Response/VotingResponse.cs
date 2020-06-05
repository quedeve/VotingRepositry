using System;
using System.Collections.Generic;
using System.Text;
using VotingPlatformDomain.Response.DataTable;

namespace VotingPlatformDomain.Response
{
    public class VotingResponse: BaseResponse
    {
        public List<VotingViewModel> ListVoting { get; set; }
        public VotingViewModel Voting { get; set; }

        public VotingResponse()
        {
            ListVoting = new List<VotingViewModel>();
            Voting = new VotingViewModel();
        }
    }

    public class VotingDTResponse: DTBaseResponse
    {
        public List<VotingViewModel> ListVoting { get; set; }
        public VotingViewModel Voting { get; set; }

        public VotingDTResponse()
        {
            ListVoting = new List<VotingViewModel>();
            Voting = new VotingViewModel();
        }

    }
}
