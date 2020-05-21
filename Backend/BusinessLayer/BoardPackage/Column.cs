using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Column : IPersistedObject<DataAccessLayer.Column>
    {
        private List<Task> taskList;
        private int limit;
        private string columnName;
        private int columnOrdinal;
        private string Email;

        const int MAX_LENGTH_NAME = 15;

        public Column()
        {
            taskList = new List<Task>();
            limit = -1;
            columnName = "";
            columnOrdinal = 0;
            Email = "";
        }

        /// <summary>
        /// Construcator of new column which initalized by a name and a column ordinal 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="email"></param>
        public Column(string columnName, int columnOrdinal, string email)
        {
            if (columnName.Length > MAX_LENGTH_NAME)
                throw new Exception("This column name length is over than " + MAX_LENGTH_NAME + " characters");
            this.taskList = new List<Task>();
            this.columnName = columnName;
            limit = -1;
            this.columnOrdinal = columnOrdinal;
            Email = email;
        }

        /// <summary>
        /// Construcator of new column which initalized by list of tasks,
        /// restriction limit of tasks, name and a column ordinal. 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnOrdinal"></param>
        public Column(List<Task> taskList, int limit, string columnName, int columnOrdinal, string email)
        {
            if (columnName.Length > MAX_LENGTH_NAME)
                throw new Exception("This column name length is over than " + MAX_LENGTH_NAME + " characters");
            this.taskList = taskList;
            this.limit = limit;
            this.columnName = columnName;
            this.columnOrdinal = columnOrdinal;
            this.Email = email;
        }

        /// <summary>
        /// Getter of the column ordinal of this Column
        /// </summary>
        /// <returns>This function returns the column ordinal of this column</returns>
        public int GetColumnOrdinal()
        {
            return columnOrdinal;
        }

        public string getEmail()
        {
            return Email;
        }
        /// <summary>
        /// This function change the column ordinal of this column by a new given name
        /// </summary>
        /// <param name="columnOrdinal"></param>
        public void SetColumnOrdinal(int columnOrdinal)
        {
            this.columnOrdinal = columnOrdinal;
        }

        /// <summary>
        /// Getter of the column ordinal of this Column
        /// </summary>
        /// <returns>This function returns the name of this column</returns>
        public string GetColumnName()
        {
            return columnName;
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
        public Task AddTask(string title, string description, DateTime dueDate, int taskId)
        {
            if (taskList.Count == limit)
            {
                throw new Exception("Can't add new task, column has a limit of " + limit);
            }
            if (description == null)
                description = "";
            Task toAdd = new Task(title, description, dueDate, taskId);
            taskList.Add(toAdd);
            toAdd.ToDalObject(Email, columnName).Save();
            return toAdd;
        }

        public void MoveExistingTaskHere(Task toAdd)
        {
            taskList.Add(toAdd);
        }

        /// <summary>
        /// This function is searching for a specific task to remove by his task id and removes him.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>This function returns the removed task</returns>

        public Task RemoveTask(int taskId)
        {
            Task toRemove = taskList.Find(x => x.GetTaskId() == taskId);
            taskList.Remove(toRemove);
            return toRemove;
        }

        /// <summary>
        /// This function gets the task id of a specific task which we want to update its due date
        /// and a new due date and updates it.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskDueDate(int taskId, DateTime dueDate)
        {
            Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
            toUpdate.UpdateTaskDueDate(dueDate);
            toUpdate.ToDalObject(Email, columnName).Save();
            return toUpdate;
        }

        /// <summary>
        /// This function gets the task id of a specific task which we want to update its title
        /// and a new title and updates it.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskTitle(int taskId, string title)
        {
            Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
            toUpdate.UpdateTaskTitle(title);
            toUpdate.ToDalObject(Email, columnName).Save();
            return toUpdate;
        }

        /// <summary>
        /// This function gets the task id of a specific task which we want to update its description
        /// and a new description and updates it.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskDescription(int taskId, string description)
        {
            Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
            toUpdate.UpdateTaskDescription(description);
            toUpdate.ToDalObject(Email, columnName).Save();
            return toUpdate;
        }

        /// <summary>
        /// This function changes the limit of tasks that this column can contains by a new limit  
        /// </summary>
        /// <param name="newLim"></param>
        public void SetLimit(int newLim)
        {
            if (taskList.Count > newLim & newLim >= 0)
            {
                throw new Exception("This column already has more tasks than the new limit");
            }
            else if(newLim < -1)
            {
                throw new Exception("This limit is unecceptable");
            }
            this.limit = newLim;
        }

        /// <summary>
        /// Getter to the limit of tasks that this column can contains
        /// </summary>
        /// <returns>This function returns the limit of tasks that this column can contains</returns>
        public int GetLimit()
        {
            return limit;
        }

        /// <summary>
        /// Getter to the list of tasks of this column
        /// </summary>
        /// <returns>This function returns the task list of this column</returns>
        public List<Task> GetTaskList()
        {
            return taskList;
        }

        /// <summary>
        /// This function transfers a column of Buisness layer to a column of the DAL 
        /// and saves this column in the data base
        /// </summary>
        /// <returns>This function returns a column of the DAL that represnt this column </returns
        public DataAccessLayer.Column ToDalObject(string Email, string column)
        {
            return new DataAccessLayer.Column(Email.ToLower(), columnName, columnOrdinal, limit);
        }
    }
}
