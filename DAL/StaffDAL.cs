using System;
using Persistance;
using MySql.Data.MySqlClient;
namespace DAL
{
    public class StaffDAL
    {
        private MySqlConnection connection = DBhelper.GetConnection();
        public Staff Login(Staff staff)
        {
            Staff _staff = null;
            // viet the nay moi chay dc nhieu test 1 luc
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "select * from Staffs where Username='" +
                      staff.Username + "' and Userpass='" +
                      MD5.CreateMD5(staff.Userpass) + "';";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        _staff = GetStaff(reader);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch
                {

                }
            }
            // Console.WriteLine(login);
            return _staff;
        }
        private Staff GetStaff(MySqlDataReader reader)
        {
            Staff staff = new Staff();
            staff.StaffID = reader.GetInt32("StaffID");
            staff.StaffName = reader.GetString("StaffName");
            staff.Username = reader.GetString("Username");
            staff.Userpass = reader.GetString("Userpass");
            staff.Role = reader.GetInt32("role");
            return staff;
        }
    }
}