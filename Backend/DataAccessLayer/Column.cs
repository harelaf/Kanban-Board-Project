using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Column : DalObject<Column>
    {
        public string email { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Limit { get; set; }

        const string colEmail = "Email";
        const string colName = "Name";
        const string colId = "Id";
        const string colLimit = "Limit";
        const string colTaskEmail = "Email";
        const string colTaskTitle = "Title";
        const string colTaskId = "taskId";
        const string colTaskDesc = "description";
        const string colTaskCreationDate = "creationDate";
        const string colTaskDueDate = "dueDate";

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Column(string Email, int colId)
        {
            this.email = email;
            this.Id = colId;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Column(string Email, string Name, int Id, int Limit)
        {
            this.email = email;
            this.Name = Name;
            this.Id = Id;
            this.Limit = Limit;
        }

        public Column()
        {
            email = "";
            Id = 0;
            Limit = 0;
            Name = "";
        }

        public override void Save()
        {
            throw new NotImplementedException();
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
                sql_query = $"SELECT * FROM tbColumns WHERE {colEmail} = {email} AND {colId} = {Id};";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    Name = (string)dataReader[colName];
                    Limit = (int)dataReader[colLimit];
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
                sql_query = $"SELECT * FROM tbTasks WHERE {colTaskEmail} = {email} AND {colTaskId} = {Id} ORDER BY {colTaskCreationDate};";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    DateTime creationDate = DateTime.Parse((string)dataReader[colTaskCreationDate]);
                    DateTime dueDate = DateTime.Parse((string)dataReader[colTaskDueDate]);
                    taskList.Add(new Task((string)dataReader[colTaskTitle], (string)dataReader[colTaskDesc], creationDate, dueDate, (int)dataReader[colTaskId], (int)dataReader[colTaskId], (string)dataReader[colTaskEmail]));
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
    }
}
