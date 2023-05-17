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

        public struct UserRecord
        {
            public int ID;
            public string Username;
            public string Password;
            public string Name;
        }

        public static List<UserRecord> GetAllUsers()
        {
            var users = new List<UserRecord>();
            var selectSql = "SELECT * FROM Users;";
            using var connection = new SqlConnection(SqlConnectionString);
            using var command = new SqlCommand(selectSql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = new UserRecord()
                {
                    ID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Name = reader.GetString(3)
                };
                users.Add(user);
            }
            return users;
        }

        private static UserRecord GetUser(SqlCommand command)
        {
            using var connection = new SqlConnection(SqlConnectionString);
            command.Connection = connection;
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserRecord()
                {
                    ID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Name = reader.GetString(3)
                }; 
            }
            return new UserRecord() { ID = 0 };
        }

        public static UserRecord GetUserByID(int id)
        {
            var selectSql = $"SELECT * FROM Users WHERE id = @id;";
            using var command = new SqlCommand(selectSql);
            command.Parameters.AddWithValue("@id", id);
            return GetUser(command);
        }

        public static UserRecord GetUserByUsername(string username)
        {
            var selectSql = $"SELECT * FROM Users WHERE username = @Username;";
            using var command = new SqlCommand(selectSql);
            command.Parameters.AddWithValue("@Username", username);
            return GetUser(command);
        }

        public static bool DeleteUser(int id)
        {
            var deleteSql = "DELETE FROM Users WHERE id = @id";
            using var connection = new SqlConnection(SqlConnectionString);
            using var command = new SqlCommand(deleteSql, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            return command.ExecuteNonQuery() != 0;
        }
        public static bool DeleteUser(string username)
        {
            var deleteSql = "DELETE FROM Users WHERE username = @Username";
            using var connection = new SqlConnection(SqlConnectionString);
            using var command = new SqlCommand(deleteSql, connection);
            command.Parameters.AddWithValue("@Username", username);
            connection.Open();
            return command.ExecuteNonQuery() != 0;
        }

        #region Debug

        public static bool Execute(string sql)
        {
            using var connection = new SqlConnection(SqlConnectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();
            return command.ExecuteNonQuery() != 0;
        }

        #endregion

        /*public static int FindUser(string username, string password)
        {
            var selectSql = "SELECT id FROM Users WHERE username = @Username";
            using var connection = new SqlConnection(SqlConnectionString);
            using var command = new SqlCommand(selectSql, connection);
            command.Parameters.AddWithValue("@Username", username);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (reader.GetString(1) != password)
                {
                    throw new ArgumentException("Incorrect password.");
                }
                return reader.GetInt32(0);
            }
            else
            {
                throw new ArgumentException("Incorrect username.");
            }
        }*/

        public static bool SetName(int id, string name)
        {
            var updateSql = "UPDATE Users SET name = @name WHERE id = @id";
            using var connection = new SqlConnection(SqlConnectionString);
            using var command = new SqlCommand(updateSql, connection);
            command.Parameters.AddWithValue("@name", id);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            return command.ExecuteNonQuery() != 0;
        }

        public static int AddUser(string username, string password, string name)
        {
            using var connection = new SqlConnection(SqlConnectionString);
            connection.Open();
            var selectSql = "SELECT COUNT(*) FROM Users WHERE username = @Username";
            var selectCommand = new SqlCommand(selectSql, connection);
            selectCommand.Parameters.AddWithValue("@Username", username);
            if ((int)selectCommand.ExecuteScalar() > 0)
            {
                throw new ArgumentException("This username is already taken");
            }
            var insertSql = "INSERT INTO Users (username, password, name) VALUES (@Username, @Password, @Name); " +
                            "SELECT SCOPE_IDENTITY();";
            var insertCommand = new SqlCommand(insertSql, connection);
            insertCommand.Parameters.AddWithValue("@Username", username);
            insertCommand.Parameters.AddWithValue("@Password", password);
            insertCommand.Parameters.AddWithValue("@Name", name);
            object newUserIdObj = insertCommand.ExecuteScalar();
            if (int.TryParse(newUserIdObj.ToString(), out int newUserId))
            {
                return newUserId;
            }
            else
            {
                throw new ArgumentException("Error while adding a new user.");
            }
        }
    }
}
