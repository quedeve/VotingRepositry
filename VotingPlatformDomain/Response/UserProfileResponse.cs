using System;
using System.Collections.Generic;
using System.Text;


namespace VotingPlatformDomain.Response
{
    public class UserProfileResponse : BaseResponse
    {
        public List<UserProfileViewModel> ListUserProfile { get; set; }
        public UserProfileViewModel UserProfile { get; set; }
        public UserProfileResponse()
        {
            ListUserProfile = new List<UserProfileViewModel>();
            UserProfile = new UserProfileViewModel();
        }
    }
}
