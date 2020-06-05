using System;
using System.Collections.Generic;

namespace VotingPlatformModel.Model
{
    public partial class UserVote
    {
        public int UserVoteId { get; set; }
        public int UserProfileId { get; set; }
        public int VotingId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual Voting Voting { get; set; }
    }
}
