using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class User : DalObject<User>
    {
        public string email { get; set; }
        public string password { get; set; }
        public string nickname { get; set; }
        public long idGiver { get; set; }
        public long numOfColumns { get; set; }


        const string colEmail = "Email";
        const string colPassword = "password";
        const string colNickname = "nickName";
        const string colIdGiver = "IdGiver";
        const string colNumOfColumns = "numOfColumns";


        public User()
        {
            email = null;
            password = null;
            nickname = null;
            numOfColumns = 0;
            idGiver = 0;
        }

        public User(string email, string password, string nickname, int idGiver, int numOfColumns)
        {
            this.email = email.ToLower();
            this.password = password;
            this.nickname = nickname;
            this.idGiver = idGiver;
            this.numOfColumns = numOfColumns;
        }

        public User(string email)
        {
            this.email = email.ToLower();
            password = null;
            nickname = null;
            idGiver = 0;
            numOfColumns = 0;
        }

        /// <summary>
        /// This function saves the information of the user that called it to the database
        /// </summary>
        public override void Save()
        {
            string connetion_string = null;
            string sql_query = null;
            string database_name = "kanbanDB.db";
            SQLiteConnection connection;
            SQLiteCommand command = new SQLiteCommand();

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));

            connetion_string = $"Data Source={path};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                sql_query = $"SELECT * FROM tbUsers WHERE {colEmail} = '{email}'";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read()) //This if statement checks if the user already exists in the database, in that case we need to update its information
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = $"UPDATE tbUsers SET {colIdGiver} = @idGiver, {colNumOfColumns} = @numOfColumns WHERE {colEmail} = @Email";
                    SQLiteParameter IdGiverParam = new SQLiteParameter(@"idGiver", idGiver);
                    SQLiteParameter numOfColumnsParam = new SQLiteParameter(@"numOfColumns", numOfColumns);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);

                    command.Parameters.Add(IdGiverParam);
                    command.Parameters.Add(numOfColumnsParam);
                    command.Parameters.Add(EmailParam);

                    command.Prepare();
                    int num_rows_changed = command.ExecuteNonQuery();
                    command.Dispose();
                }
                else //This else is if the user isnt in the database, and in the case we need to insert its information to the database
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = "INSERT INTO tbUsers VALUES(@Email,@nickName,@password,@IdGiver,@numOfColumns)";
                    SQLiteParameter IdGiverParam = new SQLiteParameter(@"idGiver", idGiver);
                    SQLiteParameter numOfColumnsParam = new SQLiteParameter(@"numOfColumns", numOfColumns);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                    SQLiteParameter NicknameParam = new SQLiteParameter(@"nickName", nickname);
                    SQLiteParameter PasswordParam = new SQLiteParameter(@"password", password);

                    command.Parameters.Add(IdGiverParam);
                    command.Parameters.Add(numOfColumnsParam);
                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(NicknameParam);
                    command.Parameters.Add(PasswordParam);

                    command.Prepare();
                    int num_rows_changed = command.ExecuteNonQuery();
                    command.Dispose();
                }
                dataReader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// This function retrieves the information of a user from the database, using its email
        /// </summary>
        /// <returns>Using the users email, retrieve the information, store it in the fields and return this user</returns>
        public override User Import()
        {
            string connetion_string = null;
            string sql_query = null;
            string database_name = "kanbanDB.db";
            SQLiteConnection connection;
            SQLiteCommand command = null;

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));

            connetion_string = $"Data Source={path};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            try
            {
                connection.Open();
                sql_query = $"SELECT * FROM tbUsers WHERE {colEmail} = '{email}';";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    email = (string)dataReader[colEmail];
                    password = (string)dataReader[colPassword];
                    nickname = (string)dataReader[colNickname];
                    idGiver = (long)dataReader[colIdGiver];
                    numOfColumns = (long)dataReader[colNumOfColumns];
                }
                dataReader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return this;
        }

        /// <summary>
        /// This function retrieves the columns of the users board from the database
        /// </summary>
        /// <returns>A list of columns, that are essentially the users board</returns>
        public List<Column> GetColumns()
        {
            int i = 0;
            List<Column> colList = new List<Column>();
            Column col;
            while (i < numOfColumns)
            {
                col = new Column(email,i);
                colList.Add(col.Import());
                i++;
            }
            return colList;
        }


        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
