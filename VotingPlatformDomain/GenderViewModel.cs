using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain
{
    public class GenderViewModel
    {
        public short GenderId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string CreateBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModfiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
