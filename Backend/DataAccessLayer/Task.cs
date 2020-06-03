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
        public String ColumnName { get; set; }
        public string Email { get; set; }
        public string Assignee { get; set; }

        const string COL_TASK_EMAIL = "Email";
        const string COL_TASK_TITLE = "Title";
        const string COL_TASK_COLUMN = "Column";
        const string COL_TASK_ID = "TaskId";
        const string COL_TASK_DESC = "Description";
        const string COL_TASK_CREATION_DATE = "CreationDate";
        const string COL_TASK_DUE_DATE = "DueDate";
        const string COL_TASK_ASSIGNEE = "Assignee";

        string ConnetionString = null;
        string SqlQuery = null;
        SQLiteConnection Connection;
        SQLiteCommand Command;
        const string DATABASE_NAME = "kanbanDB.db";
        const string Path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), DATABASE_NAME));

        public Task(string Title,string Description,DateTime CreationDate,DateTime DueDate,int TaskId, string ColumnName, string Email, string Assignee)
        {
            this.Title = Title;
            this.Description = Description;
            this.CreationDate = CreationDate;
            this.DueDate = DueDate;
            this.TaskId = TaskId;
            this.ColumnName = ColumnName;
            this.Email = Email;
            this.Assignee = Assignee;
        }

        public Task()
        {
            this.Title = null;
            this.Description = null;
            this.CreationDate = new DateTime();
            this.DueDate = new DateTime();
            this.TaskId = null;
            this.ColumnName = null;
            this.Email = "";
            this.Assignee = "";
        }

        public override void Save()
        {
            
            ConnetionString = $"Data Source={path};Version=3;";
            Connection = new SQLiteConnection(ConnetionString);
            SQLiteDataReader DataReader;

            try
            {
                Connection.Open();

                SqlQuery = $"SELECT * FROM tbTasks WHERE {COL_TASK_EMAIL} = '{Email}' AND {COL_TASK_ID} = '{TaskId}'";
                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                if (DataReader.Read())
                {
                    Command = new SQLiteCommand(null, Connection);
                    Command.CommandText = $"UPDATE tbTasks SET {COL_TASK_COLUMN} = @Column, {COL_TASK_TITLE} = @Title, {COL_TASK_DESC} = @Description, {COL_TASK_CREATION_DATE} = @DreationDate, {COL_TASK_DUE_DATE} = @DueDate, {COL_TASK_ASSIGNEE} = @Assignee WHERE {COL_TASK_EMAIL} = @Email AND {COL_TASK_ID} = @taskId";
                    SQLiteParameter ColumnParam = new SQLiteParameter(@"Column", ColumnName);
                    SQLiteParameter TitleParam = new SQLiteParameter(@"Title", Title);
                    SQLiteParameter DescrptionParam = new SQLiteParameter(@"Description", Description);
                    SQLiteParameter CreationDateParam = new SQLiteParameter(@"CreationDate", CreationDate.ToString());
                    SQLiteParameter DueDateparam = new SQLiteParameter(@"DueDate", DueDate.ToString());
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter TaskIdParam = new SQLiteParameter(@"TaskId", TaskId);
                    SQLiteParameter AssigneeParam = new SQLiteParameter(@"Assignee", Assignee);

                    Command.Parameters.Add(ColumnParam);
                    Command.Parameters.Add(TitleParam);
                    Command.Parameters.Add(DescrptionParam);
                    Command.Parameters.Add(CreationDateParam);
                    Command.Parameters.Add(DueDateparam);
                    Command.Parameters.Add(EmailParam);
                    Command.Parameters.Add(TaskidParam);
                    Command.Parameters.Add(AssigneeParam);


                    Command.Prepare();
                    int num_rows_changed = Command.ExecuteNonQuery();
                    Command.Dispose();
                }
                else
                {
                    Command = new SQLiteCommand(null, Connection);
                    Command.CommandText = "INSERT INTO tbTasks VALUES(@TaskId,@Column,@Email,@Title,@Description,@CreationDate,@DueDate,@Assignee)";
                    SQLiteParameter ColumnParam = new SQLiteParameter(@"Column", ColumnName);
                    SQLiteParameter TitleParam = new SQLiteParameter(@"Title", Title);
                    SQLiteParameter DescrptionParam = new SQLiteParameter(@"Description", Description);
                    SQLiteParameter CreationDateParam = new SQLiteParameter(@"CreationDate", CreationDate.ToString());
                    SQLiteParameter DueDateparam = new SQLiteParameter(@"DueDate", DueDate.ToString());
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter TaskIdParam = new SQLiteParameter(@"TaskId", TaskId);
                    SQLiteParameter AssigneeParam = new SQLiteParameter(@"Assignee", Assignee);

                    Command.Parameters.Add(ColumnParam);
                    Command.Parameters.Add(TitleParam);
                    Command.Parameters.Add(DescrptionParam);
                    Command.Parameters.Add(CreationDateParam);
                    Command.Parameters.Add(DueDateparam);
                    Command.Parameters.Add(EmailParam);
                    Command.Parameters.Add(TaskidParam);
                    Command.Parameters.Add(AssigneeParam);

                    Command.Prepare();
                    int Num_Rows_Changed = Command.ExecuteNonQuery();
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

        public override Task Import()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            ConnetionString = $"Data Source={Path};Version=3;";
            Connection = new SQLiteConnection(ConnetionString);
            Command = new SQLiteCommand();

            try
            {
                Connection.Open();

                Command = new SQLiteCommand(null, Connection);
                Command.CommandText = $"DELETE FROM tbTasks WHERE {COL_TASK_EMAIL} = @Email AND {COL_TASK_ID} = @taskId";
                SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                SQLiteParameter taskIdParam = new SQLiteParameter(@"taskId", TaskId);

                Command.Parameters.Add(EmailParam);
                Command.Parameters.Add(taskIdParam);

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
