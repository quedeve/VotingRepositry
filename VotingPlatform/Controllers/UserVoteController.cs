using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VotingPlatform.Helper;
using VotingPlatformDomain;
using VotingPlatformDomain.Request;
using VotingPlatformDomain.Response;
using VotingPlatformFacade;

namespace VotingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVoteController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private string CurrentLogin;
        private BaseResponse response;
        private UserVoteFacade facade;
        private IAuthenticateHelper authHelper;
        public UserVoteController(IOptions<AppSettings> _appSettings, IAuthenticateHelper _authHelper)
        {
            appSettings = _appSettings.Value;
            facade = new UserVoteFacade(appSettings.VotingPlatformConnectionString);
            authHelper = _authHelper;
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]UserVoteRequest request)
        {
            try
            {
                authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    request.CurrentLogin = CurrentLogin;
                    return Ok(await facade.Add(request));
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}