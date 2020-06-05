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
    public class CategoryController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private IAuthenticateHelper authHelper;
        private string CurrentLogin = string.Empty;
        private CategoryFacade facade;
        private BaseResponse response;
        public CategoryController(IOptions<AppSettings> _appSettings, IAuthenticateHelper _authHelper)
        {
            appSettings = _appSettings.Value;
            facade = new CategoryFacade(appSettings.VotingPlatformConnectionString);
            authHelper = _authHelper;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
              
                return Ok(await facade.GetAll());

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                    return Ok(await facade.GetAllPagination(search, draw,  order, orderDir,  startRec,  pageSize));

                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]CategoryRequest request)
        {
            try
            {
                authHelper.IsLogin(ref CurrentLogin,ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    request.CurrentLogin = CurrentLogin;
                    if (request.CategoryID <1)
                    {
                        return Ok(await facade.Add(request));
                    }
                    else
                    {
                        return Ok(await facade.Update(request));
                    }
                  

                }
                return Ok(response);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(CategoryRequest request)
        {
            try
            {
                authHelper.IsLogin(ref CurrentLogin, ref response, HttpContext.User.Identity as ClaimsIdentity);
                if (response.IsSuccess)
                {
                    request.CurrentLogin = CurrentLogin;
                    return Ok(await facade.Update(request));

                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(CategoryRequest request)
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
                return BadRequest(ex);
            }
        }

    
    }
}