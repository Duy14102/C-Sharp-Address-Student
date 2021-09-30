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
    }
}