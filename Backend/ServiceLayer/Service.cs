using System;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// The service for using the Kanban board.
    /// It allows executing all of the required behaviors by the Kanban board.
    /// You are not allowed (and can't due to the interfance) to change the signatures
    /// Do not add public methods\members! Your client expects to use specifically these functions.
    /// You may add private, non static fields (if needed).
    /// You are expected to implement all of the methods.
    /// Good luck.
    /// </summary>
    public class Service : IService
    {
        private User activeUser;
        private BoardService boardService;
        private UserService userService;
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Simple public constructor.
        /// </summary>
        public Service()
        {
            activeUser = new User(null, null);
            boardService = new BoardService();
            userService = new UserService();
        }

        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error.</returns>
        public Response LoadData()
        {
            return userService.LoadData();
        }

        ///<summary>Remove all persistent data.</summary>
        public Response DeleteData()
        {
            Response response = userService.DeleteData();
            if (!response.ErrorOccured)
            {
                activeUser = new User(null, null);
            }
            return response;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname)
        {
            if (email == null | password == null | nickname == null)
            {
                log.Warn("The email/password/nickname used to register is null");
                return new Response("The email/password/nickname used to register is null");
            }
            return userService.Register(email, password, nickname);
        }

        /// <summary>
        /// Registers a new user and joins the user to an existing board.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <param name="emailHost">The email address of the host user which owns the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname, string emailHost)
        {
            if (email == null | password == null | nickname == null | emailHost == null)
            {
                log.Warn("The email/password/nickname used to register is null");
                return new Response("The email/password/nickname used to register is null");
            }
            return userService.Register(email, password, nickname, emailHost);
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.AssignTask(email, columnOrdinal, taskId, emailAssignee);
                return response;
            }
            else
            {
                log.Error("Wasnt able to assign task, the email you provided is not the logged in user's");
                return new Response("No user is logged in to the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        		
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.DeleteTask(email, columnOrdinal, taskId);
                return response;
            }
            else
            {
                log.Error("Wasnt able to delete task, the email you provided is not the logged in user's");
                return new Response("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            if (activeUser.Email != null)
            {
                log.Warn("You can't login, there is already an online user in the system");
                return new Response<User>("You can't login, there is already an online user in the system");
            }
            if (email == null)
            {
                log.Warn("The email used to login is null");
                return new Response<User>("The email used to login is null");
            }

            Response<User> response = userService.Login(email, password);
            if (response.ErrorOccured)
                return response;
            activeUser = response.Value;
            boardService.SetActiveBoard(activeUser.GetBoard());
            return response;
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            if (activeUser.Email != null)
            {
                if (!activeUser.Email.Equals(email.ToLower()))
                {
                    log.Warn("The user you're trying to logout isn't the one that's logged in");
                    return new Response("The user you're trying to logout isn't the one that's logged in");
                }
                Response response = userService.Logout(email.ToLower());
                if (!response.ErrorOccured)
                    activeUser = new User(null, null);
                return response;
            }
            else
            {
                log.Warn("Tried to log out of a user when no user was logged in");
                return new Response("No user is logged in");
            }
        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<Board> GetBoard(string email)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
                return boardService.GetBoard();
            else
            {
                log.Error("Wasnt able to get board, the email you provided is not the logged in user's");
                return new Response<Board>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.LimitColumnTasks(columnOrdinal, limit, email);
                return response;
            }
            else
            {
                log.Error("Wasnt able to limit column tasks, the email you provided is not the logged in user's");
                return new Response<Task>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Change the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newName">The new name.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.ChangeColumnName(email, columnOrdinal, newName);
                return response;
            }
            else
            {
                log.Error("Wasnt able to change column name, the email you provided is not the logged in user's");
                return new Response<Task>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response<Task> response = boardService.AddTask(email, title, description, dueDate);
                return response;
            }
            else
            {
                log.Warn("Not able to add a task, no user connected");
                return new Response<Task>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.UpdateTaskDueDate(columnOrdinal, taskId, dueDate, email);
                return response;
            }
            else
            {
                log.Error("Wasnt able to update the task, the email you provided is not the logged in user's");
                return new Response("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.UpdateTaskTitle(columnOrdinal, taskId, title, email);
                return response;
            }
            else
            {
                log.Error("Wasnt able to update the task, the email you provided is not the logged in user's");
                return new Response("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.UpdateTaskDescription(columnOrdinal, taskId, description, email);
                return response;
            }
            else
            {
                log.Error("Wasnt able to update the task, the email you provided is not the logged in user's");
                return new Response("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.AdvanceTask(columnOrdinal, taskId, email);
                return response;
            }
            else
            {
                log.Error("Wasnt able to advance the task, the email you provided is not the logged in user's");
                return new Response("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }


        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<Column> GetColumn(string email, string columnName)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response<Column> response = boardService.GetColumn(columnName);
                return response;
            }
            else
            {
                log.Error("Wasnt able to get the column, the email you provided is not the logged in user's");
                return new Response<Column>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
                return boardService.GetColumn(columnOrdinal);
            else
            {
                log.Error("Wasnt able to get the column, the email you provided is not the logged in user's");
                return new Response<Column>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string email, int columnOrdinal)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response response = boardService.RemoveColumn(columnOrdinal, email);
                return response;
            }
            else
            {
                log.Error("Wasnt able to remove the column, the email you provided is not logged in user's");
                return new Response("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the new Column, the response should contain a error message in case of an error</returns>
        public Response<Column> AddColumn(string email, int columnOrdinal, string Name)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                Response<Column> response = boardService.AddColumn(columnOrdinal, Name, email);
                return response;
            }
            else
            {
                log.Error("Wasnt able to add the column: No user is logged in the system, or the email doesn't match the current logged in user");
                return new Response<Column>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnRight(string email, int columnOrdinal)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                return boardService.MoveColumnRight(columnOrdinal, email);
            }
            else
            {
                log.Error("Wasn't able to move the column right: No user is logged in the system, or the email doesn't match the current logged in user");
                return new Response<Column>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }
        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnLeft(string email, int columnOrdinal)
        {
            if (activeUser.Email != null && activeUser.Email.Equals(email.ToLower()))
            {
                return boardService.MoveColumnLeft(columnOrdinal, email);
            }
            else
            {
                log.Error("Wasnt able to move the column left: No user is logged in the system, or the email doesn't match the current logged in user");
                return new Response<Column>("No user is logged in the system, or the email doesn't match the current logged in user");
            }
        }

        private void CheckToSave(Response response)
        {
            if (!response.ErrorOccured)
            {
                userService.Save();
            }
        }
    }
}

