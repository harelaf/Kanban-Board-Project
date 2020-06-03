using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class RegisteredEmails
    {
        public List<string> Emails { get; set; }
        
        public RegisteredEmails(List<string> Emails)
        {
            this.Emails = Emails;
        }

        public RegisteredEmails()
        {
            Emails = new List<string>();
        }

        public RegisteredEmails Import()
        {
            string ConnectionString;
            string DATABASE_NAME = "kanbanDB.db";
            SQLiteConnection Connection;
            SQLiteCommand Command;

            string MyPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), DATABASE_NAME));

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;
            try
            {
                Connection.Open();
                string sql = "SELECT Email FROM tbUsers;";
                Command = new SQLiteCommand(sql, Connection);
                DataReader = Command.ExecuteReader();
                Emails = new List<string>();
                while (DataReader.Read())
                {
                    Emails.Add(((string)DataReader["Email"]));
                }
                DataReader.Close();
                Command.Dispose();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Close();
            }
            return this;
        }

    }
}
