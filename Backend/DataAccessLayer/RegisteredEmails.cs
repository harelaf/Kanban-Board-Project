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
            string connetion_string;
            string database_name = "kanbanDB.db";
            SQLiteConnection connection;

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));

            connetion_string = $"Data Source={path};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            try
            {
                connection.Open();
                string sql = "SELECT Email FROM tbUsers;";
                SQLiteCommand c = new SQLiteCommand(sql, connection);
                dataReader = c.ExecuteReader();
                Emails = new List<string>();
                while (dataReader.Read())
                {
                    Emails.Add(((string)dataReader["Email"]));
                }
                dataReader.Close();
                c.Dispose();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return this;
        }

    }
}
