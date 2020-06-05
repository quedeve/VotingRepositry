using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Request
{
    public class UserVoteRequest:BaseRequest
    {
        public int UserVoteID { get; set; }
        public int UserProfileID { get; set; }
        public int VotingID { get; set; }
        
    }
}
