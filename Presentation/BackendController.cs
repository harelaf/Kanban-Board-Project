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
            UserModel ActiveUser = new UserModel(this, resp.Value.Email, resp.Value.Nickname
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
            return new BoardModel(this, MyBoard.emailCreator, colList);
        }

        private ColumnModel ToColumnModel(Column MyColumn)
        {
            ObservableCollection<TaskModel> TaskModelList = new ObservableCollection<TaskModel>();
            foreach(Task tsk in MyColumn.Tasks)
            {
                TaskModelList.Add(ToTaskModel(tsk, MyColumn.Name));
            }
            return new ColumnModel(this, TaskModelList, MyColumn.Name);
        }

        private TaskModel ToTaskModel(Task tsk, string ColumnName)
        {

            return new TaskModel(tsk.Id, tsk.CreationTime, tsk.DueDate, tsk.Title, tsk.Description, tsk.emailAssignee, ColumnName);

        }

        public void Logout(string Email)
        {
            Response response = MyService.Logout(Email);
            if (response.ErrorOccured) throw new Exception(response.ErrorMessage);
            this.Email = "";
        }

        public BoardModel GetBoard(string Email)
        {
            return ToBoardModel(MyService.GetBoard(Email).Value);
        }

        public void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            Response resp = MyService.LimitColumnTasks(email, columnOrdinal, limit);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            Response resp = MyService.ChangeColumnName(email, columnOrdinal, newName);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        public TaskModel AddTask(string email,string title,string description, DateTime dueDate)
        {
            Response<Task> resp = MyService.AddTask(email, title, description,dueDate);

            TaskModel task = ToTaskModel(resp.Value, MyService.GetColumn(email,0).Value.Name);

            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return ToTaskModel(resp.Value, MyService.GetColumn(email, 0).Value.Name);
        }

        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response resp = MyService.UpdateTaskDueDate(email, columnOrdinal,taskId, dueDate);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            Response resp = MyService.UpdateTaskTitle(email, columnOrdinal, taskId, title);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            Response resp = MyService.UpdateTaskDescription(email, columnOrdinal, taskId, description);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Response resp = MyService.AdvanceTask(email, columnOrdinal, taskId);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        public ColumnModel GetColumn(string email, string columnName)
        {
            Response<Column> resp = MyService.GetColumn(email, columnName);
            
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return ToColumnModel(resp.Value);
        }

        public ColumnModel GetColumn(string email, int columnOrdinal)
        {
            Response<Column> resp = MyService.GetColumn(email, columnOrdinal);

            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return ToColumnModel(resp.Value);
        }

        public void RemoveColumn(string email, int columnOrdinal)
        {
            Response resp = MyService.RemoveColumn(email, columnOrdinal);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        public ColumnModel AddColumn(string email, int columnOrdinal,string Name)
        {
            Response<Column> resp = MyService.AddColumn(email, columnOrdinal,Name);
            ObservableCollection<TaskModel> list = new ObservableCollection<TaskModel>();
            foreach(Task task in resp.Value.Tasks)
            {

                TaskModel ToAdd = ToTaskModel(task, resp.Value.Name);
                list.Add(ToAdd);
            }
            ColumnModel column = new ColumnModel(this, list,Name);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return column;
        }

        public ColumnModel MoveColumnRight(string email, int columnOrdinal)
        {
            Response<Column> resp = MyService.MoveColumnRight(email, columnOrdinal);
            ObservableCollection<TaskModel> list = new ObservableCollection<TaskModel>();
            foreach (Task task in resp.Value.Tasks)
            {
                TaskModel ToAdd = ToTaskModel(task, resp.Value.Name);
                list.Add(ToAdd);
            }
            ColumnModel column=new ColumnModel(this, list,resp.Value.Name);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return column;
        }

        public ColumnModel MoveColumnLeft(string email, int columnOrdinal)
        {
            Response<Column> resp = MyService.MoveColumnLeft(email, columnOrdinal);
            ObservableCollection<TaskModel> list = new ObservableCollection<TaskModel>();
            foreach (Task task in resp.Value.Tasks)
            {
                TaskModel ToAdd = ToTaskModel(task, resp.Value.Name);
                list.Add(ToAdd);
            }
            ColumnModel column = new ColumnModel(this, list, resp.Value.Name);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return column;
        }
    }
}
