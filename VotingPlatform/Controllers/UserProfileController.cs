using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class UserProfileController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private UserProfileFacade facade;
        private IAuthenticateHelper authHelper;
        private BaseResponse response;
        private string CurrentLogin = string.Empty;

        public UserProfileController(IOptions<AppSettings> _appSettings, IAuthenticateHelper _authHelper)
        {
            appSettings = _appSettings.Value;
            facade = new UserProfileFacade(appSettings.VotingPlatformConnectionString, appSettings.KunciRahasiaAES);
            authHelper = _authHelper;
           

        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] UserProfileRequest request)
        {
            try
            {
                authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    request.CurrentLogin = CurrentLogin;
                    if (request.UserProfileID >0)
                    {
                        return Ok(await facade.Update(request));
                    }
                    else
                    {
                        return Ok(await facade.Add(request));
                    }
                   
                    
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserProfileRequest request)
        {
            try
            {
                return Ok(authHelper.GenerateToken(await facade.Authenticate(request)));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout([FromBody] UserProfileRequest request)
        {

            try
            {
                authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    request.CurrentLogin = CurrentLogin;
                    return Ok(authHelper.GenerateTokenLogout(CurrentLogin));

                }
                return Ok(response);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserProfileRequest request)
        {
            try
            {
                return Ok(await facade.Register(request));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UserProfileRequest request)
        {
            try
            {
                return Ok(await facade.Update(request));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] UserProfileRequest request)
        {
            try
            {
                authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    request.CurrentLogin = CurrentLogin;
                    return Ok(await facade.Delete(request));

                }
                return Ok(response);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }


}