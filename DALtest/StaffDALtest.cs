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
            staff.Username = "adminpf13";
            staff.Userpass = "admin1234";
            int expected = 2;
            int result = staffDAL.Login(staff).Role;
            Assert.True(expected == result);
        }
        [Theory]
        [InlineData("pf13", "vtcapf13", 1)]
        [InlineData("staffpf13", "staff1234", 1)]
        public void LoginTest2(string userName, string pass, int expected)
        {
            staff.Username = userName;
            staff.Userpass = pass;
            int result = staffDAL.Login(staff).Role;
            Assert.True(expected == result);
        }
        [Theory]
        [InlineData("VTCA1", "VTCA1", "VTCAcademy1", 1, 0)]
        [InlineData("VTCA2", "VTCA2", "VTCAcademy2", 2, 0)]
        [InlineData("VTCA3", "VTCA3", "VTCAcademy3", 2, 0)]
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
