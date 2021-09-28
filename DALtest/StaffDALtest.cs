using System;
using Xunit;
using Persistance;
using DAL;

namespace DALtest
{
    public class StaffDALtest
    {
        private StaffDAL staffDAL = new StaffDAL();
        private Staff staff = new Staff();
        [Fact]
        public void LoginTest1()
        {
            staff.Username = "pf13";
            staff.Userpass = "vtcapf13";
            int expected = 1;
            int result = staffDAL.Login(staff).Role;
            Assert.True(expected == result);
        }
        [Theory]
        [InlineData("pf13", "vtcapf13", 2)]
        [InlineData("pf131", "PF13VTCAcademy", 0)]
        [InlineData("pf13", "PF13VTCAcademysdf", 0)]
        [InlineData("pf14", "vtcapf14", 2)]
        public void LoginTest2(string userName, string pass, int expected)
        {
            staff.Username = userName;
            staff.Userpass = pass;
            int result = staffDAL.Login(staff).Role;
            Assert.True(expected == result);
        }
        [Theory]
        [InlineData("VTCA1", "VTCA1", "VTCAcademy", 1, 0)]
        [InlineData("VTCA2", "VTCA2", "VTCAcademy", 2, 0)]
        [InlineData("VTCA3", "VTCA3", "VTCAcademy", 2, 0)]
        public void InsertTest1(string staffName, string userName, string pass, int role, int expected)
        {
            staff.StaffName = staffName;
            staff.Username = userName;
            staff.Userpass = pass;
            staff.Role = role;
            int result = staffDAL.Insert(staff);
            Assert.True(result <= expected);
        }
    }
}
