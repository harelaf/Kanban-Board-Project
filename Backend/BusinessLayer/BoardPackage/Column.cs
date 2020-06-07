using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Column : IPersistedObject<DataAccessLayer.Column>
    {
        private List<Task> TaskList;
        private int Limit;
        private string ColumnName;
        private int ColumnOrdinal;
        private string Email;

        const int MAX_LENGTH_NAME = 15;
        const int MAX_NUM_OF_TASKS = 100;
        public Column()
        {
            TaskList = new List<Task>();
            Limit = MAX_NUM_OF_TASKS;
            ColumnName = "";
            ColumnOrdinal = 0;
            Email = "";
        }

        /// <summary>
        /// Construcator of new column which initalized by a name and a column ordinal 
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnOrdinal"></param>
        /// <param name="Email"></param>
        public Column(string ColumnName, int ColumnOrdinal, string Email)
        {
            if (ColumnName.Length > MAX_LENGTH_NAME)
                throw new Exception("This column name length is over than " + MAX_LENGTH_NAME + " characters");
            this.TaskList = new List<Task>();
            this.ColumnName = ColumnName;
            Limit = MAX_NUM_OF_TASKS;
            this.ColumnOrdinal = ColumnOrdinal;
            this.Email = Email;
        }

        /// <summary>
        /// Construcator of new column which initalized by list of tasks,
        /// restriction Limit of tasks, name and a column ordinal. 
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnOrdinal"></param>
        public Column(List<Task> TaskList, int Limit, string ColumnName, int ColumnOrdinal, string Email)
        {
            if (ColumnName.Length > MAX_LENGTH_NAME)
                throw new Exception("This column name length is over than " + MAX_LENGTH_NAME + " characters");
            this.TaskList = TaskList;
            this.Limit = Limit;
            this.ColumnName = ColumnName;
            this.ColumnOrdinal = ColumnOrdinal;
            this.Email = Email;
        }

        /// <summary>
        /// Getter of the column ordinal of this Column
        /// </summary>
        /// <returns>This function returns the column ordinal of this column</returns>
        public int GetColumnOrdinal()
        {
            return ColumnOrdinal;
        }

        public string getEmail()
        {
            return Email;
        }

        public void SetColumnName(string ColumnName)
        {
            this.ToDalObject(Email, "").Delete();
            this.ColumnName = ColumnName;
            Column toUpdate = new Column(this.TaskList, this.Limit, this.ColumnName, this.ColumnOrdinal, this.Email);
            toUpdate.ToDalObject(Email, "").Save();
        }

        /// <summary>
        /// This function change the column ordinal of this column by a new given name
        /// </summary>
        /// <param name="ColumnOrdinal"></param>
        public void SetColumnOrdinal(int ColumnOrdinal)
        {
            this.ColumnOrdinal = ColumnOrdinal;
        }

        /// <summary>
        /// Getter of the column ordinal of this Column
        /// </summary>
        /// <returns>This function returns the name of this column</returns>
        public string GetColumnName()
        {
            return ColumnName;
        }

        /// <summary>
        /// This function gets details of new task(title,description, due date,task id)
        /// to add, creates the new task and adds this task to the column tasks.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="taskId"></param>
        /// <returns>This function returns the added task</returns>
        public Task AddTask(string title, string description, DateTime dueDate, int taskId,string EmailAssignee)
        {
            if (TaskList.Count == Limit)
            {
                throw new Exception("Can't add new task, column has a Limit of " + Limit);
            }
            if (description == null)
                description = "";
            Task toAdd = new Task(title, description, dueDate, taskId, EmailAssignee);
            TaskList.Add(toAdd);
            toAdd.ToDalObject(Email, ColumnName).Save();
            return toAdd;
        }

        public void AssignTask(string email, int taskid, string emailAssignee)
        {
            Task toChange = TaskList.Find(x => x.GetTaskId().Equals(taskid));
            toChange.SetEmailAssignee(emailAssignee);
            toChange.ToDalObject(email, ColumnName).Save();
        }

        public void MoveExistingTaskHere(Task toAdd)
        {
            TaskList.Add(toAdd);
        }

        /// <summary>
        /// This function is searching for a specific task to remove by his task id and removes him.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>This function returns the removed task</returns>

        public Task RemoveTask(int TaskId, string Email)
        {
            Task toRemove = TaskList.Find(x => x.GetTaskId() == TaskId);
            if (!Email.Equals(toRemove.GetEmailAssignee()))
                throw new Exception("a user cannot delete a task he is not assigned to");
            TaskList.Remove(toRemove);
            return toRemove;
        }

        /// <summary>
        /// This function gets the task id of a specific task which we want to update its due date
        /// and a new due date and updates it.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskDueDate(string Email, int TaskId, DateTime DueDate)
        {
            Task toUpdate = TaskList.Find(x => x.GetTaskId() == TaskId);
            if (Email.Equals(TaskList[TaskId].GetEmailAssignee()))
            {
                toUpdate.UpdateTaskDueDate(DueDate);
                toUpdate.ToDalObject(Email, ColumnName).Save();
            }
            else
                throw new Exception($"The user with this Email:{Email} can't update this task due date because he is not the assignee");
            return toUpdate;

        }
        /// <summary>
        /// This function gets the task id of a specific task which we want to update its title
        /// and a new title and updates it.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskTitle(string Email,int TaskId, string Title)
        {
            Task toUpdate = TaskList.Find(x => x.GetTaskId() == TaskId);
            if (Email.Equals(TaskList[TaskId].GetEmailAssignee()))
            {
                toUpdate.UpdateTaskTitle(Title);
                toUpdate.ToDalObject(Email, ColumnName).Save();
            }
            else
                throw new Exception($"The user with this Email:{Email} can't update this task title because he is not the assignee");
            return toUpdate;
        }

        /// <summary>
        /// This function gets the task id of a specific task which we want to update its description
        /// and a new description and updates it.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskDescription(string Email,int TaskId, string Description)
        {
            Task toUpdate = TaskList.Find(x => x.GetTaskId() == TaskId);
            if (Email.Equals(TaskList[TaskId].GetEmailAssignee()))
            {
                toUpdate.UpdateTaskDescription(Description);
                toUpdate.ToDalObject(Email, ColumnName).Save();
            }
            else
                throw new Exception($"The user with this Email:{Email} can't update this task description because he is not the assignee");
            return toUpdate;
        }

        /// <summary>
        /// This function changes the Limit of tasks that this column can contains by a new Limit  
        /// </summary>
        /// <param name="newLim"></param>
        public void SetLimit(int newLim)
        {
            if (newLim == -1)
                Limit = -1;
            if (TaskList.Count > newLim & newLim >= 0)
                throw new Exception("This column already has more tasks than the new Limit");
            else if (newLim < -1)
                throw new Exception("This Limit is unecceptable");
            this.Limit = newLim;
        }

        /// <summary>
        /// Getter to the Limit of tasks that this column can contains
        /// </summary>
        /// <returns>This function returns the Limit of tasks that this column can contains</returns>
        public int GetLimit()
        {
            return Limit;
        }

        /// <summary>
        /// Getter to the list of tasks of this column
        /// </summary>
        /// <returns>This function returns the task list of this column</returns>
        public List<Task> GetTaskList()
        {
            return TaskList;
        }

        /// <summary>
        /// This function transfers a column of Buisness layer to a column of the DAL 
        /// and saves this column in the data base
        /// </summary>
        /// <returns>This function returns a column of the DAL that represnt this column </returns
        public DataAccessLayer.Column ToDalObject(string Email, string column)
        {
            return new DataAccessLayer.Column(Email.ToLower(), ColumnName, ColumnOrdinal, Limit);
        }
    }
}
