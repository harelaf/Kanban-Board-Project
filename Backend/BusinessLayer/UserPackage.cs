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
            private DalController DalController = new DalController();

            const int MIN_LENGTH_OF_PASSWORD = 5;
            const int MAX_LENGTH_OF_PASSWORD = 25;
            const int MIN_UCASE_LETTER = 1;
            const int MIN_LCASE_LETTER = 1;
            const int MIN_DIGITS = 1;

            public UserController()
            {
                UserList = new Dictionary<string, User>();
                CurrentUser = null;
                registeredemails = new RegisteredEmails();
            }

            /// <summary>
            /// This function gets the email of the user and the password
            /// and login to the system if the details are correct 
            /// </summary>
            /// <param name="Email"></param>
            /// <param name="Password"></param>
            /// <returns>returns the user that logged in</returns>
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

            /// <summary>
            /// This function logges out the active user from the system
            /// by giving the email of the active user 
            /// </summary>
            /// <param name="Email"></param>
            public void Logout(string Email)
            {
                if (!CurrentUser.GetEmail().Equals(Email))
                    throw new Exception("only the active user can logout");
                else
                    CurrentUser = null;
            }

            /// <summary>
            /// This function gets a password and checks if this password is legal to register with
            /// according to the next demands : a user password must be in length of 5 to 25 characters and must include at
            ///least one uppercase letter, one small character and a number.
            /// </summary>
            /// <param name="password"></param>
            /// <returns>returns true if this password is proper to register or false if not</returns>
            private bool CheckProperPassToRegister(string password)
            {
                if (password.Length > MAX_LENGTH_OF_PASSWORD | password.Length < MIN_LENGTH_OF_PASSWORD)
                    throw new Exception("This password is not between" + MIN_LENGTH_OF_PASSWORD + "-" + MAX_LENGTH_OF_PASSWORD + "characters");
                int AmountOfUpperCase = MIN_UCASE_LETTER;
                int AmountOfLowerCase = MIN_LCASE_LETTER;
                int AmountOfDigits = MIN_DIGITS;

                for (int index = 0; index < password.Length & (AmountOfDigits != 0 | AmountOfLowerCase != 0 | AmountOfUpperCase != 0); index++)
                {
                    for (char i = 'a'; i <= 'z' & AmountOfLowerCase != 0; i++)
                        if (password[index] == i)
                            AmountOfLowerCase--;

                    for (char i = 'A'; i <= 'Z' & AmountOfUpperCase != 0; i++)
                        if (password[index] == i)
                            AmountOfUpperCase--;

                    for (char i = '0'; i <= '9' & AmountOfDigits != 0; i++)
                        if (password[index] == i)
                            AmountOfDigits--;
                }

                if (AmountOfDigits != 0 | AmountOfUpperCase != 0 | AmountOfLowerCase != 0)
                    throw new Exception("The password does not contains at least one small character, one Capital letter and one digit");
                return true;

            }

            /// <summary>
            /// This function gets an email,password and nickname and registers to the system
            /// if the email is not already taken, the password is proper the email is legal 
            /// and the nickname is not null
            /// </summary>
            /// <param name="Email"></param>
            /// <param name="Password"></param>
            /// <param name="NickName"></param>
            public void Register(string Email, string Password, string NickName)
            {
                if (UserList.ContainsKey(Email))
                {
                    throw new Exception("this Email is already used");
                }
                if (CheckProperPassToRegister(Password) & IsLegalEmailAdress(Email))
                {
                    if (NickName == null)
                    {
                        throw new Exception("A null value was entered for the nickname");
                    }
                    else if (NickName == "")
                    {
                        throw new Exception("An empty nickname was entered");
                    }
                    User MyUser = new User(Email, Password, NickName);
                    UserList.Add(Email, MyUser);
                    //registeredEmails.Emails.Add(Email);
                    //registeredEmails.Save();
                    MyUser.ToDalObject(Email, "").Save();
                }
                else
                {
                    throw new Exception("The password or email entered are invaild");
                }
            }

            /// <summary>
            /// This function gets an email adress and checks if this email adress is legal
            /// </summary>
            /// <param name="Email"></param>
            /// <returns>returns true if the email adress is legal and false if not</returns>
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
            /// <summary>
            /// This function delete all the data of the users from the data base
            /// </summary>
            public void DeleteData()
            {
                DalController.Delete();
            }

            /// <summary>
            /// This function loads the data of the all users from the data base 
            /// </summary>
            public void LoadData()
            {
                registeredemails = registeredemails.Import();
                if (registeredemails.Emails != null)
                {
                    foreach (string email in registeredemails.Emails)
                    {
                        DataAccessLayer.User temp = new DataAccessLayer.User(email);
                        temp = temp.Import();
                        
                        User toAdd = new User(null, temp.password, temp.nickname);
                        toAdd.SetEmail(email);
                        List<DataAccessLayer.Column> columnListDAL = temp.GetColumns();
                        //DataAccessLayer.Board tempBoard = temp.myBoard;

                        List<BoardPackage.Column> cl = new List<BoardPackage.Column>();
                        foreach (DataAccessLayer.Column myColumn in columnListDAL)
                        {
                            List<DataAccessLayer.Task> TaskListDAL = myColumn.getTasks();
                            List<BoardPackage.Task> myTaskList = new List<BoardPackage.Task>();
                            foreach (DataAccessLayer.Task task in TaskListDAL)
                            {
                                myTaskList.Add(new BoardPackage.Task(task.Title, task.Description, task.DueDate, task.TaskId, task.CreationDate));
                            }
                            cl.Add(new BoardPackage.Column(myTaskList, (int)myColumn.Limit, myColumn.Name, (int)myColumn.ordinal, email));
                        }

                        Board board = new Board(cl, (int)temp.idGiver);

                        toAdd.SetBoard(board);
                        UserList.Add(email, toAdd);
                    }

                }
            }

            /// <summary>
            /// This function saves the changes that accured to the data base
            /// </summary>
            public void Save()
            {
                if (CurrentUser != null)
                    CurrentUser.ToDalObject(CurrentUser.GetEmail(), "").Save();
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

            public User(string email, string password, string nickname)
            {
                this.email = email;
                this.password = password;
                this.nickname = nickname;
                if(email != null)
                {
                    myBoard = new Board(email);
                }
                else
                {
                    myBoard = new Board();
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
                return new DataAccessLayer.User(email, password, nickname, myBoard.getIdGiver(), myBoard.GetNumOfColumns());
            }
        }
    }
}
