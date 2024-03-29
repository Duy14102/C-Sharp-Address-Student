using System;
using Xunit;
using Persistance;
using DAL;

namespace DALtest
{
    public class TableFoodDALtest
    {
        private TableFoodDAL tableFoodDAL = new TableFoodDAL();
        private TableFood tableFood = new TableFood();
        [Fact]
        public void LoginTest1()
        {
            int id = 1;
            tableFood.TableId = id;
            int result = tableFoodDAL.GetById(id).TableId;
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
        public void LoginTest2(int id)
        {
            int result = tableFoodDAL.GetById(id).TableId;
            Assert.True(id == result);
        }
    }
}