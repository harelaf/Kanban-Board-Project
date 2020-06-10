using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;

namespace Presentation
{
    class BackendController
    {
        private Service MyService;
        public string Email { get; private set; }
        
        public BackendController()
        {
            this.MyService = new Service();
            MyService.LoadData();
            Email = "";
        }

        public void DeleteData()
        {
            Response resp = MyService.DeleteData();
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        public void Register(string Email, string Nickname, string Password, string HostEmail)
        {
            Response response = (HostEmail == "" ? MyService.Register(Email, Password, Nickname)
                : MyService.Register(Email, Password, Nickname, HostEmail));
            if (response.ErrorOccured) throw new Exception(response.ErrorMessage);
        }

        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response resp = MyService.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            Response resp = MyService.DeleteTask(email, columnOrdinal, taskId);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        public UserModel Login(string email, string password)
        {
            Response<User> resp = MyService.Login(email, password);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
            UserModel ActiveUser = new UserModel(resp.Value.Email, resp.Value.Nickname
                , ToBoardModel(MyService.GetBoard(resp.Value.Email).Value));
            Email = email;
            return ActiveUser;
        }

        private BoardModel ToBoardModel(Board MyBoard)
        {
            ObservableCollection<ColumnModel> colList = new ObservableCollection<ColumnModel>();
            int i = 0;
            while (i < MyBoard.ColumnsNames.Count)
            {
                Column col = MyService.GetColumn(MyBoard.emailCreator, i).Value;
                colList.Add(ToColumnModel(col));
                i++;
            }
            return new BoardModel(MyBoard.emailCreator, colList);
        }

        private ColumnModel ToColumnModel(Column MyColumn)
        {
            ObservableCollection<TaskModel> TaskModelList = new ObservableCollection<TaskModel>();
            foreach(Task tsk in MyColumn.Tasks)
            {
                TaskModelList.Add(ToTaskModel(tsk));
            }
            return new ColumnModel(TaskModelList, MyColumn.Name);
        }

        private TaskModel ToTaskModel(Task tsk)
        {
            return new TaskModel(tsk.Id, tsk.CreationTime, tsk.DueDate, tsk.Title, tsk.Description, tsk.emailAssignee);
        }


        //Response Logout(string email);

        public BoardModel GetBoard(string Email)
        {
            return ToBoardModel(MyService.GetBoard(Email).Value);
        }

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
