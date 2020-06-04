using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class User : IPersistedObject<DataAccessLayer.User>
    {
        private String email;
        private string password;
        private string nickname;
        private BoardPackage.Board myBoard;

        public User(string email, string password, string nickname)
        {
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            if (email != null)
            {
                myBoard = new BoardPackage.Board(email);
            }
            else
            {
                myBoard = new BoardPackage.Board();
            }
        }

        public void SetEmail(string Email)
        {
            this.email = Email;
        }

        /// <summary>
        /// this function gets a password and checks if the given password is same as the user password 
        /// </summary>
        /// <param name="password"></param>
        /// <returns>returns true if the password is correct and false if not</returns>
        public Boolean ValidatePassword(string password)
        {
            return this.password.Equals(password);
        }

        /// <summary>
        /// Getter to the email adress
        /// </summary>
        /// <returns>returns the email adress</returns>
        public string GetEmail()
        {
            return email;
        }

        /// <summary>
        /// Getter to the board
        /// </summary>
        /// <returns>returns the board</returns>
        public BoardPackage.Board GetBoard()
        {
            return myBoard;
        }

        /// <summary>
        /// Getter to the nickname of the user
        /// </summary>
        /// <returns>returns the nuckname of the user</returns>
        public string GetNickname()
        {
            return nickname;
        }

        /// <summary>
        /// Setter to the board, this function gets new board and change the existing board to the new one
        /// </summary>
        /// <param name="newBoard"></param>
        public void SetBoard(BoardPackage.Board newBoard)
        {
            myBoard = newBoard;
        }


        /// <summary>
        /// This function gets an email and a column ordinal and convert the user 
        /// to a DAL user 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="colOrdinal"></param>
        /// <returns>returns a Dal user that represent this user</returns>

        public DataAccessLayer.User ToDalObject(string Email, string column)
        {
            return new DataAccessLayer.User(email.ToLower(), password, nickname, myBoard.GetColumn(0).getEmail());///////////////////////////////
        }
    }
}
