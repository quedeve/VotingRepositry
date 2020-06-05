using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Response
{
    public class UserVoteViewModel
    {
        public int UserVoteId { get; set; }
        public int UserProfileId { get; set; }
        public int VotingId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

    }
}
