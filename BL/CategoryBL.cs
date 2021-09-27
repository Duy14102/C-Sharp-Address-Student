using System;
using System.Collections.Generic;
using Persistance;
using DAL;
namespace BL
{
    public class CategoryBL
    {
        CategoryDAL categoryDAL = new CategoryDAL();
        public List<Category> GetAllCategory()
        {
            return categoryDAL.GetAllCategory();
        }
        public Category GetCategoryById(int id)
        {
            return categoryDAL.GetCategoryById(id);
        }
    }
}