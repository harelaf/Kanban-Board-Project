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
        public List<Task> TaskList { get; set; }
        public int Limit { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int id { get; set; }

        public Column(List<Task>TaskList, int Limit, string Name, string Email, int id)
        {
            this.TaskList = TaskList;
            this.Limit = Limit;
            this.Name = Name;
            this.Email = Email;
            this.id = id;
        }
        
        public Column()
        {
            TaskList=new List<Task>();
            Limit = 0;
            Name = "";
            Email = "";
            id = 0;
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
                sql_query = $"SELECT * FROM tbColumns WHERE Email = {Email} AND columnId = {id}";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = "UPDATE tbColumns SET columnId = @columnId, columnName = @columnName, limit = @limit WHERE Email = @Email AND columnId = @columnId";
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnId", id);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnName", Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limit", Limit);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnId", id);

                    command.Parameters.Add(columnIdParam);
                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(EmailParam);

                    command.Prepare();
                    int num_rows_changed = command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = "INSERT INTO tbColumns VALUES(@Email,@columnId,@columnName,@limit)";
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnId", id);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", Email);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnName", Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limit", Limit);

                    command.Parameters.Add(columnIdParam);
                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(limitParam);

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
            throw new NotImplementedException();
        }
    }
}
