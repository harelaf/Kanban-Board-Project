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
        const string database_name = "kanbanDB.db";

        public void Delete()
        {

            string connetion_string = null;
            SQLiteConnection connection;
            SQLiteCommand command;

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));

            connetion_string = $"Data Source={path};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            try
            {
                connection.Open();

                command = new SQLiteCommand(null, connection);
                command.CommandText = "DELETE FROM tbColumns; DELETE FROM tbUsers; DELETE FROM tbTasks;";

                command.Prepare();
                int num_rows_changed = command.ExecuteNonQuery();
                command.Dispose();
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

        public void CreateDataBase()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));
            if (!File.Exists(path))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(database_name);

                path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));

                string connetion_string = null;
                SQLiteConnection connection;
                SQLiteCommand command;

                connetion_string = $"Data Source={path};Version=3;";
                connection = new SQLiteConnection(connetion_string);

                string createtbUsers = "CREATE TABLE \"tbUsers\"(" +
                    "\"Email\" TEXT NOT NULL," +
                    "\"nickName\"  TEXT NOT NULL," +
                    "\"password\"  TEXT NOT NULL," +
                    "\"IdGiver\"   INTEGER NOT NULL," +
                    "\"numOfColumns\"  INTEGER NOT NULL," +
                    "PRIMARY KEY(\"Email\"))";

                string createtbTasks = "CREATE TABLE \"tbTasks\"(" +
                    "\"TaskId\"    INTEGER NOT NULL," +
                    "\"column\"    TEXT NOT NULL," +
                    "\"Email\" TEXT NOT NULL," +
                    "\"title\" TEXT NOT NULL," +
                    "\"description\"   TEXT NOT NULL," +
                    "\"creationDate\"  TEXT NOT NULL," +
                    "\"dueDate\"   TEXT NOT NULL," +
                    "PRIMARY KEY(\"TaskId\", \"column\", \"Email\"))";

                string createtbColumns = "CREATE TABLE \"tbColumns\"(" +
                    "\"Email\" TEXT NOT NULL," +
                    "\"columnId\"  INTEGER NOT NULL," +
                    "\"columnName\"    TEXT NOT NULL," +
                    "\"columnLimit\"   INTEGER NOT NULL," +
                    "PRIMARY KEY(\"Email\", \"columnName\", \"columnId\"))";

                try
                {
                    connection.Open();

                    command = new SQLiteCommand(null, connection);

                    command.CommandText = createtbUsers;
                    command.Prepare();
                    command.ExecuteNonQuery();

                    command.CommandText = createtbTasks;
                    command.Prepare();
                    command.ExecuteNonQuery();

                    command.CommandText = createtbColumns;
                    command.Prepare();
                    command.ExecuteNonQuery();

                    command.Dispose();
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
        }
    }
}
