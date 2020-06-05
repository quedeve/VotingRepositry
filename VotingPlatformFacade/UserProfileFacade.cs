using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingPlatformDomain;
using VotingPlatformDomain.Request;
using VotingPlatformDomain.Response;
using VotingPlatformModel.Interface;
using VotingPlatformModel.Model;
using VotingPlatformModel.Repository;


namespace VotingPlatformFacade
{
    public class UserProfileFacade
    {
        private VotingPlatformContext ctx;
        private IUserProfile iUserProfile;
        private string kunciRahasiaku;
        private IRole iRole;
        
        public UserProfileFacade(string connectionString, string kunciRahasiaku)
        {
            var optionBuilder = new DbContextOptionsBuilder<VotingPlatformContext>();
            optionBuilder.UseSqlServer(connectionString);

            ctx = new VotingPlatformContext(optionBuilder.Options);
            this.iUserProfile = new UserProfileRepository(ctx);
            this.iRole = new RoleRespository(ctx);
            this.kunciRahasiaku = kunciRahasiaku;
        }

        public async Task<UserProfileResponse> Add(UserProfileRequest request)
        {
            UserProfileResponse response = new UserProfileResponse();
            try
            {
                UserProfile usrProfile = new UserProfile();
                
                if (!(await iUserProfile.IsDuplicate(request.Email)))
                {
                    
                    usrProfile.Email = request.Email;
                    usrProfile.Password = EncryptString(request.Password);
                    usrProfile.FirstName = request.FirstName;
                    usrProfile.LastName = request.LastName;
                    usrProfile.GenderId = request.Gender;
                    usrProfile.Age = request.Age;
                    usrProfile.RoleId = request.RoleID;
                    usrProfile.Created = DateTime.Now.ToLocalTime();
                    usrProfile.CreatedBy = request.CurrentLogin;
                    usrProfile.RowStatus = request.RowStatus;
                    if (await iUserProfile.Add<UserProfile>(usrProfile))
                    {
                        return response;
                        
                    }
                    response.Message = "Failed to Add UserProfile";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Email Is Duplicate with Existing Data";
                    response.IsSuccess = false;
                }
                
              
            }
            catch (Exception ex)
            {
                response.Message = "Opps, Something Error in Our System : "+ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<UserProfileResponse> Authenticate(UserProfileRequest request)
        {
            UserProfileResponse response = new UserProfileResponse();
            try
            {
                request.Password = EncryptString(request.Password);
                var queryResult =  await iUserProfile.GetUserProfile(request.Email, request.Password);
                if (queryResult != null)
                {
                    response.UserProfile = new UserProfileViewModel()
                    {
                        UserId = queryResult.UserId,
                        Email = queryResult.Email,
                        Password = queryResult.Password,
                        GenderId = queryResult.GenderId,
                        FirstName = queryResult.FirstName,
                        LastName = queryResult.LastName,
                        Age = queryResult.Age
                    };
                    return response;
                }
                response.IsSuccess = false;
                response.Message = "Email and Password not Found!!!";
            }
            catch (Exception ex)
            {

                response.Message = "Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<UserProfileResponse> Register(UserProfileRequest request)
        {
            UserProfileResponse response = new UserProfileResponse();
            try
            {
                UserProfile usrProfile = new UserProfile();

                if (!(await iUserProfile.IsDuplicate(request.Email)))
                {
            
                    usrProfile.Email = request.Email;
                    usrProfile.Password = EncryptString(request.Password);
                    usrProfile.FirstName = request.FirstName;
                    usrProfile.LastName = request.LastName;
                    usrProfile.GenderId = request.Gender;
                    usrProfile.Age = request.Age;
                    usrProfile.RoleId =  iRole.GetRoleByName(request.Role).Result.RoleId;
                    usrProfile.Created = DateTime.Now.ToLocalTime();
                    usrProfile.CreatedBy = request.Email;
                    usrProfile.RowStatus = request.RowStatus;
                    if (await iUserProfile.Add<UserProfile>(usrProfile))
                    {
                        return response;

                    }
                    response.Message = "Failed to Register UserProfile";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Email Is Duplicate with Existing Data";
                    response.IsSuccess = false;
                }
              

            }
            catch (Exception ex)
            {
                response.Message = "Opps, Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }


        public async Task<UserProfileResponse> Update(UserProfileRequest request)
        {
            UserProfileResponse response = new UserProfileResponse();
            try
            {
                UserProfile usrProfile = new UserProfile();
                usrProfile.Email = request.Email;
                usrProfile.Password = EncryptString(request.Password);
                usrProfile.FirstName = request.FirstName;
                usrProfile.LastName = request.LastName;
                usrProfile.GenderId = request.Gender;
                usrProfile.Age = request.Age;
                usrProfile.RoleId = request.RoleID;
                usrProfile.Modified = DateTime.Now.ToLocalTime();
                usrProfile.ModifiedBy = request.CurrentLogin;
                usrProfile.RowStatus = request.RowStatus;
                if (!(await iUserProfile.IsDuplicate<UserProfile>(usrProfile)))
                {
                    if (await iUserProfile.Update<UserProfile>(usrProfile))
                    {
                        return response;
                    }
                }
                else
                {
                    response.Message = "Email Is Duplicate with Existing Data";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Opps, Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<UserProfileResponse> Delete(UserProfileRequest request)
        {
            UserProfileResponse response = new UserProfileResponse();
            try
            {
                if (await iUserProfile.Delete(request.RoleID))
                {
                    return response;
                }
                response.Message = "Failed to Delete UserProfile";
                response.IsSuccess = false;
            }
            catch (Exception ex)
            {
                response.Message = "Opps, Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<UserProfileResponse> GetByEmail(string request)
        {
            UserProfileResponse response = new UserProfileResponse();
            try
            {
                var queryResult = await iUserProfile.GetUserProfile(request);
                if (queryResult != null)
                {
                    response.UserProfile = new UserProfileViewModel()
                    {
                        UserId = queryResult.UserId,
                        Email = queryResult.Email,
                        Password = queryResult.Password,
                        GenderId = queryResult.GenderId,
                        FirstName = queryResult.FirstName,
                        LastName = queryResult.LastName,
                        Age = queryResult.Age
                    };
                    return response;
                }
                response.IsSuccess = false;
                response.Message = "Email not Found!!!";
            }
            catch (Exception ex)
            {

                response.Message = "Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        private string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
              
                aes.Key = Encoding.UTF8.GetBytes(kunciRahasiaku);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        //private string DecryptString(string cipherText)
        //{
        //    byte[] iv = new byte[16];
        //    byte[] buffer = Convert.FromBase64String(cipherText);

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(kunciRahasiaku);
        //        aes.IV = iv;
        //        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        //        using (MemoryStream memoryStream = new MemoryStream(buffer))
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
        //                {
        //                    return streamReader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
