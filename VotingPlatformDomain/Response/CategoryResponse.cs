using System;
using System.Collections.Generic;
using System.Text;
using VotingPlatformDomain.Response.DataTable;

namespace VotingPlatformDomain.Response
{
    public class CategoryResponse : BaseResponse
    {
        public List<CategoryViewModel> ListCategory { get; set; }
        public CategoryViewModel Category { get; set; }

        public CategoryResponse()
        {
            ListCategory = new List<CategoryViewModel>();
            Category = new CategoryViewModel();
        }
    }

    public class CategoryDTResponse : DTBaseResponse
    {
        public List<CategoryViewModel> ListCategory { get; set; }
        public CategoryViewModel Category { get; set; }
        public CategoryDTResponse()
        {
            ListCategory = new List<CategoryViewModel>();
            Category = new CategoryViewModel();
        }

    }
}
