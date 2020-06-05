using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Request
{
    public class CategoryRequest : BaseRequest
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
