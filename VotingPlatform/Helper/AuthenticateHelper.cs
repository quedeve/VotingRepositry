using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VotingPlatformDomain;
using VotingPlatformDomain.Response;

namespace VotingPlatform.Helper
{
    public interface IAuthenticateHelper
    {
        UserProfileResponse GenerateToken(UserProfileResponse request);
        UserProfileResponse GenerateTokenLogout(string currentLogin);
        void IsLogin(ref string email,ref BaseResponse response, ClaimsIdentity identity);
        //IEnumerable<User> GetAll();
    }
    public class AuthenticateHelper : IAuthenticateHelper
    {

        private readonly AppSettings appSettings;

        public AuthenticateHelper(IOptions<AppSettings> _appSetinng )
        {
            appSettings = _appSetinng.Value;
        }

        public UserProfileResponse GenerateToken(UserProfileResponse request)
        {

            // return null if user not found
            if (request.IsSuccess)
            {
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.KataKunciRahasiaku);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, request.UserProfile.Email)
                    }),
                    Expires = DateTime.Now.AddHours(12),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                request.Token = tokenHandler.WriteToken(token);
            }

          

            return request;
        }

        public void IsLogin(ref string email,  ref BaseResponse response, ClaimsIdentity identity)
        {
            response = new BaseResponse();
            IList<Claim> claim = identity.Claims.ToList();
            if (claim.Count>0)
            {
                var value = claim.Where(x => x.Type == "exp").Select(x => int.Parse(x.Value)).First();
                DateTime exp = new DateTime(value);
                var date = DateTime.FromFileTime(value);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(value);
                if (dateTimeOffset.LocalDateTime < DateTime.Now.ToLocalTime())
                {
                    response.IsSuccess = false;
                    response.IsLogin = false;
                    response.Message = "Token Expired, Please Relogin !";
                }
                else
                {
                    email = claim[0].Value;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.IsLogin = false;
                response.Message = "Token Expired, Please Relogin !";
            }
           
        }

        public UserProfileResponse GenerateTokenLogout(string currentLogin)
        {
            UserProfileResponse response = new UserProfileResponse();
            // return null if user not found

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.KataKunciRahasiaku);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, currentLogin)

                    }),
                    Expires = DateTime.Now,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
            response.Token = tokenHandler.WriteToken(token);



            return response;
        }
    }
}
