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
        
        /// <summary>
        /// constructore with service instance and initial empty user email
        /// </summary>
        public BackendController()
        {
            this.MyService = new Service();
            MyService.LoadData();
            Email = "";
        }

        /// <summary>
        /// delete all database
        /// </summary>
        public void DeleteData()
        {
            Response resp = MyService.DeleteData();
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        /// <summary>
        /// takes care of user registration for his own board or an existing one
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Nickname"></param>
        /// <param name="Password"></param>
        /// <param name="HostEmail"></param>
        public void Register(string Email, string Nickname, string Password, string HostEmail)
        {
            Response response = (HostEmail == "" ? MyService.Register(Email, Password, Nickname)
                : MyService.Register(Email, Password, Nickname, HostEmail));
            if (response.ErrorOccured) throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// assign task for a new member of the board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="emailAssignee"></param>
        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response resp = MyService.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// delete a task with the specified id frm the board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            Response resp = MyService.DeleteTask(email, columnOrdinal, taskId);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
        }

        /// <summary>
        /// takes care of user login to the board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public BoardModel Login(string email, string password)
        {
            Response<User> resp = MyService.Login(email, password);
            if (resp.ErrorOccured)
            {
                throw new Exception(resp.ErrorMessage);
            }
            this.Email = email;
            BoardModel ActiveBoard = ToBoardModel(MyService.GetBoard(email).Value, new UserModel(this, email, resp.Value.Nickname));
            return ActiveBoard;
        }

        /// <summary>
        /// convert service.board to boardModel
        /// </summary>
        /// <param name="MyBoard"></param>
        /// <returns></returns>
        private BoardModel ToBoardModel(Board MyBoard, UserModel ActiveUser)
        {
            ObservableCollection<ColumnModel> colList = new ObservableCollection<ColumnModel>();
            int i = 0;
            while (i < MyBoard.ColumnsNames.Count)
            {
                Column col = MyService.GetColumn(Email, i).Value;
                colList.Add(ToColumnModel(col));
                i++;
            }
            return new BoardModel(this, MyBoard.emailCreator, colList, ActiveUser);
        }

        /// <summary>
        /// convert service.column to columnModel
        /// </summary>
        /// <param name="MyColumn"></param>
        /// <returns></returns>
        private ColumnModel ToColumnModel(Column MyColumn)
        {
            ObservableCollection<TaskModel> TaskModelList = new ObservableCollection<TaskModel>();
            foreach(Task tsk in MyColumn.Tasks)
            {
                TaskModelList.Add(ToTaskModel(tsk, MyColumn.Name));
            }
            return new ColumnModel(this, TaskModelList, MyColumn.Name, MyColumn.Limit);
        }

        /// <summary>
        /// convert service.Task to taskModel
        /// </summary>
        /// <param name="tsk"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        private TaskModel ToTaskModel(Task tsk, string ColumnName)
        {
            return new TaskModel(this,tsk.Id, tsk.CreationTime, tsk.DueDate, tsk.Title, tsk.Description, tsk.emailAssignee, ColumnName);
        }

        /// <summary>
        /// takes care of user LogOut
        /// </summary>
        /// <param name="Email"></param>
        public void Logout(string Email)
        {
            Response response = MyService.Logout(Email);
            if (response.ErrorOccured) throw new Exception(response.ErrorMessage);
            this.Email = "";
        }

        /// <summary>
        /// returns a boardModel of this email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public BoardModel GetBoard(string Email, UserModel ActiveUser)
        {
            return ToBoardModel(MyService.GetBoard(Email).Value, ActiveUser);
        }

        /// <summary>
        /// limit the capacity of specified column
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="limit"></param>
        public void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            Response resp = MyService.LimitColumnTasks(email, columnOrdinal, limit);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// replace column name
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="newName"></param>
        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            Response resp = MyService.ChangeColumnName(email, columnOrdinal, newName);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// add a new task to the board with the given components
        /// </summary>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public TaskModel AddTask(string email,string title,string description, DateTime dueDate)
        {
            Response<Task> resp = MyService.AddTask(email, title, description,dueDate);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            TaskModel task = ToTaskModel(resp.Value, MyService.GetColumn(email,0).Value.Name);
            return ToTaskModel(resp.Value, MyService.GetColumn(email, 0).Value.Name);
        }

        /// <summary>
        /// updates task's dueDate 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response resp = MyService.UpdateTaskDueDate(email, columnOrdinal,taskId, dueDate);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// updates task's title
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            Response resp = MyService.UpdateTaskTitle(email, columnOrdinal, taskId, title);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// updates task's description
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            Response resp = MyService.UpdateTaskDescription(email, columnOrdinal, taskId, description);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// advance the task with specified id to the next column if possible
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Response resp = MyService.AdvanceTask(email, columnOrdinal, taskId);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// retrns the columnModel with the specified name
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ColumnModel GetColumn(string email, string columnName)
        {
            Response<Column> resp = MyService.GetColumn(email, columnName);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return ToColumnModel(resp.Value);
        }

        /// <summary>
        /// retrns the columnModel on the specified index
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        public ColumnModel GetColumn(string email, int columnOrdinal)
        {
            Response<Column> resp = MyService.GetColumn(email, columnOrdinal);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            return ToColumnModel(resp.Value);
        }

        /// <summary>
        /// removes the column on the specified index
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        public void RemoveColumn(string email, int columnOrdinal)
        {
            Response resp = MyService.RemoveColumn(email, columnOrdinal);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
        }

        /// <summary>
        /// add a new column on the requsted index
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal">index to insert in</param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public ColumnModel AddColumn(string email, int columnOrdinal,string Name)
        {
            Response<Column> resp = MyService.AddColumn(email, columnOrdinal,Name);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            ObservableCollection<TaskModel> list = new ObservableCollection<TaskModel>();
            foreach(Task task in resp.Value.Tasks)
            {
                TaskModel ToAdd = ToTaskModel(task, resp.Value.Name);
                list.Add(ToAdd);
            }
            ColumnModel column = new ColumnModel(this, list, Name, resp.Value.Limit);
            return column;
        }

        /// <summary>
        /// replace the column position with the one on its right if possible
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        public ColumnModel MoveColumnRight(string email, int columnOrdinal)
        {
            Response<Column> resp = MyService.MoveColumnRight(email, columnOrdinal);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            ObservableCollection<TaskModel> list = new ObservableCollection<TaskModel>();
            foreach (Task task in resp.Value.Tasks)
            {
                TaskModel ToAdd = ToTaskModel(task, resp.Value.Name);
                list.Add(ToAdd);
            }
            ColumnModel column=new ColumnModel(this, list,resp.Value.Name, resp.Value.Limit);
            return column;
        }

        /// <summary>
        /// replace the column's position with the one on its right if possible
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal">the column index</param>
        /// <returns></returns>
        public ColumnModel MoveColumnLeft(string email, int columnOrdinal)
        {
            Response<Column> resp = MyService.MoveColumnLeft(email, columnOrdinal);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            ObservableCollection<TaskModel> list = new ObservableCollection<TaskModel>();
            foreach (Task task in resp.Value.Tasks)
            {
                TaskModel ToAdd = ToTaskModel(task, resp.Value.Name);
                list.Add(ToAdd);
            }
            ColumnModel column = new ColumnModel(this, list, resp.Value.Name, resp.Value.Limit);
            return column;
        }
    }
}
