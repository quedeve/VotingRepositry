using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain
{
    public class VotingViewModel
    {
        public int VotingId { get; set; }
        public string VotingName { get; set; }
        public string VotingDescription { get; set; }
        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public int  SupportersCount { get; set; }

    }
}
