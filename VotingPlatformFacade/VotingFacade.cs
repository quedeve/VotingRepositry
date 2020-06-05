using System;
using System.Collections.Generic;
using System.Linq;
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
    public class VotingFacade
    {
        private VotingPlatformContext ctx;
        private IVoting iVoting;

        public VotingFacade(string connectionString)
        {
            var optionBuilder = new DbContextOptionsBuilder<VotingPlatformContext>();
            optionBuilder.UseSqlServer(connectionString);

            ctx = new VotingPlatformContext(optionBuilder.Options);
            this.iVoting = new VotingRepository(ctx);
        }

        public async Task<VotingResponse> Add(VotingRequest request)
        {
            VotingResponse response = new VotingResponse();
            try
            {
                Voting voting = new Voting();
                voting.VotingName = request.VotingName;
                voting.VotingDescription = request.VotingDescription;
                voting.DueDate = request.DueDate;
                voting.CategoryId = request.CategoryID;
                voting.Created = DateTime.Now;
                voting.CreatedBy = request.CurrentLogin;
                voting.RowStatus = true;
                if (await iVoting.Add<Voting>(voting))
                {
                    return response;
                }
                response.Message = "Failed to Add Voting";
                response.IsSuccess = false;

            }
            catch (Exception ex)
            {
                response.Message = "Something Error in Our System : "+ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<VotingResponse> Update(VotingRequest request)
        {
            VotingResponse response = new VotingResponse();
            try
            {
                Voting voting = new Voting();
                voting.VotingId = request.VotingID;
                voting.VotingName = request.VotingName;
                voting.VotingDescription = request.VotingDescription;
                voting.DueDate = request.DueDate;
                voting.CategoryId = request.CategoryID;
                voting.Modified = DateTime.Now;
                voting.ModifiedBy = request.CurrentLogin;
                if (await iVoting.Update<Voting>(voting))
                {
                    return response;
                }
                response.Message = "Failed to Update Voting";
                response.IsSuccess = false;
            }
            catch (Exception ex)
            {
                response.Message = "Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<VotingResponse> Delete(VotingRequest request)
        {
            VotingResponse response = new VotingResponse();
            try
            {
                if (await iVoting.Delete(request.VotingID))
                {
                    return response;
                }
                response.Message = "Failed to Delete Voting";
                response.IsSuccess = false;
            }
            catch (Exception ex)
            {
                response.Message = "Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<VotingResponse> GetAll()
        {
            VotingResponse response = new VotingResponse();
            try
            {
                Voting voting = new Voting();
                var query= await iVoting.GetAll<Voting>();
                response.ListVoting = (from q in query
                                       select new VotingViewModel
                                       {
                                           VotingId = q.VotingId,
                                           CategoryId = q.CategoryId,
                                           Created = q.Created,
                                           DueDate = q.DueDate,
                                           VotingName = q.VotingName,
                                           VotingDescription = q.VotingDescription
                                       }).ToList();
            }
            catch (Exception ex)
            {

                response.Message = "Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<VotingResponse> GetByID(VotingRequest request)
        {
            VotingResponse response = new VotingResponse();
            try
            {
                response.Voting = await iVoting.GetByID<VotingViewModel>(request.VotingID);
            }
            catch (Exception ex)
            {
                response.Message = "Something Error in Our System : " + ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<VotingDTResponse> GetAllPagination(string search, int draw, string order, string orderDir, int startRec, int pageSize)
        {
            VotingDTResponse response = new VotingDTResponse();

            try
            {
                var query = await iVoting.GetAll();
                response.recordsTotal = query.Count();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x => x.VotingName.ToLower().Contains(search.ToLower()) ||
                                        x.CategoryName.ToLower().Contains(search.ToLower()));
                }
                response.recordsFiltered = query.Count();
                switch (order)
                {
                    case "0":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.VotingName);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.VotingName);
                        }
                        break;
                    case "1":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.VotingDescription);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.VotingDescription);
                        }
                        break;
                    case "2":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.Created);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Created);
                        }
                        break;
                    case "3":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.SupportersCount);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SupportersCount);
                        }
                        break;
                    case "4":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.DueDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.DueDate);
                        }
                        break;
                    case "5":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.CategoryName);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryName);
                        }
                        break;
                }
                response.ListVoting = query.Skip(startRec).Take(pageSize).ToList();
                response.draw = Convert.ToInt32(draw);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something Error in Our System : " + ex.Message;
            }


            return response;
        }

        public async Task<VotingDTResponse> GetAllPaginationClientSide(string search, int draw, string order, string orderDir, int startRec, int pageSize, int? categoryID, string VotingName)
        {
            VotingDTResponse response = new VotingDTResponse();

            try
            {
                var query = await iVoting.GetAll();
                response.recordsTotal = query.Count();
                if (categoryID != null)
                {
                    query = query.Where(x => x.CategoryId == categoryID);
                }
                if (!(string.IsNullOrEmpty(VotingName)&&string.IsNullOrWhiteSpace(VotingName)))
                {
                    query = query.Where(x => x.VotingName.ToLower().Contains(VotingName.ToLower()));
                }
                response.recordsFiltered = query.Count();
                switch (order)
                {
                    case "0":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.VotingName);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.VotingName);
                        }
                        break;
                    case "1":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.VotingDescription);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.VotingDescription);
                        }
                        break;
                    case "2":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.Created);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Created);
                        }
                        break;
                    case "3":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.SupportersCount);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SupportersCount);
                        }
                        break;
                    case "4":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.DueDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.DueDate);
                        }
                        break;
                    case "5":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.CategoryName);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryName);
                        }
                        break;
                }
                response.ListVoting = query.Skip(startRec).Take(pageSize).ToList();
                response.draw = Convert.ToInt32(draw);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something Error in Our System : " + ex.Message;
            }


            return response;
        }
    }

}
