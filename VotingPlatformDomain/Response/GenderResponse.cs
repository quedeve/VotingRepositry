using System;
using System.Collections.Generic;


namespace VotingPlatformDomain.Response
{
    public class GenderResponse : BaseResponse
    {
        public List<GenderViewModel> ListGender { get; set; }
        public GenderResponse()
        {
            ListGender = new List<GenderViewModel>();
        }
    }
}
