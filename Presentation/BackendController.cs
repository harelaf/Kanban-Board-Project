using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;

namespace Presentation
{
    class BackendController
    {
        private Service myService;

        public BackendController()
        {
            this.myService = new Service();
            myService.LoadData();
        }

        //Response LoadData();

        public void DeleteData()
        {
            Response resp = myService.DeleteData();
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        //Response DeleteData();

        public void Register(string Email, string Nickname, string Password, string HostEmail)
        {
            Response response = (HostEmail == "" ? MyService.Register(Email, Password, Nickname) : MyService.Register(Email, Password, Nickname, HostEmail));
            if (response.ErrorOccured) throw new Exception(response.ErrorMessage);
        }


        //Response Register(string email, string password, string nickname);

        //Response Register(string email, string password, string nickname, string emailHost);

        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response resp = myService.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        //Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee);

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            Response resp = myService.DeleteTask(email, columnOrdinal, taskId);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        //Response DeleteTask(string email, int columnOrdinal, int taskId);

        public UserModel Login(string email, string password)
        {
            Response<User> resp = myService.Login(email, password);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
            UserModel ActiveUser = new UserModel(resp.Value.)
        }

        //Response<User> Login(string email, string password);

        //Response Logout(string email);

        //Response<Board> GetBoard(string email);

        //Response LimitColumnTasks(string email, int columnOrdinal, int limit);

        //Response ChangeColumnName(string email, int columnOrdinal, string newName);

        //Response<Task> AddTask(string email, string title, string description, DateTime dueDate);

        //Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate);

        //Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title);

        //Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description);

        //Response AdvanceTask(string email, int columnOrdinal, int taskId);

        //Response<Column> GetColumn(string email, string columnName);

        //Response<Column> GetColumn(string email, int columnOrdinal);

        //Response RemoveColumn(string email, int columnOrdinal);

        //Response<Column> AddColumn(string email, int columnOrdinal, string Name);

        //Response<Column> MoveColumnRight(string email, int columnOrdinal);

        //Response<Column> MoveColumnLeft(string email, int columnOrdinal);

    }
    }
