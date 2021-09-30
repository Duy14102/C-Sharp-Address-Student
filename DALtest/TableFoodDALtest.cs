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
            tableFood.Name = "Table 1";
            int expected = 1;
            int result = tableFoodDAL.TableTest(tableFood).Status;
            Assert.True(expected == result);
        }
        [Theory]
        [InlineData("Table 2", 1)]
        [InlineData("Table 3", 1)]
        [InlineData("Table 4", 1)]
        [InlineData("Table 5", 1)]
        [InlineData("Table 6", 1)]
        [InlineData("Table 7", 1)]
        [InlineData("Table 8", 1)]
        [InlineData("Table 9", 1)]
        [InlineData("Table 10", 1)]
        public void LoginTest2(string tableName, int expected)
        {
            tableFood.Name = tableName;
            int result = tableFoodDAL.TableTest(tableFood).Status;
            Assert.True(expected == result);
        }
    }
}