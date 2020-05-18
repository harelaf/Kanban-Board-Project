using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class User : DalObject<User>
    {
        public string email { get; set; }
        public string password { get; set; }
        public string nickname { get; set; }
        public Board myBoard { get; set; }
        public DalController dalController;

        public User()
        {
            email = null;
            password = null;
            nickname = null;
            myBoard = null;
            dalController = new DalController();
        }

        public User(string email, string password, string nickname, Board myBoard)
        {
            this.email = email.ToLower();
            this.password = password;
            this.nickname = nickname;
            this.myBoard = myBoard;
            dalController = new DalController();
        }

        public User(string email)
        {
            this.email = email.ToLower();
            password = null;
            nickname = null;
            myBoard = null;
            dalController = new DalController();
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
                sql_query = $"SELECT * FROM tbUsers WHERE Email = {email}";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = "UPDATE tbUsers SET IdGiver = @idGiver, numOfColumns = @numOfColumns WHERE Email = @Email";
                    SQLiteParameter IdGiverParam = new SQLiteParameter(@"idGiver", myBoard.idGiver);
                    SQLiteParameter numOfColumnsParam = new SQLiteParameter(@"numOfColumns", myBoard.columnList.Count);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", email);

                    command.Parameters.Add(IdGiverParam);
                    command.Parameters.Add(numOfColumnsParam);
                    command.Parameters.Add(EmailParam);

                    command.Prepare();
                    int num_rows_changed = command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    command = new SQLiteCommand(null, connection);
                    command.CommandText = "INSERT INTO tbUsers VALUES(@Email,@nickName,@password,@IdGiver,@numOfColumns)";
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

        public override User Import()
        {
            string connetion_string = null;
            string sql_query = null;
            string database_name = "kanbanDB.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command = new SQLiteCommand();

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            int idGiver = 0, NumOfColumns = 0, i = 0, colLimit = 0;
            string columnName = "";
            List<Column> columnList = new List<Column>();
            List<Task> taskList = new List<Task>();
            myBoard = new Board();
            try
            {
                connection.Open();
                sql_query = $"SELECT * FROM tbUsers WHERE Email = {email};";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    email = (string)dataReader["Email"];
                    password = (string)dataReader["password"];
                    nickname = (string)dataReader["nickName"];
                    idGiver = (int)dataReader["idGiver"];
                    NumOfColumns = (int)dataReader["NumOfColumns"];
                }
                while (i < NumOfColumns)
                {
                    taskList = new List<Task>();
                    sql_query = $"SELECT * FROM tbTasks WHERE Email = {email} AND ColumnId = {i};";
                    command = new SQLiteCommand(sql_query, connection);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Task toAdd = new Task((string)dataReader["title"], (string)dataReader["description"],
                            DateTime.Parse((string)dataReader["creationDate"]), DateTime.Parse((string)dataReader["dueDate"]), (int)dataReader["taskId"], email, i);
                        taskList.Add(toAdd);
                    }
                    sql_query = $"SELECT * FROM tbColumns WHERE Email = {email} AND ColumnId = {i}";
                    command = new SQLiteCommand(sql_query, connection);
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        colLimit = (int)dataReader["limit"];
                        columnName = (string)dataReader["columnName"];
                    }
                    Column newCol = new Column(taskList, colLimit, columnName, email, i);
                    columnList.Add(newCol);
                    i++;
                }
                myBoard = new Board(columnList, idGiver);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return this;
        }
    }
}
