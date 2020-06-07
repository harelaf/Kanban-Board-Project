using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DalController
    {
        string ConnectionString = null;
        SQLiteConnection Connection;
        SQLiteCommand Command;
        const string DATABASE_NAME = "kanbanDB.db";
        string MyPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), DATABASE_NAME));

        const string COL_EMAIL = "Email";

        /// <summary>
        /// This function deletes all of the information stored in the database
        /// </summary>
        public void Delete()
        {

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            try
            {
                Connection.Open();

                Command = new SQLiteCommand(null, Connection);
                Command.CommandText = "DELETE FROM tbColumns; DELETE FROM tbUsers; DELETE FROM tbTasks; DELETE FROM tbBoards;";

                Command.Prepare();
                int num_rows_changed = Command.ExecuteNonQuery();
                Command.Dispose();
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
        /// This function creates a new file in the bin folder of the database we use, called kanbanDB
        /// </summary>
        public void CreateDataBase()
        {

            if (!File.Exists(MyPath))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(DATABASE_NAME);

                ConnectionString = $"Data Source={MyPath};Version=3;";
                Connection = new SQLiteConnection(ConnectionString);

                string CreatetbUsers = "CREATE TABLE \"tbUsers\"(" +
                    "\"Email\" TEXT NOT NULL," +
                    "\"Nickname\"  TEXT NOT NULL," +
                    "\"Password\"  TEXT NOT NULL," +
                    "PRIMARY KEY(\"Email\"))";

                string CreatetbBoards = "CREATE TABLE \"tbBoards\"(" +
                    "\"CreatorEmail\" TEXT NOT NULL," +
                    "\"IdGiver\"   INTEGER NOT NULL," +
                    "\"NumOfColumns\"  INTEGER NOT NULL," +
                    "PRIMARY KEY(\"CreatorEmail\"))";

                string CreatetbTasks = "CREATE TABLE \"tbTasks\"(" +
                    "\"TaskId\"    INTEGER NOT NULL," +
                    "\"Column\"    TEXT NOT NULL," +
                    "\"Email\" TEXT NOT NULL," +
                    "\"Title\" TEXT NOT NULL," +
                    "\"Description\"   TEXT NOT NULL," +
                    "\"CreationDate\"  TEXT NOT NULL," +
                    "\"DueDate\"   TEXT NOT NULL," +
                    "\"Assignee\" TEXT NOT NULL," +
                    "PRIMARY KEY(\"TaskId\", \"Column\", \"Email\"))";

                string CreatetbColumns = "CREATE TABLE \"tbColumns\"(" +
                    "\"Email\" TEXT NOT NULL," +
                    "\"ColumnId\"  INTEGER NOT NULL," +
                    "\"ColumnName\"    TEXT NOT NULL," +
                    "\"ColumnLimit\"   INTEGER NOT NULL," +
                    "PRIMARY KEY(\"Email\", \"ColumnName\", \"ColumnId\"))";

                try
                {
                    Connection.Open();

                    Command = new SQLiteCommand(null, Connection);

                    Command.CommandText = CreatetbUsers;
                    Command.Prepare();
                    Command.ExecuteNonQuery();

                    Command.CommandText = CreatetbBoards;
                    Command.Prepare();
                    Command.ExecuteNonQuery();

                    Command.CommandText = CreatetbTasks;
                    Command.Prepare();
                    Command.ExecuteNonQuery();

                    Command.CommandText = CreatetbColumns;
                    Command.Prepare();
                    Command.ExecuteNonQuery();

                    Command.Dispose();
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
        }

        public List<string> ImportRegisteredEmails()
        {

            List<string> EmailList = new List<string>();

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;
            try
            {
                Connection.Open();
                string sql = "SELECT Email FROM tbUsers;";
                Command = new SQLiteCommand(sql, Connection);
                DataReader = Command.ExecuteReader();
                while (DataReader.Read())
                {
                    EmailList.Add((string)DataReader[COL_EMAIL]);
                }
                DataReader.Close();
                Command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Close();
            }
            return EmailList;
        }
    }
}
