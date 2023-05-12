using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal static class Sql
    {
        private const string SqlConnectionString =
           "Server=localhost\\SQLEXPRESS;Database=Coursework;Trusted_Connection=True;";
        private static readonly string DatabaseTableName = "Users";
        private static readonly string Idfield = "id";
        private static readonly string EmailField = "email";
        private static readonly string PasswordField = "password";

        public static int FindUser(string email, string password)
        {
            var selectSql = "SELECT id FROM Users WHERE email = @Email";
            using var connection = new SqlConnection(SqlConnectionString);
            using var command = new SqlCommand(selectSql, connection);
            command.Parameters.AddWithValue("@Email", email);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (reader.GetString(1) != password)
                {
                    throw new ArgumentException("Invalid password.");
                }
                return reader.GetInt32(0);
            }
            else
            {
                throw new ArgumentException("User with this email address does not exists.");
            }
        }

        public static int AddUser(string email, string password)
        {
            using SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();
            var checkSql = "SELECT COUNT(*) FROM Users WHERE email = @Email";
            var checkCommand = new SqlCommand(checkSql, connection);
            checkCommand.Parameters.AddWithValue("@Email", email);
            if ((int)checkCommand.ExecuteScalar() > 0)
            {
                throw new ArgumentException("A user with this email address already exists.");
            }
            var insertSql = "INSERT INTO Users (email, password) VALUES (@Email, @Password); " +
                            "SELECT SCOPE_IDENTITY();";
            var insertCommand = new SqlCommand(insertSql, connection);
            insertCommand.Parameters.AddWithValue("@Email", email);
            insertCommand.Parameters.AddWithValue("@Password", password);
            object newUserIdObj = insertCommand.ExecuteScalar();
            if (newUserIdObj != null && int.TryParse(newUserIdObj.ToString(), out int newUserId))
            {
                return newUserId;
            }
            else
            {
                return -1;
            }
        }
    }
}
