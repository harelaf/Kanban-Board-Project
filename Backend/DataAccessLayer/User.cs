using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class User : DalObject<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string HostEmail { get; set; }

        const string COL_EMAIL = "Email";
        const string COL_PASSWORD = "Password";
        const string COL_NICKNAME = "Nickname";
        const string COL_HOST_EMAIL = "HostEmail";

        string ConnetionString = null;
        string SqlQuery = null;
        SQLiteConnection Connection;
        SQLiteCommand Command;
        const string DATABASE_NAME = "kanbanDB.db";
        string MyPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), DATABASE_NAME));

        public User()
        {
            Email = null;
            Password = null;
            Nickname = null;
            HostEmail = null;
        }

        public User(string Email, string Password, string Nickname, string HostEmail)
        {
            this.Email = Email.ToLower();
            this.Password = Password;
            this.Nickname = Nickname;
            this.HostEmail = HostEmail;
        }

        public User(string Email)
        {
            this.Email = Email.ToLower();
            Password = null;
            Nickname = null;
            HostEmail = null;
        }

        /// <summary>
        /// This function saves the information of the user that called it to the database
        /// </summary>
        public override void Save()
        {

            ConnetionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnetionString);
            SQLiteDataReader DataReader;

            try
            {
                Connection.Open();

                SqlQuery = $"SELECT * FROM tbUsers WHERE {COL_EMAIL} = '{Email}'";
                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                if (!DataReader.Read())
                {
                    Command = new SQLiteCommand(null, Connection);
                    Command.CommandText = "INSERT INTO tbUsers VALUES(@Email,@Nickname,@Password,@HostEmail)";
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter NicknameParam = new SQLiteParameter(@"Nickname", Nickname);
                    SQLiteParameter PasswordParam = new SQLiteParameter(@"Password", Password);
                    SQLiteParameter HostEmailParam = new SQLiteParameter(@"HostEmail", HostEmail);

                    Command.Parameters.Add(EmailParam);
                    Command.Parameters.Add(NicknameParam);
                    Command.Parameters.Add(PasswordParam);
                    Command.Parameters.Add(HostEmailParam);

                    Command.Prepare();
                    int num_rows_changed = Command.ExecuteNonQuery();
                    Command.Dispose();
                }
                DataReader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// This function retrieves the information of a user from the database, using its email
        /// </summary>
        /// <returns>Using the users email, retrieve the information, store it in the fields and return this user</returns>
        public override User Import()
        {

            ConnetionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnetionString);
            SQLiteDataReader DataReader;
            try
            {
                Connection.Open();
                SqlQuery = $"SELECT * FROM tbUsers WHERE {COL_EMAIL} = '{Email}';";
                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                if (DataReader.Read())
                {
                    Email = (string)DataReader[COL_EMAIL];
                    Password = (string)DataReader[COL_PASSWORD];
                    Nickname = (string)DataReader[COL_NICKNAME];
                }
                DataReader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Connection.Close();
                Command.Dispose();
            }
            return this;
        }

        public Board GetBoard()
        {
            Board b = new Board(HostEmail);
            return b.Import();
        }


        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
