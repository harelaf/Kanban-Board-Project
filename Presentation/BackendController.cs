using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;

namespace Presentation
{
    class BackendController
    {
        private Service MyService;
        
        public BackendController()
        {
            this.MyService = new Service();
            MyService.LoadData();
        }

        //Response LoadData();

        public void DeleteData()
        {
            Response resp = MyService.DeleteData();
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        //Response DeleteData();

        public void Register(string Email, string Nickname, string Password, string HostEmail)
        {
            Response response = (HostEmail == "" ? MyService.Register(Email, Password, Nickname)
                : MyService.Register(Email, Password, Nickname, HostEmail));
            if (response.ErrorOccured) throw new Exception(response.ErrorMessage);
        }


        //Response Register(string email, string password, string nickname);

        //Response Register(string email, string password, string nickname, string emailHost);

        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response resp = MyService.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        //Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee);

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            Response resp = MyService.DeleteTask(email, columnOrdinal, taskId);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        //Response DeleteTask(string email, int columnOrdinal, int taskId);

        public UserModel Login(string email, string password)
        {
            Response<User> resp = MyService.Login(email, password);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
            UserModel ActiveUser = new UserModel(resp.Value.Email, resp.Value.Nickname
                , toBoardModel(MyService.GetBoard(resp.Value.Email).Value));
            return ActiveUser;
        }

        private BoardModel toBoardModel(Board MyBoard)
        {
            List<ColumnModel> colList = new List<ColumnModel>();
            int i = 0;
            while (i < MyBoard.ColumnsNames.Count)
            {
                Column col = MyService.GetColumn(MyBoard.emailCreator, i).Value;
                colList.Add(toColumnModel(col));
                i++;
            }
            return new BoardModel(MyBoard.emailCreator, colList);
        }

        private ColumnModel toColumnModel(Column MyColumn)
        {
            List<TaskModel> TaskModelList = new List<TaskModel>();
            foreach(Task tsk in MyColumn.Tasks)
            {
                TaskModelList.Add(toTaskModel(tsk));
            }
            return new ColumnModel(TaskModelList);
        }

        private TaskModel toTaskModel(Task tsk)
        {
            return new TaskModel(tsk.Id, tsk.CreationTime, tsk.DueDate, tsk.Title, tsk.Description, tsk.emailAssignee);
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
