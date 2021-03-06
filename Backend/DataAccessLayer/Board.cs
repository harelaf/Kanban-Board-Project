using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{

    class Board : DalObject<Board>
    {
        public string CreatorEmail {get; set;}
        public long NumOfColumns { get; set; }
        public long IdGiver { get; set; }

        const string COL_CREATOR_EMAIL = "CreatorEmail";
        const string COL_NUM_OF_COLUMNS = "NumOfColumns";
        const string COL_ID_GIVER = "IdGiver";
        const string COL_USER_HOST_EMAIL = "HostEmail";
        const string COL_USER_EMAIL = "Email";

        string ConnectionString = null;
        string SqlQuery = null;
        SQLiteConnection Connection;
        SQLiteCommand Command;
        const string DATABASE_NAME = "kanbanDB.db";
        string MyPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), DATABASE_NAME));

        public Board()
        {
            CreatorEmail = "";
            NumOfColumns = 0;
            IdGiver = 0;
        }

        public Board(string CreatorEmail, int IdGiver, int NumOfColumns)
        {
            this.CreatorEmail = CreatorEmail;
            this.NumOfColumns = NumOfColumns;
            this.IdGiver = IdGiver;
        }

        public Board(string CreatorEmail)
        {
            this.CreatorEmail = CreatorEmail;
            NumOfColumns = 0;
            IdGiver = 0;
        }

        public override void Save()
        {

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;

            try
            {
                Connection.Open();

                SqlQuery = $"SELECT * FROM tbBoards WHERE {COL_CREATOR_EMAIL} = '{CreatorEmail}'";
                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                if (!DataReader.Read())
                {
                    SaveDoesntExist(Connection);   
                }
                else
                {
                    SaveExists(Connection);
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
        /// This function saves to the database a new board
        /// </summary>
        /// <param name="Connection">Recieves the connection to the database</param>
        private void SaveDoesntExist(SQLiteConnection Connection)
        {
            if(Connection.State == System.Data.ConnectionState.Open)
            {
                Command = new SQLiteCommand(null, Connection);
                Command.CommandText = "INSERT INTO tbBoards VALUES(@CreatorEmail,@NumOfColumns,@IdGiver)";
                SQLiteParameter CreatorEmailParam = new SQLiteParameter(@"CreatorEmail", CreatorEmail);
                SQLiteParameter IdGiverParam = new SQLiteParameter(@"IdGiver", IdGiver);
                SQLiteParameter NumOfColumnsParam = new SQLiteParameter(@"NumOfColumns", NumOfColumns);

                Command.Parameters.Add(CreatorEmailParam);
                Command.Parameters.Add(IdGiverParam);
                Command.Parameters.Add(NumOfColumnsParam);

                Command.Prepare();
                int num_rows_changed = Command.ExecuteNonQuery();
                Command.Dispose();
            }
        }

        /// <summary>
        /// This function saves to the database an already existing board
        /// </summary>
        /// <param name="Connection">Recieves the connection to the database</param>
        private void SaveExists(SQLiteConnection Connection)
        {
            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Command = new SQLiteCommand(null, Connection);
                Command.CommandText = $"UPDATE tbBoards SET {COL_NUM_OF_COLUMNS} = @NumOfColumns, {COL_ID_GIVER} = @IdGiver WHERE {COL_CREATOR_EMAIL} = @Email";

                SQLiteParameter NumOfColumnsParam = new SQLiteParameter(@"NumOfColumns", NumOfColumns);
                SQLiteParameter IdGiverParam = new SQLiteParameter(@"IdGiver", IdGiver);
                SQLiteParameter CreatorEmailParam = new SQLiteParameter(@"Email", CreatorEmail);

                Command.Parameters.Add(NumOfColumnsParam);
                Command.Parameters.Add(IdGiverParam);
                Command.Parameters.Add(CreatorEmailParam);

                Command.Prepare();
                int num_rows_changed = Command.ExecuteNonQuery();
                Command.Dispose();
            }
        }

        public override Board Import()
        {

            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;
            try
            {
                Connection.Open();
                SqlQuery = $"SELECT * FROM tbBoards WHERE {COL_CREATOR_EMAIL} = '{CreatorEmail}'";

                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                if (DataReader.Read())
                {
                    NumOfColumns = (long)DataReader[COL_NUM_OF_COLUMNS];
                    IdGiver = (long)DataReader[COL_ID_GIVER];
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

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public List<Column> GetColumns()
        {
            int i = 0;
            List<Column> ColList = new List<Column>();
            Column Col;
            while (i < NumOfColumns)
            {
                Col = new Column(CreatorEmail,i);
                ColList.Add(Col.Import());
                i++;
            }
            return ColList;
        }

        public List<string> GetMembers()
        {
            List<string> Members = new List<string>();
            ConnectionString = $"Data Source={MyPath};Version=3;";
            Connection = new SQLiteConnection(ConnectionString);
            SQLiteDataReader DataReader;
            try
            {
                Connection.Open();
                SqlQuery = $"SELECT * FROM tbUsers WHERE {COL_USER_HOST_EMAIL} = '{CreatorEmail}'";

                Command = new SQLiteCommand(SqlQuery, Connection);
                DataReader = Command.ExecuteReader();
                while(DataReader.Read())
                {
                    Members.Add((string)DataReader[COL_USER_EMAIL]);
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
            return Members;
        }
    }
}
