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

        string connetion_string = null;
        string sql_query = null;
        string database_name = "kanbanDB.sqlite";
        SQLiteConnection connection;
        SQLiteCommand command;

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
            throw new NotImplementedException();
        }

        public override User Import()
        {
            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            int idGiver = 0, NumOfColumns = 0, i = 0;
            List<Column> columnList;
            List<Task> taskList;
            try
            {
                connection.Open();
                string sql_query = $"SELECT * FROM tbUsers WHERE Email = {email};";
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
                    sql_query = $"SELECT * FROM tbTasks WHERE Email = {email} AND ColumnId = {i};";
                    command = new SQLiteCommand(sql_query, connection);
                    dataReader = command.ExecuteReader();
                    taskList = new List<Task>();
                    while (dataReader.Read())
                    {

                        //taskList.Add(new Task((string)dataReader["title"], (string)dataReader["description"],
                        //    new DateTime((string)dataReader["creationDate"]), dataReader["dueDate"]));
                    }
                }
                //myBoard = new Board(, idGiver);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
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
