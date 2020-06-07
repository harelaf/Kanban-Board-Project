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
        public string Email { get; set; }
        public string Name { get; set; }
        public long Ordinal { get; set; }
        public long Limit { get; set; }

        const string COL_EMAIL = "Email";
        const string COL_NAME = "ColumnName";
        const string COL_ORDINAL = "ColumnId";
        const string COL_LIMIT = "ColumnLimit";
        const string COL_TASK_EMAIL = "Email";
        const string COL_TASK_TITLE = "Title";
        const string COL_TASK_COLUMN = "Column";
        const string COL_TASK_ID = "TaskId";
        const string COL_TASK_DESC = "Description";
        const string COL_TASK_CREATION_DATE = "CreationDate";
        const string COL_TASK_DUE_DATE = "DueDate";
        const string COL_TASK_ASSIGNEE = "AssigneeEmail";

        string ConnectionString = null;
        string SqlQuery = null;
        SQLiteConnection Connection;
        SQLiteCommand Command;
        const string DATABASE_NAME = "kanbanDB.db";
        string MyPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), DATABASE_NAME));

        public Column(string Email, int Ordinal)
        {
            this.Email = Email;
            this.Ordinal = Ordinal;
        }

        public Column(string Email, string Name, int Ordinal, int Limit)
        {
            this.Email = Email;
            this.Name = Name;
            this.Ordinal = Ordinal;
            this.Limit = Limit;
        }
        
        public Column()
        {
            Email = "";
            Ordinal = 0;
            Limit = 0;
            Name = "";
        }

        /// <summary>
        /// This function saves the information of this column to the database
        /// </summary>
        public override void Save()
        {

            string ReplacedEmail = Email;
            
            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;

            try
            {
                Connection.Open();

                SqlQuery = $"SELECT * FROM tbColumns WHERE {COL_EMAIL} = '{Email}' AND {COL_NAME} = '{Name}'";

                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                if (DataReader.Read()) //This if checks if the column is already in the database, and in that case we update its information

                {
                    Command = new SQLiteCommand(null, Connection);
                    Command.CommandText = $"UPDATE tbColumns SET {COL_LIMIT} = @Limit, {COL_ORDINAL} = @ColumnOrdinal WHERE {COL_EMAIL} = @Email AND {COL_NAME} = @ColumnName";
                    SQLiteParameter ColumnNameParam = new SQLiteParameter(@"ColumnName", Name);
                    SQLiteParameter LimitParam = new SQLiteParameter(@"Limit", Limit);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter ColumnOrdinalParam = new SQLiteParameter(@"ColumnOrdinal", Ordinal);

                    Command.Parameters.Add(ColumnNameParam);
                    Command.Parameters.Add(LimitParam);
                    Command.Parameters.Add(EmailParam);
                    Command.Parameters.Add(ColumnOrdinalParam);

                    Command.Prepare();
                    int num_rows_changed = Command.ExecuteNonQuery();
                    Command.Dispose();
                }
                else //Otherwise we insert its information
                {
                    Command = new SQLiteCommand(null, Connection);
                    Command.CommandText = "INSERT INTO tbColumns VALUES(@Email,@ColumnOrdinal,@ColumnName,@Limit)";
                    
                    SQLiteParameter ColumnNameParam = new SQLiteParameter(@"ColumnName", Name);
                    SQLiteParameter LimitParam = new SQLiteParameter(@"Limit", Limit);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter ColumnOrdinalParam = new SQLiteParameter(@"ColumnOrdinal", Ordinal);

                    Command.Parameters.Add(ColumnNameParam);
                    Command.Parameters.Add(LimitParam);
                    Command.Parameters.Add(EmailParam);
                    Command.Parameters.Add(ColumnOrdinalParam);

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
        /// This function retrieves the information of a column using the users email and its the columns id
        /// </summary>
        /// <returns>A column object with all of the information from the database</returns>
        public override Column Import()
        {

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;
            try
            {
                Connection.Open();
                SqlQuery = $"SELECT * FROM tbColumns WHERE {COL_EMAIL} = '{Email}' AND {COL_ORDINAL} = {Ordinal}";

                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                if (DataReader.Read())
                {
                    Name = (string)DataReader[COL_NAME];
                    Limit = (long)DataReader[COL_LIMIT];
                    Ordinal = (long)DataReader[COL_ORDINAL];
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

        /// <summary>
        /// This function retrieves all of the tasks that are in a specific column
        /// </summary>
        /// <returns>A list of tasks containing every task in that column</returns>
        public List<Task> getTasks()
        {
            List<Task> taskList = new List<Task>();

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;
            try
            {
                Connection.Open();
                SqlQuery = $"SELECT * FROM tbTasks WHERE {COL_TASK_EMAIL} = '{Email}' AND {COL_TASK_COLUMN} = '{Name}' ORDER BY {COL_TASK_CREATION_DATE};";

                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                while (DataReader.Read())
                {
                    DateTime CreationDate = DateTime.Parse((string)DataReader[COL_TASK_CREATION_DATE]);
                    DateTime DueDate = DateTime.Parse((string)DataReader[COL_TASK_DUE_DATE]);
                    taskList.Add(new Task((string)DataReader[COL_TASK_TITLE], (string)DataReader[COL_TASK_DESC], CreationDate, DueDate, (int)((long)DataReader[COL_TASK_ID]), (string)DataReader[COL_TASK_COLUMN], (string)DataReader[COL_TASK_EMAIL], (string)DataReader[COL_TASK_ASSIGNEE]));
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
            
            return taskList;
        }

        /// <summary>
        /// This function deletes a column from the database
        /// </summary>
        public override void Delete()
        {

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);

            try
            {
                Connection.Open();

                Command = new SQLiteCommand(null, Connection);
                Command.CommandText = $"DELETE FROM tbColumns WHERE {COL_EMAIL} = @Email AND {COL_NAME} = @ColumnName";
                SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                SQLiteParameter ColumnNameParam = new SQLiteParameter(@"ColumnName", Name);

                Command.Parameters.Add(EmailParam);
                Command.Parameters.Add(ColumnNameParam);

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
    }
}
