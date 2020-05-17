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
        public int idGiver { get; set; }
        public int numOfColumns { get; set; }
        public DalController dalController;


        const string colEmail = "Email";
        const string colPassword = "password";
        const string colNickname = "nickName";
        const string colIdGiver = "idGiver";
        const string colNumOfColumns = "NumOfColumns";


        public User()
        {
            email = null;
            password = null;
            nickname = null;
            dalController = new DalController();
        }

        public User(string email, string password, string nickname)
        {
            this.email = email.ToLower();
            this.password = password;
            this.nickname = nickname;
            dalController = new DalController();
        }

        public User(string email)
        {
            this.email = email.ToLower();
            password = null;
            nickname = null;
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
            SQLiteCommand command = null;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;
            int idGiver = 0, NumOfColumns = 0, i = 0;
            try
            {
                connection.Open();
                sql_query = $"SELECT * FROM tbUsers WHERE Email = {email};";
                command = new SQLiteCommand(sql_query, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    email = (string)dataReader[colEmail];
                    password = (string)dataReader[colPassword];
                    nickname = (string)dataReader[colNickname];
                    idGiver = (int)dataReader[colIdGiver];
                    NumOfColumns = (int)dataReader[colNumOfColumns];
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

        public List<Column> GetColumns()
        {
            int i = 0;
            List<Column> colList = new List<Column>(numOfColumns);
            Column col;
            while (i < numOfColumns)
            {
                col = new Column(email, i);
                colList.Add(col.Import());
            }
            return colList;
        }
    }
}
