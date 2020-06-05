using System;
using System.Collections.Generic;

namespace VotingPlatformModel.Model
{
    public partial class Role
    {
        public Role()
        {
            UserProfile = new HashSet<UserProfile>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
}
