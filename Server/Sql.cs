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
            public byte[] HistoryKey;
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
                    Name = reader.GetString(3),
                    HistoryKey = (byte[])reader[4],
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
                    Name = reader.GetString(3),
                    HistoryKey = (byte[])reader[4],
                }; 
            }
            return new UserRecord() { ID = 0, Username = "", Password = "", Name = "" };
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
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            return command.ExecuteNonQuery() != 0;
        }

        public static int AddUser(string username, string password, string name, byte[] historyKey)
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
            var insertSql = "INSERT INTO Users (username, password, name, history_key) VALUES " +
                            "(@Username, @Password, @Name, @HistoryKey); " +
                            "SELECT SCOPE_IDENTITY();";
            var insertCommand = new SqlCommand(insertSql, connection);
            insertCommand.Parameters.AddWithValue("@Username", username);
            insertCommand.Parameters.AddWithValue("@Password", password);
            insertCommand.Parameters.AddWithValue("@Name", name);
            insertCommand.Parameters.AddWithValue("@HistoryKey", historyKey);
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
