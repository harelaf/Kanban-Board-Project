using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    namespace UserPackage
    {

        class UserController
        {
            private Dictionary<string, User> UserList;
            private User CurrentUser;
            private RegisteredEmails registeredEmails;

            public UserController()
            {
                UserList = new Dictionary<string, User>();
                CurrentUser = null;
                registeredEmails = new RegisteredEmails();
            }

            public User Login(string Email, string Password)
            {
                if (UserList.ContainsKey(Email))
                {
                    User MyUser = UserList[Email];
                    if (MyUser.ValidatePassword(Password))
                    {
                        CurrentUser = MyUser;
                        return MyUser;
                    }
                    else
                        throw new Exception("your password is incorrect");
                    
                }
                else
                    throw new Exception("there is no such user. please register.");
            }

            public void Logout(string Email)
            {
                if (!CurrentUser.GetEmail().Equals(Email))
                    throw new Exception("only the active user can logout");
                else
                    CurrentUser = null;
            }

            private bool CheckProperPassToRegister(string password)
            {
                if (password.Length > 20 | password.Length < 4)
                    throw new Exception("This password is not between 4-20 characters");
                bool isExistSmallChar = false;
                bool isExistCapitalLetter = false;
                bool isExistNumber = false;
                for (int index = 0; index < password.Length & (!isExistCapitalLetter | !isExistSmallChar | !isExistNumber); index++)
                {
                    for (char i = 'a'; i < 'z' & !isExistSmallChar; i++)
                        if (password[index] == i)
                            isExistSmallChar = true;

                    for (char i = 'A'; i < 'Z' & !isExistCapitalLetter; i++)
                        if (password[index] == i)
                            isExistCapitalLetter = true;

                    for (char i = '0'; i < '9' & !isExistNumber; i++)
                        if (password[index] == i)
                            isExistNumber = true;

                }

                if (isExistNumber == false | isExistCapitalLetter == false | isExistSmallChar == false)
                    throw new Exception("The password does not contains at least one small character, one Capital letter and one digit");
                return true;

            }

            public void Register(string Email, string Password, string NickName)
            {
                if (UserList.ContainsKey(Email))
                {
                    throw new Exception("this Email is already used");
                }
                if (CheckProperPassToRegister(Password))
                {
                    User MyUser = new User(Email, Password, NickName);
                    UserList.Add(Email, MyUser);
                    registeredEmails.Emails.Add(Email);
                    registeredEmails.Save();
                }
            }

            public void LoadData()
            {
                registeredEmails = registeredEmails.Import();
                foreach (string email in registeredEmails.Emails)
                {
                    DataAccessLayer.User temp = new DataAccessLayer.User(email);
                    temp = temp.Import();
                    UserList.Add(email, new User(email, temp.password, temp.nickname));
                }
            }

            public void Save()
            {
                if (CurrentUser != null)
                {
                    CurrentUser.ToDalObject().Save();
                }
                else
                {
                    throw new Exception("No user is currently logged in");
                }
            }
        }

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
                myBoard = new BoardPackage.Board();
            }

            public Boolean ValidatePassword(string password)
            {
                return this.password.Equals(password);
            }

            public string GetEmail()
            {
                return email;
            }

            public BoardPackage.Board GetBoard()
            {
                return myBoard;
            }

            public string GetNickname()
            {
                return nickname;
            }

            public void LoadData()
            {

            }

            public DataAccessLayer.User ToDalObject()
            {
                return new DataAccessLayer.User(email, password, nickname, myBoard.ToDalObject());
            }
        }
    }
}
