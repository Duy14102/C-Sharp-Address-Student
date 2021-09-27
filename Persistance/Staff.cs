using System;

namespace Persistance
{
    public class Staff
    {
        public int StaffID { set; get; }
        public string Username { set; get; }
        public string Userpass { set; get; }
        public string StaffName { set; get; }
        public int Role { set; get; }
        public const int STAFF_ROLE = 1;
        public const int ADMIN_ROLE = 2;
    }
}
