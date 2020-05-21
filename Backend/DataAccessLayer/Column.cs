using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Column : DalObject<Column>
    {
        public string email { get; set; }
        public string Name { get; set; }
        public long ordinal { get; set; }
        public long Limit { get; set; }

        const string colEmail = "Email";
        const string colName = "columnName";
        const string colOrdinal = "columnId";
        const string colLimit = "columnLimit";
        const string colTaskEmail = "Email";
        const string colTaskTitle = "Title";
        const string colTaskColumn = "column";
        const string colTaskId = "TaskId";
        const string colTaskDesc = "Description";
        const string colTaskCreationDate = "CreationDate";
        const string colTaskDueDate = "DueDate";

        public Column(string Email, int ordinal)
        {
            this.email = Email;
            this.ordinal = ordinal;
        }

        public Column(string Email, string Name, int ordinal, int Limit)
        {
            this.email = Email;
            this.Name = Name;
            this.ordinal = ordinal;
            this.Limit = Limit;
        }
        
        public Column()
        {
            email = "";
            ordinal = 0;
            Limit = 0;
            Name = "";
        }

        public override void Save()
        {
            string connetion_string = null;
            string sql_query = null;
            string database_name = "kanbanDB.db";
            SQLiteConnection connection;
            SQLiteCommand command = new SQLiteCommand();
            string ReplacedEmail = email;

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));

            connetion_string = $"Data Source={path};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                sql_query = $"SELECT * FROM tbColumns WHERE {colEmail} = '{email}' AND {colName} = '{Name}'";

                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = $"UPDATE tbColumns SET {colLimit} = @limit, {colOrdinal} = @columnOrdinal WHERE {colEmail} = @Email AND {colName} = @columnName";
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnName", Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limit", Limit);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinal", ordinal);

                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(columnOrdinalParam);

                    command.Prepare();
                    int num_rows_changed = command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = $"INSERT INTO tbColumns ({colEmail},{colOrdinal},{colName},{colLimit}) VALUES(@Email,@columnOrdinal,@columnName,@limit)";
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnName", Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limit", Limit);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinal", ordinal);

                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(columnOrdinalParam);

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

        public override Column Import()
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
                sql_query = $"SELECT * FROM tbColumns WHERE {colEmail} = '{email}' AND {colOrdinal} = {ordinal};";

                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    Name = (string)dataReader[colName];
                    Limit = (long)dataReader[colLimit];
                    ordinal = (long)dataReader[colOrdinal];
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

        public List<Task> getTasks()
        {
            List<Task> taskList = new List<Task>();
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
                sql_query = $"SELECT * FROM tbTasks WHERE {colTaskEmail} = '{email}' AND {colTaskColumn} = '{Name}' ORDER BY {colTaskCreationDate};";

                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    DateTime creationDate = DateTime.Parse((string)dataReader[colTaskCreationDate]);
                    DateTime dueDate = DateTime.Parse((string)dataReader[colTaskDueDate]);
                    taskList.Add(new Task((string)dataReader[colTaskTitle], (string)dataReader[colTaskDesc], creationDate, dueDate, (int)((long)dataReader[colTaskId]), (string)dataReader[colTaskColumn], (string)dataReader[colTaskEmail]));
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
            
            return taskList;
        }

        public override void Delete()
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
                command.CommandText = $"DELETE FROM tbColumns WHERE {colEmail} = @Email AND {colName} = @columnName";
                SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                SQLiteParameter columnNameParam = new SQLiteParameter(@"columnName", Name);

                command.Parameters.Add(EmailParam);
                command.Parameters.Add(columnNameParam);

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
