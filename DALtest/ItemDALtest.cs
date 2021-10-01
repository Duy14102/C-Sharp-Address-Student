using System;
using Xunit;
using Persistance;
using DAL;

namespace DALtest
{
    public class ItemDALtest
    {
        private ItemDAL itemDAL = new ItemDAL();
        private Item item = new Item();
        [Fact]
        public void LoginTest1()
        {
            int id = 1;
            item.ItemsID = id;
            int result = itemDAL.GetById(id).ItemsID;
            Assert.True(id == result);
        }
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public void LoginTest2(int id)
        {
            int result = itemDAL.GetById(id).ItemsID;
            Assert.True(id == result);
        }
        [Theory]
        [InlineData("Pork", 1, 10, 0)]
        [InlineData("Potato", 3, 5, 0)]
        [InlineData("Tomatoes", 3, 5, 0)]
        [InlineData("Dog meat", 1, 15, 0)]
        [InlineData("Beef", 1, 15, 0)]
        [InlineData("Bean", 3, 5, 0)]
        [InlineData("Sting", 5, 10, 0)]
        public void InsertTest1(string name, int categoryid, decimal price, int expected)
        {
            item.ItemName = name;
            item.CategoryInfo.CategoryID = categoryid;
            item.ItemPrice = price;
            int result = itemDAL.InsertTest(item);
            Assert.True(result <= expected);
        }
    }
}