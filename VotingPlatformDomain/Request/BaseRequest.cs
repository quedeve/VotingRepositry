using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Request
{
    public class BaseRequest
    {
        public string Token { get; set; }
        public string CurrentLogin { get; set; }
    }
}
