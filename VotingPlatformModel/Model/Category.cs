using System;
using System.Collections.Generic;

namespace VotingPlatformModel.Model
{
    public partial class Category
    {
        public Category()
        {
            Voting = new HashSet<Voting>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public virtual ICollection<Voting> Voting { get; set; }
    }
}
