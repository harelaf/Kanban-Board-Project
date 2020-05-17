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
            private RegisteredEmails registeredemails;

            public UserController()
            {
                UserList = new Dictionary<string, User>();
                CurrentUser = null;
                registeredemails = new RegisteredEmails();
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
                    for (char i = 'a'; i <= 'z' & !isExistSmallChar; i++)
                    {
                        if (password[index] == i)
                        {
                            isExistSmallChar = true;
                        }
                    }

                    for (char i = 'A'; i <= 'Z' & !isExistCapitalLetter; i++)
                    {
                        if (password[index] == i)
                        {
                            isExistCapitalLetter = true;
                        }
                    }

                    for (char i = '0'; i <= '9' & !isExistNumber; i++)
                    {
                        if (password[index] == i)
                        {
                            isExistNumber = true;
                        }
                    }
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
                if (CheckProperPassToRegister(Password) & IsLegalEmailAdress(Email))
                {
                    /* //Un-needed test for same nicknames
                    foreach (User item in UserList.Values)
                    {
                        if (item.GetNickname().Equals(NickName))
                        { 
                            throw new Exception("This nickname is already in use"); 
                        }
                    }
                    */
                    if(NickName == null)
                    {
                        throw new Exception("A null value was entered for the nickname");
                    }
                    else if(NickName == "")
                    {
                        throw new Exception("An empty nickname was entered");
                    }
                    User MyUser = new User(Email, Password, NickName);
                    UserList.Add(Email, MyUser);
                    //registeredEmails.Emails.Add(Email);
                    //registeredEmails.Save();
                    MyUser.ToDalObject().Save();
                }
                else
                {
                    throw new Exception("The password or email entered are invaild");
                }
            }

            private bool IsLegalEmailAdress(string Email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(Email);
                    if (Email.IndexOf('@') == -1)
                        throw new Exception("Ilegal email, the email must contains @");
                    int index = Email.IndexOf('@');
                    int counter = 0;

                    if (Email.Substring(index + 1).Contains("@"))
                        throw new Exception("Ilegal email, the email contains more than one @");

                    for (int i = index + 1; i < Email.Length; i++)
                    {
                        if (Email[i] != '.')
                            counter++;
                        else if (counter < 2)
                            throw new Exception("Ilegal email, every generic top level must contains 2 or more characters");
                        else
                            counter = 0;
                    }

                    if (counter < 2)
                        throw new Exception("Ilegal email, every generic top level must contains 2 or more characters");

                    return addr.Address == Email;
                }
                catch
                {
                    return false;
                }
            }

            public void LoadData()
            {
                registeredemails = registeredemails.Import();
                if(registeredemails.Emails == null)
                {
                    foreach (string email in registeredemails.Emails)
                    {
                        DataAccessLayer.User temp = new DataAccessLayer.User(email);
                        temp = temp.Import();

                        User toAdd = new User(email, temp.password, temp.nickname);

                        DataAccessLayer.Board tempBoard = temp.myBoard;
                    
                        List<BoardPackage.Column> cl = new List<BoardPackage.Column>();
                        foreach(DataAccessLayer.Column myColumn in tempBoard.columnList)
                        {
                            List<BoardPackage.Task> myTaskList = new List<BoardPackage.Task>();
                            foreach (DataAccessLayer.Task task in myColumn.TaskList)
                            {
                                myTaskList.Add(new BoardPackage.Task(task.Title, task.Description, task.DueDate, task.TaskId, task.CreationDate));
                            }
                            cl.Add(new BoardPackage.Column(myTaskList, myColumn.Limit,myColumn.Name));
                        }

                        BoardPackage.Board board = new BoardPackage.Board(cl, tempBoard.idGiver);

                        toAdd.SetBoard(board);
                        UserList.Add(email, toAdd);
                    }
                
                }
            }

            public void Save()
            {
                if (CurrentUser != null)
                    CurrentUser.ToDalObject().Save();
                else
                    throw new Exception("No user is currently logged in");
            }
        }

        class User : IPersistedObject<DataAccessLayer.User>
        {
            private String email;
            private string password;
            private string nickname;
            private BoardPackage.Board myBoard;
            private int numOfColumns;

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

            public void SetBoard(BoardPackage.Board newBoard)
            {
                myBoard = newBoard;
            }

            public DataAccessLayer.User ToDalObject()
            {
                return new DataAccessLayer.User(email, password, nickname, myBoard.ToDalObject());
            }
        }
    }
}
