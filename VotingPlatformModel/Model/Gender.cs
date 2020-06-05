using System;
using System.Collections.Generic;

namespace VotingPlatformModel.Model
{
    public partial class Gender
    {
        public Gender()
        {
            UserProfile = new HashSet<UserProfile>();
        }

        public short GenderId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string CreateBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModfiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
}
