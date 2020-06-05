using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VotingPlatformDomain;
using VotingPlatformDomain.Request;
using VotingPlatformDomain.Response;
using VotingPlatformModel.Interface;
using VotingPlatformModel.Model;
using VotingPlatformModel.Repository;

namespace VotingPlatformFacade
{
    public class CategoryFacade
    {
        private VotingPlatformContext ctx;
        private ICategory iCategory;
        public CategoryFacade(string connectionString)
        {
            var optionBuilder = new DbContextOptionsBuilder<VotingPlatformContext>();
            optionBuilder.UseSqlServer(connectionString);

            ctx = new VotingPlatformContext(optionBuilder.Options);
            this.iCategory = new CategoryRepository(ctx);
        }

        public async Task<CategoryResponse> GetAll()
        {
            CategoryResponse response = new CategoryResponse();
            try
            {
                var query = await iCategory.GetAll<Category>() as List<Category>;
                response.ListCategory = (from q in query
                                         select new CategoryViewModel
                                         {
                                             CategoryId = q.CategoryId,
                                             CategoryName = q.CategoryName,
                                             Description = q.Description
                                         }).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something Error in Our System : " + ex.Message;
            }
            return response;
        }

        public async Task<CategoryResponse> Add(CategoryRequest request)
        {
            CategoryResponse response = new CategoryResponse();
            try
            {
                Category category = new Category();
                if (!(await iCategory.IsDuplicate(request.CategoryName)))
                {
                    category.CategoryName = request.CategoryName;
                    category.Description = request.Description;
                    category.Created = DateTime.Now;
                    category.CreatedBy = request.CurrentLogin;
                    category.RowStatus = true; 
                    if (await iCategory.Add<Category>(category))
                    {
                        return response;

                    }
                    response.Message = "Failed to Add Category";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Category is Duplicated with existing";
                    response.IsSuccess = false;
                }
               

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something Error in Our System : "+ex.Message;
            }
            return response;
        }

        public async Task<CategoryResponse> Update(CategoryRequest request)
        {
            CategoryResponse response = new CategoryResponse();
            try
            {
                Category category = new Category();
                category.CategoryId = request.CategoryID;
                category.CategoryName = request.CategoryName;
                category.Description = request.Description;
                category.Modified = DateTime.Now;
                category.ModifiedBy = request.CurrentLogin;
                if (!(await iCategory.IsDuplicate<Category>(category)))
                {
                    if (await iCategory.Update(category))
                    {
                        return response;
                    }
                    else
                    {
                        response.Message = "Failed to Update Category";
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.Message = "Category is Duplicated with existing";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = "Something Error in Our System : " + ex.Message;
            }


            return response;
        }

        public async Task<CategoryResponse> Delete(CategoryRequest request)
        {
            CategoryResponse response = new CategoryResponse();
            try
            {
                if (await iCategory.Delete(request.CategoryID))
                {
                    return response;
                }
                response.IsSuccess = false;
                response.Message = "Delete Category Failed";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something Error in Our System : " + ex.Message;
            }
            return response;
        }

        public async Task<CategoryDTResponse> GetAllPagination(string search, int draw, string order, string orderDir, int startRec, int pageSize)
        {
            CategoryDTResponse response = new CategoryDTResponse();

            try
            {
                var query = await iCategory.GetAll();
                response.recordsTotal = query.Count();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x => x.CategoryName.ToLower().Contains(search.ToLower())||
                                        x.Description.ToLower().Contains(search.ToLower()));
                }
                response.recordsFiltered = query.Count();
                switch (order)
                {
                    case "0":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.CategoryName);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryName);
                        }
                        break;
                    case "1":
                        if (orderDir == "asc")
                        {
                            query = query.OrderBy(x => x.Description);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Description);
                        }
                        break;
                }
                response.ListCategory = (from q in query
                                 select new CategoryViewModel
                                 {
                                    CategoryId = q.CategoryId,
                                    CategoryName = q.CategoryName,
                                    Description = q.Description
                                 }).Skip(startRec).Take(pageSize).ToList();
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
