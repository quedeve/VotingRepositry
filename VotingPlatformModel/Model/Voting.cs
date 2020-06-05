using System;
using System.Collections.Generic;

namespace VotingPlatformModel.Model
{
    public partial class Voting
    {
        public Voting()
        {
            UserVote = new HashSet<UserVote>();
        }

        public int VotingId { get; set; }
        public string VotingName { get; set; }
        public string VotingDescription { get; set; }
        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<UserVote> UserVote { get; set; }
    }
}
