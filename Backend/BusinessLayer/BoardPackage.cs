using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    namespace BoardPackage
    {
        class Board// : IPersistedObject<DataAccessLayer.Board>
        {

            private List<Column> list;
            private int idGiver;

            /// <summary>
            /// an empty constractor of new board 
            /// </summary>
            public Board()
            {
                for (int i = 0; i < 3; i++)
                    list[i] = new Column();
                idGiver = 0;
            }

            /// <summary>
            /// Constractor of new board which gets list of columns and idGiver and initializing the fields
            /// </summary>
            /// <param name="list"></param>
            /// <param name="idGiver"></param>
            public Board(List<Column> list, int idGiver)
            {
                this.list = list;
                this.idGiver = idGiver;
            }

            /// <summary>
            /// This function advance task from one column to the next column
            /// </summary>
            /// <param name="ColumnOrdinal"></param>
            /// <param name="taskId"></param>
            public void AdvanceTask(int ColumnOrdinal, int taskId)
            {
                if (ColumnOrdinal == list.Count - 1)//cannot advance further than 'done'.
                    throw new Exception("Can't advance mission that is already done");

                if (ColumnOrdinal > list.Count - 1 | ColumnOrdinal < 0)
                    throw new Exception("This columnOrdinal is illegal");

                Task toRemove = list[ColumnOrdinal].GetTaskList().Find(x => x.GetTaskId() == taskId);
                list[ColumnOrdinal + 1].AddTask(toRemove.GetTitle(), toRemove.GetDescription(), toRemove.GetDueDate(), taskId);//first tries to add to the next column and removes after if adding succeeded
                Task removed = list[ColumnOrdinal].RemoveTask(taskId);

            }

            /// <summary>
            /// This function adds new task to the board by giving the title, description and dueDate of the new task
            /// </summary>
            /// <param name="title"></param>
            /// <param name="description"></param>
            /// <param name="dueDate"></param>
            /// <returns> This function returns the added task </returns>

            public Task AddTask(string title, string description, DateTime dueDate)
            {
                Task toAdd = list[0].AddTask(title, description, dueDate, idGiver);
                idGiver++;
                return toAdd;
            }
            /// <summary>
            /// getter to the amount of columns in the board
            /// </summary>
            /// <returns>This function returns the amount of columns in the list</returns>

            public int GetNumOfColumns()
            {
                return list.Count;
            }

            public int getIdGiver()
            {
                return idGiver;
            }


            /// <summary>
            /// This function searches a specific column by his name 
            /// </summary>
            /// <param name="ColumnName"></param>
            /// <returns>returns the fit column</returns>

	    public Column GetColumn(string ColumnName)
            {
                bool isFound = false;
                int index = 0;
                while (!isFound & index < list.Count)
                {
                    if (ColumnName.Equals(list[index].GetColumnName()))
                        return GetColumn(index);
                    index++;
                }
                throw new Exception("This Column does not exist");
            }
            /// <summary>
            /// This function searches a specific column by his column ordinal  
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>returns the fit column</returns>
            public Column GetColumn(int columnOrdinal)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This Column does not exist");
                return list[columnOrdinal];
            }
            /// <summary>
            /// This function sets the limit of tasks that can be in a specific column
            /// </summary>
            /// <param name="columnId"></param>
            /// <param name="limit"></param>
            public void SetLimit(int columnId, int limit)
            {
                //Un-needed test for limiting columns 1 and 3
                //if (columnId == 2 | columnId == 0)
                //  throw new Exception("Can not limit the amount of tasks in the first and third columns");
                if (columnId > list.Count - 1 | columnId < 0)
                    throw new Exception("This columnOrdinal does not exist");
                GetColumn(columnId).SetLimit(limit);
            }
            /// <summary>
            /// This function updates the description of a specific task 
            /// by giving a column ordinal and task id to identify the specific task
            /// and by giving a new description to the task
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="description"></param>
            public void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                GetColumn(columnOrdinal).UpdateTaskDescription(taskId, description);
            }

            /// <summary>
            /// This function updates the title of a specific task 
            /// by giving a column ordinal and task id to identify the specific task
            /// and by giving a new title to the task
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="title"></param>
            public void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
            {
                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                GetColumn(columnOrdinal).UpdateTaskTitle(taskId, title);
            }

            /// <summary>
            /// This function updates the due date of a specific task 
            /// by giving a column ordinal and task id to identify the specific task
            /// and by giving a new due date to the task
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="title"></param>
            public void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
            {
                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                GetColumn(columnOrdinal).UpdateTaskDueDate(taskId, dueDate);
            }
            /// <summary>
            /// This function removes a column from the board by using the column ordinal of the unwanted column 
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the removed column</returns>
            public Column RemoveColumn(int columnOrdinal)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                Column removed = GetColumn(columnOrdinal);
                list.Remove(removed);
                return removed;
            }
            /// <summary>
            /// This function is shifting a specific column, one column left by using the column ordinal
            /// of the column we want to shift left
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the shifted column</returns>
            public Column MoveColumnLeft(int columnOrdinal)
            {
                if (columnOrdinal == 0)
                    throw new Exception("You can't move the first column left");

                Column res = GetColumn(columnOrdinal);
                list.Remove(res);
                list.Insert(columnOrdinal - 1, res);

                return res;
            }
            /// <summary>
            /// This function is shifting a specific column, one column right by using the column ordinal
            /// of the column we want to shift right
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the shifted column</returns>
            public Column MoveColumnRight(int columnOrdinal)
            {
                if (columnOrdinal == list.Count-1)
                    throw new Exception("You can't move the last column right");

                Column res = GetColumn(columnOrdinal);
                list.Remove(res);
                list.Insert(columnOrdinal + 1, res);
                return res;
            }
            /// <summary>
            /// This function creates a new column with new name and column ordinal 
            /// and adds this column to the board
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="name"></param>
            /// <returns>This function returns the new column that added to the board</returns>
            public Column AddColumn(int columnOrdinal,string name)
            {
                if (columnOrdinal < 0 | columnOrdinal > list.Count)
                    throw new Exception("The columnOrdinal is ilegal");
                Column add = new Column(name,columnOrdinal);
                list.Insert(columnOrdinal, add);
                return add;
            }

            /// <summary>
            /// This function passing the board to the DAL for saving the data of the board
            /// </summary>
            /// <returns>This function returns other board that represents this board in the DAL</returns>
	        public Boolean ToDalObject()
            {
                foreach(Column myColumn in list)
                {
                    myColumn.ToDalObject().Save();
                }
                return true;
            }
        }


        class BoardController
        {
            private Board activeBoard;

            /// <summary>
            /// Constactor of an empty BoardController
            /// </summary>
            public BoardController()
            {
                activeBoard = null;
            }
            /// <summary>
            /// This function initalizing the active board 
            /// </summary>
            /// <param name="newBoard"></param>
            public void SetActiveBoard(Board newBoard)
            {
                activeBoard = newBoard;
            }
            /// <summary>
            /// Getter to the board
            /// </summary>
            /// <returns>This function returns the active board</returns>
            public Board GetBoard()
            {
                return activeBoard;
            }
            /// <summary>
            /// This function searches a specific column by its column name
            /// </summary>
            /// <param name="columnName"></param>
            /// <returns>This function returns the column which its name is the specific column name we are looking for </returns>
            public Column GetColumn(string columnName)
            {
                return activeBoard.GetColumn(columnName);
            }
            /// <summary>
            /// This function searches a specific column by its column ordinal
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the column which its column ordinal is the specific column name we are looking for</returns>
            public Column GetColumn(int columnOrdinal)
            {
                return activeBoard.GetColumn(columnOrdinal);
            }

            /// <summary>
            /// This function gets new details of task to add to the column (title, description and due date) 
            /// </summary>
            /// <param name="title"></param>
            /// <param name="description"></param>
            /// <param name="dueDate"></param>
            /// <returns>This function returns the new added task</returns>
            public Task AddTask(string title, string description, DateTime dueDate)
            {
                return activeBoard.AddTask(title, description, dueDate);
            }

            /// <summary>
            /// This function is advancing a task by giving the identify details of the task(column ordinal,task id) 
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            public void AdvanceTask(int columnOrdinal, int taskId)
            {
                activeBoard.AdvanceTask(columnOrdinal, taskId);
            }

            /// <summary>
            /// This function gets The identify deatils of specific task(column ordinal and task id)
            /// and gets a new due date and update this task's due date
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="dueDate"></param>
            public void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
            {
                activeBoard.UpdateTaskDueDate(columnOrdinal, taskId, dueDate);
            }

            /// <summary>
            /// /// This function gets The identify deatils of specific task(column ordinal and task id)
            /// and gets a new title date and update this task's title
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="title"></param>
            public void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
            {
                activeBoard.UpdateTaskTitle(columnOrdinal, taskId, title);
            }

            /// <summary>
            /// This function initizaling the limit of a specific column which identified by its column id
            /// </summary>
            /// <param name="columnId"></param>
            /// <param name="limit"></param>
            public void SetLimit(int columnId, int limit)
            {
                activeBoard.SetLimit(columnId, limit);
            }

            /// <summary>
            /// /// This function gets The identify deatils of specific task(column ordinal and task id)
            /// and gets a new description and update this task's description
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="description"></param>
            public void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
                activeBoard.UpdateTaskDescription(columnOrdinal, taskId, description);
            }

            /// <summary>
            /// This function gets the column ordinal of a specific column and removes it from the board
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the removed column</returns>
            public Column RemoveColumn(int columnOrdinal)
            {
                return activeBoard.RemoveColumn(columnOrdinal);
            }

            /// <summary>
            /// This column is shifting a specific column one column left. This column is identify by his column ordinal 
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the shifted column</returns>
            public Column MoveColumnLeft(int columnOrdinal)
            {
                return activeBoard.MoveColumnLeft(columnOrdinal);
            }

            /// <summary>
            /// This column is shifting a specific column one column right. This column is identify by his column ordinal 
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the shifted column</returns>
            public Column MoveColumnRight(int columnOrdinal)
            {
               return activeBoard.MoveColumnRight(columnOrdinal);
            }

            /// <summary>
            /// This function gets details of a new column (column ordinal and name) and add this column to the active board
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="name"></param>
            /// <returns>This function returns the added column</returns>
            public Column AddColumn(int columnOrdinal, string name)
            {
                return activeBoard.AddColumn(columnOrdinal,name);
            }
        }

        class Column : IPersistedObject<DataAccessLayer.Column>
        {
            private List<Task> taskList;
            private int limit;
            private string columnName;
            private int columnOrdinal;
            private string Email;

            /// <summary>
            /// an empty constructor of a new empty column 
            /// </summary>
            public Column()
            {
                taskList = new List<Task>();
                limit = -1;
                columnName = "";
                this.columnOrdinal = 0;
            }
            
            /// <summary>
            /// Construcator of new column which initalized by a name and a column ordinal 
            /// </summary>
            /// <param name="columnName"></param>
            /// <param name="columnOrdinal"></param>
            public Column(string columnName,int columnOrdinal)
            {
                this.taskList = new List<Task>();
                this.columnName = columnName;
                limit = -1;
                this.columnOrdinal = columnOrdinal;
            }

            /// <summary>
            /// Construcator of new column which initalized by list of tasks,
            /// restriction limit of tasks, name and a column ordinal. 
            /// </summary>
            /// <param name="columnName"></param>
            /// <param name="columnOrdinal"></param>
            public Column(List<Task> taskList, int limit, string columnName,int columnOrdinal)
            {
                this.taskList = taskList;
                this.limit = limit;
                this.columnName = columnName;
                this.columnOrdinal = columnOrdinal;
            }

            /// <summary>
            /// Getter of the column ordinal of this Column
            /// </summary>
            /// <returns>This function returns the column ordinal of this column</returns>
            public int GetColumnOrdinal()
            {
                return columnOrdinal;
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
                return toAdd;
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
            public void UpdateTaskDueDate(int taskId, DateTime dueDate)
            {
                Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
                toUpdate.UpdateTaskDueDate(dueDate);
            }

            /// <summary>
            /// This function gets the task id of a specific task which we want to update its title
            /// and a new title and updates it.
            /// </summary>
            /// <param name="taskId"></param>
            /// <param name="title"></param>
            public void UpdateTaskTitle(int taskId, string title)
            {
                Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
                toUpdate.UpdateTaskTitle(title);
            }

            /// <summary>
            /// This function gets the task id of a specific task which we want to update its description
            /// and a new description and updates it.
            /// </summary>
            /// <param name="taskId"></param>
            /// <param name="description"></param>
            public void UpdateTaskDescription(int taskId, string description)
            {
                Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
                toUpdate.UpdateTaskDescription(description);
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

            public DataAccessLayer.Column ToDalObject()
            {
                List<DataAccessLayer.Task> DALList = new List<DataAccessLayer.Task>();
                foreach (Task task in taskList)
                {
                    task.ToDalObject().Save();
                    //DALList.Add(task.ToDalObject());
                }
                return new DataAccessLayer.Column(Email, columnName, columnOrdinal, limit);
            }
        }

        class Task : IPersistedObject<DataAccessLayer.Task>
        {

            private string title;
            private string description;
            private DateTime creationDate;
            private DateTime dueDate;
            private int taskId;

            public Task(string title, string description, DateTime dueDate, int taskId)
            {
                if (!ValidateTitle(title) | !ValidateDescription(description) | !ValidateDueDate(dueDate))
                    throw new Exception("One or more of the parameters illegal");
                this.title = title;
                this.description = description;
                this.creationDate = DateTime.Now;
                this.dueDate = dueDate;
                this.taskId = taskId;
            }

            public Task(string title, string description, DateTime dueDate, int taskId, DateTime creationDate)
            {
                if (!ValidateTitle(title) | !ValidateDescription(description) | !ValidateDueDate(dueDate))
                    throw new Exception("One or more of the parameters illegal");
                this.title = title;
                this.description = description;
                this.creationDate = creationDate;
                this.dueDate = dueDate;
                this.taskId = taskId;
            }

            public DateTime GetCreationDate()
            {
                return creationDate;
            }

            public int GetTaskId()
            {
                return taskId;
            }

            public void SetTaskId(int id)
            {
                this.taskId = id;
            }

            public string GetTitle()
            {
                return title;
            }

            public string GetDescription()
            {
                return description;
            }

            public DateTime GetDueDate()
            {
                return dueDate;
            }

            public void UpdateTaskDueDate(DateTime dueDate)
            {
                if (!ValidateDueDate(dueDate))
                    throw new Exception("The dueDate is Illegal(Date already passed)");
                this.dueDate = dueDate;
            }

            public void UpdateTaskTitle(string title)
            {
                if (!ValidateTitle(title))
                    throw new Exception("Illegal title(over 50 characters or empty)");
                this.title = title;
            }

            public void UpdateTaskDescription(string description)
            {
                if (description == null)
                    description = "";
                if (!ValidateDescription(description))
                    throw new Exception("Description not fit(over than 300 character)");
                this.description = description;
            }

            private bool ValidateTitle(string title)
            {
                return title != null && title.Length <= 50 & title.Length > 0;
            }

            private bool ValidateDescription(string newDesc)
            {
                return newDesc.Length <= 300;
            }

            private bool ValidateDueDate(DateTime newDue)
            {
                return newDue.CompareTo(DateTime.Today) >= 0;//new date is in the future.
            }

            public DataAccessLayer.Task ToDalObject()
            {
                return new DataAccessLayer.Task(title, description, creationDate, dueDate, taskId);
            }
        }
    }
}
