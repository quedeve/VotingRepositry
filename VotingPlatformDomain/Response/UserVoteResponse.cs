using System;
using System.Collections.Generic;
using System.Text;


namespace VotingPlatformDomain.Response
{
    public class UserVoteResponse:BaseResponse
    {
        public List<UserVoteViewModel> ListUserVote { get; set; }
        public UserVoteViewModel UserVote { get; set; }

        public UserVoteResponse()
        {
            UserVote = new UserVoteViewModel();
            ListUserVote = new List<UserVoteViewModel>();
        }
    }
}
