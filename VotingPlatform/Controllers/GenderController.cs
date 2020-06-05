using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VotingPlatformDomain;
using VotingPlatformFacade;

namespace VotingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {

        private readonly AppSettings appSettings;

        private GenderFacade facade;
        public GenderController(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings.Value;
            facade = new GenderFacade(appSettings.VotingPlatformConnectionString);
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                return Ok(await facade.GetAllGender());

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}