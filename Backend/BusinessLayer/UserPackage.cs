using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    namespace UserPackage
    {

        class UserController
        {
            private Dictionary<string, User> UserList;
            private User CurrentUser;

            public UserController()
            {

            }

            public User login(string Email, string Password)
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
                    {
                        throw new Exception("your password is incorrect");
                    }
                }
                else
                {
                    throw new Exception("there is no such user. please register.");
                }
                
            }

            public void Logout(string Email)
            {
                if (CurrentUser.GetEmail() != Email)
                {
                    throw new Exception("only the active user can logout");
                }
                else
                {
                    CurrentUser = null;
                }
            }

            public void Register(string Email, string Password, string NickName)
            {
                if (UserList.ContainsKey(Email))
                {
                    throw new Exception("this Email is already used");

                }
                else
                {
                    User MyUser = new User(Email, Password, NickName);
                    UserList.Add(Email, MyUser);
                }
            }
        }

        class User
        {
            private String email;
            private string password;
            private string nickname;
            private myBoard Board;


            public User(string email, string password, string nickname)
            {
                this.email = email;
                this.password = password;
                this.nickname = nickname;
                myBoard = new BoardPackage.Board();
            }

            public Boolean ValidatePassword(string password)
            {
                return this.password == password;
            }

            public string GetEmail()
            {
                return email;
            }

            public Board GetBoard()
            {
                return Board;
            }
        }
    }
}
