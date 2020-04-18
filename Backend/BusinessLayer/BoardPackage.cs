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
        class Board : IPersistedObject<DataAccessLayer.Board>
        {
            private Column backlog;
            private Column inProgress;
            private Column done;

            public Board()
            {
                backlog = new Column();
                inProgress = new Column();
                done = new Column();
            }

            public Board(Column backlog, Column inProgress, Column done)
            {
                this.backlog = backlog;
                this.inProgress = inProgress;
                this.done = done;
            }

            public void AdvanceTask(int ColumnOrdinal, int taskId)
            {

                if (ColumnOrdinal == 2)
                    throw new Exception("Can't advance mission that is already done");
                if (ColumnOrdinal == 1)
                {
                    Task removed = inProgress.RemoveTask(taskId);
                    done.AddTask(removed.GetTitle(), removed.GetDescription(), removed.GetDueDate());
                }
                else if (ColumnOrdinal == 0)
                {
                    Task removed = backlog.RemoveTask(taskId);
                    inProgress.AddTask(removed.GetTitle(), removed.GetDescription(), removed.GetDueDate());
                }
                else
                {
                    throw new Exception("This columnOrdinal is illegal");
                }

            }

            public Task AddTask(string title, string description, DateTime dueDate)
            {
                return backlog.AddTask(title, description, dueDate);
            }

            public Column GetColumn(string ColumnName)
            {
                if (ColumnName.Equals("backlog"))
                    return backlog;
                else if (ColumnName.Equals("in progress"))
                    return inProgress;
                else if (ColumnName.Equals("done"))
                    return done;
                else
                    throw new Exception("This Column does not exist");
            }

            public Column GetColumn(int columnOrdinal)
            {
                if (columnOrdinal == 0)
                    return backlog;
                else if (columnOrdinal == 1)
                    return inProgress;
                else if (columnOrdinal == 2)
                    return done;
                else
                    throw new Exception("This Column does not exist");
            }

            public void SetLimit(int columnId, int limit)
            {
                GetColumn(columnId).SetLimit(limit);
            }

            public void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
                GetColumn(columnOrdinal).UpdateTaskDescription(taskId, description);
            }

            public void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
            {
                GetColumn(columnOrdinal).UpdateTaskTitle(taskId, title);
            }

            public void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
            {
                GetColumn(columnOrdinal).UpdateTaskDueDate(taskId, dueDate);
            }

            public DataAccessLayer.Board ToDalObject()
            {
                return new DataAccessLayer.Board(backlog.ToDalObject(), inProgress.ToDalObject(), done.ToDalObject());
            }
        }


        class BoardController
        {
            private Board activeBoard;

            public BoardController()
            {
                activeBoard = null;
            }

            public void SetActiveBoard(Board newBoard)
            {
                activeBoard = newBoard;
            }

            public Board GetBoard()
            {
                return activeBoard;
            }

            public Column GetColumn(string columnName)
            {
               return activeBoard.GetColumn(columnName);
            }

            public Column GetColumn(int columnOrdinal )
            {
                return activeBoard.GetColumn(columnOrdinal);
            }

            public Task AddTask(string title, string description, DateTime dueDate)
            {
                return activeBoard.AddTask(title, description, dueDate);
            }

            public void AdvanceTask(int columnOrdinal, int taskId)
            {
                activeBoard.AdvanceTask(columnOrdinal, taskId);
            }

            public void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
            {
                activeBoard.UpdateTaskDueDate(columnOrdinal, taskId, dueDate);
            }

            public void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
            {
                activeBoard.UpdateTaskTitle(columnOrdinal, taskId, title);
            }

            public void SetLimit(int columnId,int limit)
            {
                activeBoard.SetLimit(columnId, limit);
            }

            public void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
              activeBoard.UpdateTaskDescription(columnOrdinal, taskId, description);
            }


        }

        class Column : IPersistedObject<DataAccessLayer.Column>
        {
            List<Task> taskList;
            int limit;

            public Column()
            {
                taskList = new List<Task>();
                limit = -1;
            }

            public Column(List<Task> taskList, int limit)
            {
                this.taskList = taskList;
                this.limit = limit;
            }

            public Task AddTask(string title, string description, DateTime dueDate)
            {
                if (taskList.Count == limit)
                {
                    throw new Exception("Can't add new task, column has a limit of " + limit);
                }
                Task toAdd = new Task(title, description, dueDate, taskList.Count);
                taskList.Add(toAdd);
                return toAdd;
            }

            public Task RemoveTask(int taskId)
            {
                Task toRemove = taskList[taskId];
                taskList.Remove(toRemove);
                foreach (Task task in taskList)
                {
                    task.SetTaskId(taskList.IndexOf(task));
                }
                return toRemove;
            }

            public void UpdateTaskDueDate(int taskId, DateTime dueDate)
            {
                Task toUpdate = taskList.ElementAt(taskId);
                toUpdate.UpdateTaskDueDate(dueDate);
            }

            public void UpdateTaskTitle(int taskId, string title)
            {
                Task toUpdate = taskList.ElementAt(taskId);
                toUpdate.UpdateTaskTitle(title);
            }

            public void UpdateTaskDescription(int taskId, string description)
            {
                Task toUpdate = taskList.ElementAt(taskId);
                toUpdate.UpdateTaskDescription(description);
            }

            public void SetLimit(int newLim)
            {
                if (taskList.Count > newLim)
                {
                    throw new Exception("This column already has more tasks than the new limit");
                }
                this.limit = newLim;
            }

            public int GetLimit()
            {
                return limit;
            }

            public List<Task> GetTaskList()
            {
                return taskList;
            }

            public DataAccessLayer.Column ToDalObject()
            {
                List <DataAccessLayer.Task> DALList = new List<DataAccessLayer.Task>();
                foreach (Task task in taskList)
                {
                    DALList.Add(task.ToDalObject());
                }
                return new DataAccessLayer.Column(DALList, limit);
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
                if (!ValidateDescription(description))
                    throw new Exception("Description not fit(over than 300 character)");
                this.description = description;
                      
            }

            private bool ValidateTitle(string title)
            {
                return title.Length <= 50 & title.Length > 0;
            }

            private bool ValidateDescription(string newDesc)
            {
                return newDesc.Length <= 300;
            }

            private bool ValidateDueDate(DateTime newDue)
            {
                return newDue.CompareTo(DateTime.Now) > 0;
            }

            public DataAccessLayer.Task ToDalObject()
            {
                return new DataAccessLayer.Task(title, description, creationDate, dueDate, taskId);
            }
        }
    }
}
