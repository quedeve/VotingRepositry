using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Response.DataTable
{
    public class DTBaseResponse
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsLogin { get; set; }
        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }
        public DTBaseResponse()
        {
            IsSuccess = true;
            IsLogin = true;
            Message = string.Empty;
        }
    }
}
