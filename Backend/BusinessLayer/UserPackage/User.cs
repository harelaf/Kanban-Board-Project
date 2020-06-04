using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class User : IPersistedObject<DataAccessLayer.User>
    {
        private String Email;
        private string Password;
        private string Nickname;
        private BoardPackage.Board MyBoard;

        public User(string Email, string Password, string Nickname)
        {
            this.Email = Email;
            this.Password = Password;
            this.Nickname = Nickname;
            if (Email != null)
            {
                MyBoard = new BoardPackage.Board(Email);
            }
            else
            {
                MyBoard = new BoardPackage.Board();
            }
        }

        public void SetEmail(string Email)
        {
            this.Email = Email;
        }

        /// <summary>
        /// this function gets a Password and checks if the given Password is same as the user Password 
        /// </summary>
        /// <param name="Password"></param>
        /// <returns>returns true if the Password is correct and false if not</returns>
        public Boolean ValidatePassword(string Password)
        {
            return this.Password.Equals(Password);
        }

        /// <summary>
        /// Getter to the Email adress
        /// </summary>
        /// <returns>returns the Email adress</returns>
        public string GetEmail()
        {
            return Email;
        }

        /// <summary>
        /// Getter to the board
        /// </summary>
        /// <returns>returns the board</returns>
        public BoardPackage.Board GetBoard()
        {
            return MyBoard;
        }

        /// <summary>
        /// Getter to the Nickname of the user
        /// </summary>
        /// <returns>returns the nuckname of the user</returns>
        public string GetNickname()
        {
            return Nickname;
        }

        /// <summary>
        /// Setter to the board, this function gets new board and change the existing board to the new one
        /// </summary>
        /// <param name="newBoard"></param>
        public void SetBoard(BoardPackage.Board newBoard)
        {
            MyBoard = newBoard;
        }


        /// <summary>
        /// This function gets an Email and a column ordinal and convert the user 
        /// to a DAL user 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="colOrdinal"></param>
        /// <returns>returns a Dal user that represent this user</returns>

        public DataAccessLayer.User ToDalObject(string Email, string column)
        {
            return new DataAccessLayer.User(Email.ToLower(), Password, Nickname, MyBoard.GetColumn(0).getEmail());///////////////////////////////
        }
    }
}
