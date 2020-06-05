using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingPlatformModel.Interface;
using VotingPlatformModel.Model;

namespace VotingPlatformModel.Repository
{
    public class CategoryRepository : ICategory
    {
        VotingPlatformContext ctx;

        public CategoryRepository(VotingPlatformContext _ctx)
        {
            ctx = _ctx;
        }

        public async Task<bool> Add<T1>(T1 Model)
        {

                Category category = Model as Category;
                ctx.Category.Add(category);
                ctx.SaveChanges();
      
            return true;
        }

        public async Task<bool> Delete(int ID)
        {

                var category = ctx.Category.Where(x => x.RowStatus == true && x.CategoryId == ID).FirstOrDefault();
                if (category != null)
                {
                    category.RowStatus = false;
                    ctx.SaveChanges();
                    return true;
                }
   
            return false;
        }

        public async Task<List<T1>> GetAll<T1>()
        {


                    return await ctx.Category.Where(x=>x.RowStatus==true).ToListAsync() as List<T1>;

        }

        public async Task<IQueryable<Category>> GetAll()
        {
            return  ctx.Category.Where(x => x.RowStatus == true);
        }

        public async Task<T1> GetByID<T1>(int ID)where T1:class
        {

                
                return ctx.Category.Where(x => x.RowStatus == true && x.CategoryId == ID).FirstOrDefault() as T1;

        }

        public async Task<bool> IsDuplicate(string Key)
        {

                return ctx.Category.Any(x => x.RowStatus == true && x.CategoryName.ToLower() == Key.ToLower());

            
        }

        public async Task<bool> IsDuplicate<T1>(T1 Model) where T1 : class
        {

                var request = Model as Category;
                return ctx.Category.Any(x => x.RowStatus == true && x.CategoryName == request.CategoryName && x.CategoryId!=request.CategoryId);

        }

        public async Task<bool> Update<T1>(T1 Model)
        {
            try
            {
                var request = Model as Category;
                var category = ctx.Category.Where(x => x.RowStatus == true && x.CategoryId == request.CategoryId).FirstOrDefault();
                if (category != null)
                {
                    category.CategoryName = request.CategoryName;
                    category.Description = request.Description;
                    category.Modified = request.Modified;
                    category.ModifiedBy = request.ModifiedBy;
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch(DbException dbEx)
            {
                throw dbEx;
            }
            catch (Exception ex)
            {

                throw ex;
            }

              

            return false;
        }
    }
}
