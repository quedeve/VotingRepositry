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
    public class VotingController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private VotingFacade facade;
        private IAuthenticateHelper authHelper;
        private string CurrentLogin = string.Empty;
        private BaseResponse response;

        public VotingController(IOptions<AppSettings> _appSettings, IAuthenticateHelper _authHelper)
        {
            appSettings = _appSettings.Value;
            facade = new VotingFacade(appSettings.VotingPlatformConnectionString);
            authHelper = _authHelper;
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] VotingRequest request)
        {
            try
            {

                return Ok(await facade.GetAll());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllPagination")]
        public async Task<IActionResult> GetAllPagination()
        {
            try
            {
                authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    string search = HttpContext.Request.Query["search[value]"].ToString();
                    int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
                    string order = HttpContext.Request.Query["order[0][column]"];
                    string orderDir = HttpContext.Request.Query["order[0][dir]"];
                    int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                    int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
                    return Ok(await facade.GetAllPagination(search, draw, order, orderDir, startRec, pageSize));

                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllClientSide")]
        public async Task<IActionResult> GetAllClientSide(int? CategoryID=null, string VotingName = null)
        {
            try
            {
                
                    string search = HttpContext.Request.Query["search[value]"].ToString();
                    int draw = Convert.ToInt32(HttpContext.Request.Query["draw"]);
                    string order = HttpContext.Request.Query["order[0][column]"];
                    string orderDir = HttpContext.Request.Query["order[0][dir]"];
                    int startRec = Convert.ToInt32(HttpContext.Request.Query["start"]);
                    int pageSize = Convert.ToInt32(HttpContext.Request.Query["length"]);
                    return Ok(await facade.GetAllPaginationClientSide(search, draw, order, orderDir, startRec, pageSize, CategoryID, VotingName));

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] VotingRequest request)
        {
            try
            {
                authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    request.CurrentLogin = CurrentLogin;
                    if (request.VotingID>0)
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

        //[HttpPost]
        //[Route("Update")]
        //public async Task<IActionResult> Update([FromBody] VotingRequest request)
        //{
        //    try
        //    {
        //        authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
        //        if (response.IsSuccess)
        //        {
        //            request.CurrentLogin = CurrentLogin;
        //            return Ok(await facade.Update(request));

        //        }
        //        return Ok(response);

        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] VotingRequest request)
        {
            try
            {
                return Ok(await facade.Delete(request));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetByID")]
        public async Task<IActionResult> GetByID([FromBody] VotingRequest request)
        {
            try
            {
                return Ok(await facade.GetByID(request));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}