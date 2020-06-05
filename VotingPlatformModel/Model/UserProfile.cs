using System;
using System.Collections.Generic;

namespace VotingPlatformModel.Model
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            UserVote = new HashSet<UserVote>();
        }

        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short GenderId { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserVote> UserVote { get; set; }
    }
}
