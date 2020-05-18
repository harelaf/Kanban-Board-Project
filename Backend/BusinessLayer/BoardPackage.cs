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

            public Board()
            {
                for (int i = 0; i < 3; i++)
                    list[i] = new Column();
                idGiver = 0;
            }

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
            public void AdvanceTask(int ColumnOrdinal, int taskId, string Email)
            {
                if (ColumnOrdinal == list.Count - 1)//cannot advance further than 'done'.
                    throw new Exception("Can't advance mission that is already done");

                if (ColumnOrdinal > list.Count - 1 | ColumnOrdinal < 0)
                    throw new Exception("This columnOrdinal is illegal");

                Task toRemove = list[ColumnOrdinal].GetTaskList().Find(x => x.GetTaskId() == taskId);
                list[ColumnOrdinal + 1].AddTask(toRemove.GetTitle(), toRemove.GetDescription(), toRemove.GetDueDate(), taskId);//first tries to add to the next column and removes after if adding succeeded
                Task removed = list[ColumnOrdinal].RemoveTask(taskId);
                toRemove.ToDalObject(Email, ColumnOrdinal).Save();
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
            /// <summary>
            /// Getter to the idGiver 
            /// </summary>
            /// <returns>This function returns the idGiver field</returns>

            public int getIdGiver()
            {
                return idGiver;
            }

            public List<Column> GetColumns()
            {
                return list;
            }


            /// <summary>
            /// This function searches a specific column by his name 
            /// </summary>
            /// <param name="ColumnName"></param>
            /// <returns>returns the fit column</returns>

<<<<<<< HEAD
            public Column GetColumn(string ColumnName)
=======
	        public Column GetColumn(string ColumnName)
>>>>>>> aa3e7949062134494312405435234c7b34b45ebc
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
            /// This function searches a specific column by his column ordinal and returns it 
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
            /// <returns>returns the updated task</returns>
            public Task UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                return GetColumn(columnOrdinal).UpdateTaskDescription(taskId, description);
            }

            /// <summary>
            /// This function updates the title of a specific task 
            /// by giving a column ordinal and task id to identify the specific task
            /// and by giving a new title to the task
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="title"></param>
            /// <returns>returns the updated task</returns>
            public Task UpdateTaskTitle(int columnOrdinal, int taskId, string title)
            {
                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                return GetColumn(columnOrdinal).UpdateTaskTitle(taskId, title);
            }

            /// <summary>
            /// This function updates the due date of a specific task 
            /// by giving a column ordinal and task id to identify the specific task
            /// and by giving a new due date to the task
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="title"></param>
            /// <returns>returns the updated task</returns>
            public Task UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
            {
                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                return GetColumn(columnOrdinal).UpdateTaskDueDate(taskId, dueDate);
            }
            /// <summary>
            /// This function removes a column from the board by using the column ordinal of the unwanted column 
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the removed column</returns>
            public Column RemoveColumn(int columnOrdinal)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                {
                    throw new Exception("This columnOrdinal does not exist");
                }

                Column removed = GetColumn(columnOrdinal);
                Column toAddTo;
                if (columnOrdinal == 0)
                {
                    toAddTo = GetColumn(columnOrdinal + 1);
                    if(removed.GetTaskList().Count > toAddTo.GetLimit() - toAddTo.GetTaskList().Count)
                    {
                        throw new Exception("There isn't enough available space in the right column");
                    }
                    foreach (Task toMove in removed.GetTaskList())
                    {
                        toAddTo.MoveExistingTaskHere(toMove);
                        toMove.ToDalObject(removed.getEmail(), list.IndexOf(toAddTo)).Save();
                    }
                }
                else
                {
                    toAddTo = GetColumn(columnOrdinal - 1);
                    if (removed.GetTaskList().Count > toAddTo.GetLimit() - toAddTo.GetTaskList().Count)
                    {
                        throw new Exception("There isn't enough available space in the left column");
                    }
                    foreach (Task toMove in removed.GetTaskList())
                    {
                        toAddTo.MoveExistingTaskHere(toMove);
                        toMove.ToDalObject(removed.getEmail(), list.IndexOf(toAddTo)).Save();
                    }
                }
                list.Remove(removed);
                removed.ToDalObject(removed.getEmail(), removed.GetColumnOrdinal()).Delete();
                toAddTo.ToDalObject(toAddTo.getEmail(), list.IndexOf(toAddTo)).Save();
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

                Column toMove = GetColumn(columnOrdinal);
                Column Moved = GetColumn(columnOrdinal - 1);
                list.Remove(toMove);
                list.Insert(columnOrdinal - 1, toMove);
                toMove.ToDalObject(toMove.getEmail(), columnOrdinal - 1).Save();
                Moved.ToDalObject(Moved.getEmail(), list.IndexOf(Moved)).Save();
                return toMove;
            }
            /// <summary>
            /// This function is shifting a specific column, one column right by using the column ordinal
            /// of the column we want to shift right
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <returns>This function returns the shifted column</returns>
            public Column MoveColumnRight(int columnOrdinal)
            {
                if (columnOrdinal == list.Count - 1)
                    throw new Exception("You can't move the last column right");

                Column toMove = GetColumn(columnOrdinal);
                Column Moved = GetColumn(columnOrdinal + 1);
                list.Remove(toMove);
                list.Insert(columnOrdinal + 1, toMove);
                toMove.ToDalObject(toMove.getEmail(), columnOrdinal + 1).Save();
                Moved.ToDalObject(Moved.getEmail(), list.IndexOf(Moved)).Save();
                return toMove;
            }
            /// <summary>
            /// This function creates a new column with new name and column ordinal 
            /// and adds this column to the board
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="name"></param>
            /// <returns>This function returns the new column that added to the board</returns>
<<<<<<< HEAD
            public Column AddColumn(int columnOrdinal, string name)
            {
                if (columnOrdinal < 0 | columnOrdinal > list.Count)
                    throw new Exception("The columnOrdinal is ilegal");
                Column add = new Column(name, columnOrdinal);
=======
            public Column AddColumn(int columnOrdinal, string name, string email)
            {
                if (columnOrdinal < 0 | columnOrdinal > list.Count)
                    throw new Exception("The columnOrdinal is ilegal");
                foreach (Column col in list)
                {
                    if (col.GetColumnName() == name)
                    {
                        throw new Exception("There is already a column with that name");
                    }
                }
                Column add = new Column(name,columnOrdinal,email);
>>>>>>> aa3e7949062134494312405435234c7b34b45ebc
                list.Insert(columnOrdinal, add);
                add.ToDalObject(email, columnOrdinal).Save();
                foreach (Column toUpdate in list)
                {
                    if(toUpdate.GetColumnOrdinal() > columnOrdinal)
                    {
                        toUpdate.ToDalObject(email, list.IndexOf(toUpdate)).Save();
                    }
                }
                return add;
            }

        }

        class BoardController
        {
            private Board activeBoard;


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
            public void AdvanceTask(int columnOrdinal, int taskId, string Email)
            {
                activeBoard.AdvanceTask(columnOrdinal, taskId, Email);
            }

            /// <summary>
            /// This function gets The identify deatils of specific task(column ordinal and task id)
            /// and gets a new due date and update this task's due date
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="dueDate"></param>
            /// <returns>returns the updated task</returns>
            public Task UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
            {
                return activeBoard.UpdateTaskDueDate(columnOrdinal, taskId, dueDate);
            }

            /// <summary>
            /// /// This function gets The identify deatils of specific task(column ordinal and task id)
            /// and gets a new title date and update this task's title
            /// </summary>
            /// <param name="columnOrdinal"></param>
            /// <param name="taskId"></param>
            /// <param name="title"></param>
            /// <returns><returns>returns the updated task</returns></returns>
            public Task UpdateTaskTitle(int columnOrdinal, int taskId, string title)
            {
                return activeBoard.UpdateTaskTitle(columnOrdinal, taskId, title);
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
            /// <returns>returns the updated task</returns>
            public Task UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
                return activeBoard.UpdateTaskDescription(columnOrdinal, taskId, description);
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
            /// <param name="email"></param>
            /// <returns>This function returns the added column</returns>
            public Column AddColumn(int columnOrdinal, string name, string email)
            {
<<<<<<< HEAD
                return activeBoard.AddColumn(columnOrdinal, name);
=======
                return activeBoard.AddColumn(columnOrdinal,name,email);
>>>>>>> aa3e7949062134494312405435234c7b34b45ebc
            }
        }

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
<<<<<<< HEAD

            public Column(string columnName, int columnOrdinal)
=======
            
            /// <summary>
            /// Construcator of new column which initalized by a name and a column ordinal 
            /// </summary>
            /// <param name="columnName"></param>
            /// <param name="columnOrdinal"></param>
            /// <param name="email"></param>
            public Column(string columnName,int columnOrdinal, string email)
>>>>>>> aa3e7949062134494312405435234c7b34b45ebc
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
                if (taskList.Count == limit)
                {
                    throw new Exception("Can't add new task, column has a limit of " + limit);
                }
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
<<<<<<< HEAD
            {
                return new DataAccessLayer.Column(Email, columnName, columnOrdinal, limit);
=======
            { 
                return new DataAccessLayer.Column(Email, columnName, colOrdinal, limit);
>>>>>>> aa3e7949062134494312405435234c7b34b45ebc
            }
        }

        class Task : IPersistedObject<DataAccessLayer.Task>
        {

            private string title;
            private string description;
            private DateTime creationDate;
            private DateTime dueDate;
            private int taskId;

            const int TITLE_MAX_LENGTH = 50;
            const int DESC_MAX_LENGTH = 300;
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

            /// <summary>
            /// Getter to the creation date of the task
            /// </summary>
            /// <returns>returns the creation date</returns>
            public DateTime GetCreationDate()
            {
                return creationDate;
            }

            /// <summary>
            /// Getter to the id of the task
            /// </summary>
            /// <returns>returns the task id</returns>
            public int GetTaskId()
            {
                return taskId;
            }

            /// <summary>
            /// Setter to the task id, this function get a new task id and updates the old one to this new task id.
            /// </summary>
            /// <param name="id"></param>
            public void SetTaskId(int id)
            {
                this.taskId = id;
            }

            /// <summary>
            /// Getter to the title of the task
            /// </summary>
            /// <returns>returns the title of the task</returns>
            public string GetTitle()
            {
                return title;
            }

            /// <summary>
            /// Getter to the description of the task 
            /// </summary>
            /// <returns>returns the description of the task</returns>
            public string GetDescription()
            {
                return description;
            }

            /// <summary>
            /// Getter to the due date of the task
            /// </summary>
            /// <returns>returns the due date of the task</returns>
            public DateTime GetDueDate()
            {
                return dueDate;
            }

            /// <summary>
            /// This function gets a new due date and updates the older one to the new due date
            /// </summary>
            /// <param name="dueDate"></param>
            public void UpdateTaskDueDate(DateTime dueDate)
            {
                if (!ValidateDueDate(dueDate))
                    throw new Exception("The dueDate is Illegal(Date already passed)");
                this.dueDate = dueDate;
            }
            /// <summary>
            /// This function gets a new title and updates the older one to the new title
            /// </summary>
            /// <param name="title"></param>
            public void UpdateTaskTitle(string title)
            {
                if (!ValidateTitle(title))
                    throw new Exception("Illegal title(over "+TITLE_MAX_LENGTH+" characters or empty)");
                this.title = title;
            }

            /// <summary>
            /// This function gets a new description and updates the older one to the new description
            /// </summary>
            /// <param name="description"></param>
            public void UpdateTaskDescription(string description)
            {
                if (description == null)
                    description = "";
                if (!ValidateDescription(description))
                    throw new Exception("Description not fit(over than "+DESC_MAX_LENGTH+"characters)");
                this.description = description;
            }

            /// <summary>
            /// This function gets a title and checks if the title is validate
            /// (the title can't be empty and must contains at most 50 characters)
            /// </summary>
            /// <param name="title"></param>
            /// <returns>returns true if the title is validate or false if not</returns>
            private bool ValidateTitle(string title)
            {
                return title != null && title.Length <= TITLE_MAX_LENGTH & title.Length > 0;
            }

            /// <summary>
            /// This function gets a description and checks if the description is validate
            /// (the description contains at most 300 characters)
            /// </summary>
            /// <param name="newDesc"></param>
            /// <returns>returns true if the description is validate or false if not</returns>
            private bool ValidateDescription(string newDesc)
            {
                return newDesc.Length <= DESC_MAX_LENGTH;
            }

            /// <summary>
            /// This function gets a new due date and checks if this new due date is possible
            /// (not passed yet)
            /// </summary>
            /// <param name="newDue"></param>
            /// <returns>returns true if the due date is validate or false if not</returns>
            private bool ValidateDueDate(DateTime newDue)
            {
                return newDue.CompareTo(DateTime.Today) >= 0;//new date is in the future.
            }

            public DataAccessLayer.Task ToDalObject(string Email, string column)
            {
                return new DataAccessLayer.Task(title, description, creationDate, dueDate, taskId, column, Email);
            }
        }
    }
}
