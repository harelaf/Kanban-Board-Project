using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class RegisteredEmails
    {
        public List<string> Emails { get; set; }
        string connetion_string = null;
        string sql_query = null;
        string database_name = "kanbanDB.sqlite";
        SQLiteConnection connection;
        SQLiteCommand command;
        
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
            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            try
            {
                connection.Open();
                string sql = "SELECT Email FROM tbUsers;";
                SQLiteCommand c = new SQLiteCommand(sql, connection);
                dataReader = c.ExecuteReader();
                Emails.Clear();
                while (dataReader.Read())
                {
                    Emails.Add((string)dataReader["Email"]);
                }
            }
            catch(Exception ex)
            {
                Emails = null;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return this;
        }

    }
}
