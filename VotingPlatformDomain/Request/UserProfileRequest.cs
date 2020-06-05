using System;
using System.Collections.Generic;
using System.Text;

namespace VotingPlatformDomain.Request
{
    public class UserProfileRequest : BaseRequest
    {
        public int UserProfileID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short Gender { get; set; }
        public int Age { get; set; }
        public int RoleID { get; set; }
        public string Role { get; set; }
        public bool RowStatus { get; set; }
        public UserProfileRequest()
        {
            RowStatus = true;
        }
    }
}
