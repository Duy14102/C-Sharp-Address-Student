using System;
using Xunit;
using Persistance;
using DAL;

namespace DALtest
{
    public class CategoryDALtest
    {
        private CategoryDAL categoryDAL = new CategoryDAL();
        private Category category = new Category();
        [Fact]
        public void LoginTest1()
        {
            int id = 1;
            category.CategoryID = id;
            int result = categoryDAL.GetCategoryById(id).CategoryID;
            Assert.True(id == result);
        }
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void LoginTest2(int id)
        {
            int result = categoryDAL.GetCategoryById(id).CategoryID;
            Assert.True(id == result);
        }
        [Theory]
        [InlineData("Sea Food", 0)]
        [InlineData("Fruit", 0)]
        [InlineData("Vegetables", 0)]
        [InlineData("Starchy Food", 0)]
        [InlineData("Dairy", 0)]
        [InlineData("Protein", 0)]
        [InlineData("Fat", 0)]
        [InlineData("Diet", 0)]
        public void InsertTest1(string name, int expected)
        {
            category.CategoryName = name;
            int result = categoryDAL.InsertTest(category);
            Assert.True(result <= expected);
        }
    }
}