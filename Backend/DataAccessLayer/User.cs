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
            throw new NotImplementedException();
        }

        public override User Import()
        {
            string connetion_string = null;
            string sql_query = null;
            string database_name = "kanbanDB.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                string sql = "SELECT * FROM dbUsers WHERE Email = " + email;
                SQLiteCommand c = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = c.ExecuteReader();
            }
            catch (Exception e)
            {

            }
        }
    }
}
