using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        public int colOrdinal { get; set; }
        public string Email { get; set; }

        public Task(string Title,string Description,DateTime CreationDate,DateTime DueDate,int TaskId, int colOrdinal, string Email)
        {
            this.Title = Title;
            this.Description = Description;
            this.CreationDate = CreationDate;
            this.DueDate = DueDate;
            this.TaskId = TaskId;
            this.colOrdinal = colOrdinal;
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
            columnId = 0;
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
                sql_query = $"SELECT * FROM tbTasks WHERE Email = {Email} AND taskId = {TaskId}";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = "UPDATE tbTasks SET column = @column, title = @title, descrption = @description, creationDate = @creationDate, dueDate = @dueDate WHERE Email = @Email AND taskId = @taskId";
                    SQLiteParameter columnParam = new SQLiteParameter(@"column", columnId);
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
                    command.CommandText = "INSERT INTO tbTasks VALUES(@Email,@nickName,@password,@IdGiver,@numOfColumns)";
                    SQLiteParameter IdGiverParam = new SQLiteParameter(@"idGiver", myBoard.idGiver);
                    SQLiteParameter numOfColumnsParam = new SQLiteParameter(@"numOfColumns", myBoard.columnList.Count);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);
                    SQLiteParameter NicknameParam = new SQLiteParameter(@"nickName", nickname);
                    SQLiteParameter PasswordParam = new SQLiteParameter(@"password", password);

                    command.Parameters.Add(IdGiverParam);
                    command.Parameters.Add(numOfColumnsParam);
                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(NicknameParam);
                    command.Parameters.Add(PasswordParam);

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

        public override Task Import()
        {
            throw new NotImplementedException();
        }
    }
}
