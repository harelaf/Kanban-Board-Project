using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    namespace BoardPackage
    {
        class Board
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

            public void AdvanceTask(int ColumnOrdinal, int taskId)
            {

                if (ColumnOrdinal == 3)
                    throw new Exception("Can't advance mission that is already done");
                if (ColumnOrdinal == 2)
                {
                    Task removed = inProgress.RemoveTask(taskId);
                    done.AddTask(removed.GetTitle(), removed.GetDescription(), removed.GetDueDate());
                }
                else if (ColumnOrdinal == 1)
                {
                    Task removed = backlog.RemoveTask(taskId);
                    inProgress.AddTask(removed.GetTitle(), removed.GetDescription(), removed.GetDueDate());
                }
                else
                {
                    throw new Exception("This columnOrdinal Illegal");
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
                else if (ColumnName.Equals("inProgress"))
                    return inProgress;
                else if (ColumnName.Equals("done"))
                    return done;
                else
                    throw new Exception("This Column is not exist");
            }

            public Column GetColumn(int columnOrdinal)
            {
                if (columnOrdinal == 1)
                    return backlog;
                else if (columnOrdinal == 2)
                    return inProgress;
                else if (columnOrdinal == 3)
                    return done;
                else
                    throw new Exception("This Column is not exist");
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

        }


        class BoardController
        {
            private Board activeBoard;

            public BoardController()
            {
                activeBoard = null;
            }

            public BoardController(Board activeBoard)
            {
                this.activeBoard = activeBoard;
            }


            public Column GetColumn(string columnName)
            {
               return activeBoard.GetColumn(columnName);
            }

            public Column GetColumn(int columnOrdinal)
            {
                return activeBoard.GetColumn(columnOrdinal);
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

        class Column
        {
            List<Task> taskList;
            int limit;

            public Column()
            {
                taskList = new List<Task>();
                limit = -1;
            }

            public Task AddTask(string title, string description, DateTime dueDate)
            {
                if (taskList.Count == limit)
                {
                    throw new Exception("Can't add new task, column has a limit of " + limit);
                }
                Task toAdd = new Task(title, description, dueDate);
                taskList.Add(toAdd);
                return toAdd;
            }

            public Task RemoveTask(int taskId)
            {
                Task toRemove = taskList.ElementAt(taskId);
                taskList.Remove(toRemove);
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

        }

        class Task
        {

            private string title;
            private string description;
            private DateTime creationDate;
            private DateTime dueDate;

            public Task(string title, string description, DateTime dueDate)
            {
                if (!ValidateTitle(title) | !ValidateDescription(description) | !ValidateDueDate(dueDate))
                    throw new Exception("One or more of the parameters illegal");
                this.title = title;
                this.description = description;
                this.creationDate = DateTime.Now;
                this.dueDate = dueDate;
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
                    throw new Exception("The dueDate is Illegal(already passed)");
                this.dueDate = dueDate;
            }

            public void UpdateTaskTitle(string title)
            {
                if (!ValidateTitle(title))
                    throw new Exception("Illegal title(over 50 characters)");
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
                return title.Length <= 50;
            }

            private bool ValidateDescription(string newDesc)
            {
                return newDesc.Length <= 300;
            }

            private bool ValidateDueDate(DateTime newDue)
            {
                bool res = newDue.Year >= DateTime.Now.Year;
                res = res & newDue.Month >= DateTime.Now.Month;
                res = res & newDue.Day >= DateTime.Now.Day;
                res = res & newDue.Hour >= DateTime.Now.Hour;
                res = res & newDue.Minute >= DateTime.Now.Minute;
                res = res & newDue.Second >= DateTime.Now.Second;
                return res;
            }
        }
    }
}
