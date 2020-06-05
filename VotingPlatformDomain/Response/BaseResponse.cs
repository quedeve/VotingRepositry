using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Response
{
    public class BaseResponse
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsLogin { get; set; }
        public BaseResponse()
        {
            IsSuccess = true;
            Message = "Success";
            IsLogin = true;
        }
    }
}
