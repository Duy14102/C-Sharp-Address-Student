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
            return _staff;
        }
        public int Insert(Staff staff)
        {
            int? result = null;
            MySqlConnection connection = DBhelper.GetConnection();
            string sql = @"insert into Staffs(StaffName, Username, Userpass, role) values 
                      (@staffName, @userName, @userPass, @role);";
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@staffName", staff.StaffName);
                    command.Parameters.AddWithValue("@userName", staff.Username);
                    command.Parameters.AddWithValue("@userPass", MD5.CreateMD5(staff.Userpass));
                    command.Parameters.AddWithValue("@role", staff.Role);
                    result = command.ExecuteNonQuery();
                }
                catch
                {
                    result = -2;
                }
                finally
                {
                    connection.Close();
                }
            }
            return result ?? 0;
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