using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Column : DalObject<Column>
    {
        public string email { get; set; }
        public string Name { get; set; }
        public int ordinal { get; set; }
        public int Limit { get; set; }

        const string colEmail = "Email";
        const string colName = "Name";
        const string colOrdinal = "Ordinal";
        const string colLimit = "Limit";
        const string colTaskEmail = "Email";
        const string colTaskTitle = "Title";
        const string colTaskColumn = "ColumnOrdinal";
        const string colTaskId = "TaskId";
        const string colTaskDesc = "Description";
        const string colTaskCreationDate = "CreationDate";
        const string colTaskDueDate = "DueDate";

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Column(string Email, int ordinal)
        {
            this.email = Email;
            this.ordinal = ordinal;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
            string database_name = "kanbanDB.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command = new SQLiteCommand();

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            try
            {
                sql_query = $"SELECT * FROM tbColumns WHERE Email = {email} AND columnId = {ordinal}";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = $"UPDATE tbColumns SET {colName} = @columnName, {colLimit} = @limit, {colOrdinal} = @columnOrdinal WHERE {colEmail} = @Email AND {colOrdinal} = @columnId";
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnId", ordinal);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnName", Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limit", Limit);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinal", ordinal);

                    command.Parameters.Add(columnIdParam);
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
                    command.CommandText = "INSERT INTO tbColumns VALUES(@Email,@columnId,@columnOrdinal,@columnName,@limit)";
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnId", ordinal);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnName", Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limit", Limit);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinal", ordinal);

                    command.Parameters.Add(columnIdParam);
                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(columnOrdinalParam);

                    command.Prepare();
                    int num_rows_changed = command.ExecuteNonQuery();
                    command.Dispose();
                }
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
            string database_name = "kanbanDB.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command = null;


            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            try
            {
                connection.Open();
                sql_query = $"SELECT * FROM tbColumns WHERE {colEmail} = {email} AND {colOrdinal} = {ordinal};";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    Name = (string)dataReader[colName];
                    Limit = (int)dataReader[colLimit];
                    ordinal = (int)dataReader[colOrdinal];
                }
            }
            catch (Exception)
            {
                email = null;
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
            string database_name = "kanbanDB.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command = null;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            try
            {
                connection.Open();
                sql_query = $"SELECT * FROM tbTasks WHERE {colTaskEmail} = {email} AND {colTaskColumn} = {Name} ORDER BY {colTaskCreationDate};";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    DateTime creationDate = DateTime.Parse((string)dataReader[colTaskCreationDate]);
                    DateTime dueDate = DateTime.Parse((string)dataReader[colTaskDueDate]);
                    taskList.Add(new Task((string)dataReader[colTaskTitle], (string)dataReader[colTaskDesc], creationDate, dueDate, (int)dataReader[colTaskId], (string)dataReader[colTaskColumn], (string)dataReader[colTaskEmail]));
                }
            }
            catch (Exception)
            {
                email = null;
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
            string database_name = "kanbanDB.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command = new SQLiteCommand();

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);

            try
            {
                command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM tbColumns WHERE {colEmail} = @Email AND {colOrdinal} = @columnId";
                SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                SQLiteParameter columnIdParam = new SQLiteParameter(@"columnId", ordinal);

                command.Parameters.Add(EmailParam);
                command.Parameters.Add(columnIdParam);

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
