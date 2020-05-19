using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Task : DalObject<Task>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public int TaskId { get; set; }
        public String columnName { get; set; }
        public string Email { get; set; }

        const string colTaskId = "TaskId";
        const string colTaskEmail = "Email";
        const string colTaskTitle = "Title";
        const string colTaskColumn = "column";
        const string colTaskDesc = "description";
        const string colTaskCreationDate = "creationDate";
        const string colTaskDueDate = "dueDate";

        public Task(string Title,string Description,DateTime CreationDate,DateTime DueDate,int TaskId, string columnName, string Email)
        {
            this.Title = Title;
            this.Description = Description;
            this.CreationDate = CreationDate;
            this.DueDate = DueDate;
            this.TaskId = TaskId;
            this.columnName = columnName;
            this.Email = Email;
        }

        public Task()
        {
            Title = null;
            Description = null;
            CreationDate = new DateTime();
            DueDate = new DateTime();
            TaskId = 0;
            Email = "";
            columnName = "";
        }

        public override void Save()
        {
            string connetion_string = null;
            string sql_query = null;
            string database_name = "kanbanDB.db";
            SQLiteConnection connection;
            SQLiteCommand command = new SQLiteCommand();

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), database_name));

            connetion_string = $"Data Source={path};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                sql_query = $"SELECT * FROM tbTasks WHERE {colTaskEmail} = '{Email}' AND {colTaskId} = '{TaskId}'";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = $"UPDATE tbTasks SET {colTaskColumn} = @column, {colTaskTitle} = @title, {colTaskDesc} = @description, {colTaskCreationDate} = @creationDate, {colTaskDueDate} = @dueDate WHERE {colTaskEmail} = @Email AND {colTaskId} = @taskId";
                    SQLiteParameter columnParam = new SQLiteParameter(@"column", columnName);
                    SQLiteParameter titleParam = new SQLiteParameter(@"title", Title);
                    SQLiteParameter descrptionParam = new SQLiteParameter(@"description", Description);
                    SQLiteParameter creationDateParam = new SQLiteParameter(@"creationDate", CreationDate.ToString());
                    SQLiteParameter dueDateparam = new SQLiteParameter(@"dueDate", DueDate.ToString());
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter taskidParam = new SQLiteParameter(@"taskId", TaskId);

                    command.Parameters.Add(columnParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descrptionParam);
                    command.Parameters.Add(creationDateParam);
                    command.Parameters.Add(dueDateparam);
                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(taskidParam);

                    command.Prepare();
                    int num_rows_changed = command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = "INSERT INTO tbTasks VALUES(@TaskId,@column,@Email,@title,@description,@creationDate,@dueDate)";
                    SQLiteParameter TaskIdParam = new SQLiteParameter(@"TaskId", TaskId);
                    SQLiteParameter columnParam = new SQLiteParameter(@"column", columnName);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter titleParam = new SQLiteParameter(@"title", Title);
                    SQLiteParameter descrptionParam = new SQLiteParameter(@"description", Description);
                    SQLiteParameter creationDateParam = new SQLiteParameter(@"creationDate", CreationDate.ToString());
                    SQLiteParameter dueDateparam = new SQLiteParameter(@"dueDate", DueDate.ToString());

                    command.Parameters.Add(TaskIdParam);
                    command.Parameters.Add(columnParam);
                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descrptionParam);
                    command.Parameters.Add(creationDateParam);
                    command.Parameters.Add(dueDateparam);

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

        public override Task Import()
        {
            throw new NotImplementedException();
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
                command.CommandText = $"DELETE FROM tbTasks WHERE {colTaskEmail} = @Email AND {colTaskId} = @taskId";
                SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                SQLiteParameter taskIdParam = new SQLiteParameter(@"taskId", TaskId);

                command.Parameters.Add(EmailParam);
                command.Parameters.Add(taskIdParam);

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
