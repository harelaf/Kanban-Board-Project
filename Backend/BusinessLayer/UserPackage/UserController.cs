using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class UserController
    {
        private Dictionary<string, User> UserList;
        private User CurrentUser;
        private DalController DalController = new DalController();

        const int MIN_LENGTH_OF_Password = 5;
        const int MAX_LENGTH_OF_Password = 25;
        const int MIN_UCASE_LETTER = 1;
        const int MIN_LCASE_LETTER = 1;
        const int MIN_DIGITS = 1;

        public UserController()
        {
            UserList = new Dictionary<string, User>();
            CurrentUser = null;
            DalController.CreateDataBase();
        }

        /// <summary>
        /// This function gets the email of the user and the Password
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
                    throw new Exception("your Password is incorrect");

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
        /// This function gets a Password and checks if this Password is legal to register with
        /// according to the next demands : a user Password must be in length of 5 to 25 characters and must include at
        ///least one uppercase letter, one small character and a number.
        /// </summary>
        /// <param name="Password"></param>
        /// <returns>returns true if this Password is proper to register or false if not</returns>
        private bool CheckProperPassToRegister(string Password)
        {
            if (Password.Length > MAX_LENGTH_OF_Password | Password.Length < MIN_LENGTH_OF_Password)
                throw new Exception("This password is not between " + MIN_LENGTH_OF_Password + "-" + MAX_LENGTH_OF_Password + " characters");
            int AmountOfUpperCase = MIN_UCASE_LETTER;
            int AmountOfLowerCase = MIN_LCASE_LETTER;
            int AmountOfDigits = MIN_DIGITS;

            for (int Index = 0; Index < Password.Length & (AmountOfDigits > 0 | AmountOfLowerCase > 0 | AmountOfUpperCase > 0); Index++)
            {

                if (Password[Index] >= 'a' & Password[Index] <= 'z')
                    AmountOfLowerCase--;

                else if (Password[Index] >= 'A' & Password[Index] <= 'Z')
                    AmountOfUpperCase--;

                else if (Password[Index] >= '0' & Password[Index] <= '9')
                    AmountOfDigits--;

            }

            if (AmountOfDigits > 0 | AmountOfUpperCase > 0 | AmountOfLowerCase > 0)
                throw new Exception("The password does not contains at least one small character, one Capital letter and one digit");
            return true;

        }

        /// <summary>
        /// This function gets an email,Password and Nickname and registers to the system
        /// if the email is not already taken, the Password is proper the email is legal 
        /// and the Nickname is not null
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <param name="Nickname"></param>
        public void Register(string Email, string Password, string Nickname)
        {
            if (UserList.ContainsKey(Email))
            {
                throw new Exception("this Email is already used");
            }
            if (CheckProperPassToRegister(Password) & IsLegalEmailAdress(Email))
            {
                if (Nickname == null)
                {
                    throw new Exception("A null value was entered for the Nickname");
                }
                else if (Nickname == "")
                {
                    throw new Exception("An empty Nickname was entered");
                }
                User MyUser = new User(Email, Password, Nickname);
                UserList.Add(Email, MyUser);
                MyUser.GetBoard().ToDalObject(MyUser.GetEmail(), "").Save();
                MyUser.ToDalObject(Email, "").Save();
            }
            else
            {
                throw new Exception("The Password or email entered are invaild");
            }
        }

        public void Register(string Email, string Password, string Nickname, string EmailHost)
        {
            if (!UserList.ContainsKey(EmailHost))
                throw new Exception("can't register to this board because the email host doesn't exist in the system");
            if (CheckProperPassToRegister(Password) & IsLegalEmailAdress(Email))
            {
                if (Nickname == null)
                {
                    throw new Exception("A null value was entered for the Nickname");
                }
                else if (Nickname == "")
                {
                    throw new Exception("An empty Nickname was entered");
                }
                User UserHost=null;
           
                UserHost=UserList[EmailHost];
                User MyUser = new User(Email, Password, Nickname, UserHost.GetBoard());
                UserList.Add(Email, MyUser);
                MyUser.ToDalObject(Email, "").Save();
            }
            else
            {
                throw new Exception("The Password or email entered are invaild");
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
                var Addr = new System.Net.Mail.MailAddress(Email);
                if (Email.IndexOf('@') == -1)
                    throw new Exception("Illegal email, the email must contains @");
                int Index = Email.IndexOf('@');
                int Counter = 0;

                if (Email.Substring(Index + 1).Contains("@"))
                    throw new Exception("Illegal email, the email contains more than one @");

                if (!Email.Substring(Index + 1).Contains("."))
                    throw new Exception("Illegal email, the email doesnt contain a '.'");

                for (int i = Index + 1; i < Email.Length; i++)
                {
                    if (Email[i] != '.')
                        Counter++;
                    else if (Counter < 2)
                        throw new Exception("Illegal email, every generic top level must contains 2 or more characters");
                    else
                        Counter = 0;
                }

                if (Counter < 2)
                    throw new Exception("Illegal email, every generic top level must contains 2 or more characters");

                return Addr.Address == Email;
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
            DalController.CreateDataBase();
            List<string> EmailList = DalController.ImportRegisteredEmails();
            if (EmailList != null)
            {
                foreach (string CurrEmail in EmailList)
                {
                    DataAccessLayer.User Temp = new DataAccessLayer.User(CurrEmail);
                    Temp = Temp.Import();

                    User toAdd = new User(null, Temp.Password, Temp.Nickname);
                    toAdd.SetEmail(CurrEmail);
                    
                    DataAccessLayer.Board TempBoard = Temp.GetBoard();
                    List<DataAccessLayer.Column> ColumnListDAL = TempBoard.GetColumns();

                    List<BoardPackage.Column> MyColumnList = new List<BoardPackage.Column>();
                    foreach (DataAccessLayer.Column DalColumn in ColumnListDAL)
                    {
                        List<DataAccessLayer.Task> TaskListDAL = DalColumn.getTasks();
                        List<BoardPackage.Task> myTaskList = new List<BoardPackage.Task>();
                        foreach (DataAccessLayer.Task DalTask in TaskListDAL)
                        {
                            myTaskList.Add(new BoardPackage.Task(DalTask.Title, DalTask.Description, DalTask.DueDate, DalTask.TaskId, DalTask.CreationDate, DalTask.Assignee));
                        }
                        MyColumnList.Add(new BoardPackage.Column(myTaskList, (int)DalColumn.Limit, DalColumn.Name, (int)DalColumn.Ordinal, CurrEmail.ToLower()));
                    }

                    BoardPackage.Board board = new BoardPackage.Board(MyColumnList, (int)TempBoard.IdGiver, Temp.HostEmail, TempBoard.GetMembers());

                    toAdd.SetBoard(board);
                    UserList.Add(CurrEmail, toAdd);
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
}
