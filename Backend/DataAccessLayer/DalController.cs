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
        public void Delete()
        {
            string connetion_string = null;
            string database_name = "kanbanDB.db";
            SQLiteConnection connection;
            SQLiteCommand command = new SQLiteCommand();

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
    }
}
