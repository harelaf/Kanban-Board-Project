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

            public Task AddTask(string title, string description, DateTime dueDate)
            {
                Task toAdd = list[0].AddTask(title, description, dueDate, idGiver);
                idGiver++;
                return toAdd;
            }

            public int GetNumOfColumns()
            {
                return list.Count;
            }

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

            public Column GetColumn(int columnOrdinal)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This Column does not exist");
                return list[columnOrdinal];
            }

            public void SetLimit(int columnId, int limit)
            {
                //Un-needed test for limiting columns 1 and 3
                //if (columnId == 2 | columnId == 0)
                //  throw new Exception("Can not limit the amount of tasks in the first and third columns");
                if (columnId > list.Count - 1 | columnId < 0)
                    throw new Exception("This columnOrdinal does not exist");
                GetColumn(columnId).SetLimit(limit);
            }

            public void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                GetColumn(columnOrdinal).UpdateTaskDescription(taskId, description);
            }

            public void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
            {
                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                GetColumn(columnOrdinal).UpdateTaskTitle(taskId, title);
            }

            public void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
            {
                if (columnOrdinal == list.Count - 1)
                    throw new Exception("Cannot change tasks that are in the done column");

                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                GetColumn(columnOrdinal).UpdateTaskDueDate(taskId, dueDate);
            }

            public Column RemoveColumn(int columnOrdinal)
            {
                if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                    throw new Exception("This columnOrdinal does not exist");

                Column removed = GetColumn(columnOrdinal);
                list.Remove(removed);
                return removed;
            }

            public Column MoveColumnLeft(int columnOrdinal)
            {
                if (columnOrdinal == 0)
                    throw new Exception("You can't move the first column left");

                Column res = GetColumn(columnOrdinal);
                list.Remove(res);
                list.Insert(columnOrdinal - 1, res);

                return res;
            }

            public Column MoveColumnRight(int columnOrdinal)
            {
                if (columnOrdinal == list.Count-1)
                    throw new Exception("You can't move the last column right");

                Column res = GetColumn(columnOrdinal);
                list.Remove(res);
                list.Insert(columnOrdinal + 1, res);
                return res;
            }
            
            public Column AddColumn(int columnOrdinal,string name)
            {
                if (columnOrdinal < 0 | columnOrdinal > list.Count)
                    throw new Exception("The columnOrdinal is ilegal");
                Column add = new Column(name);
                list.Insert(columnOrdinal, add);
                return add;
            }

            public DataAccessLayer.Board ToDalObject()
            {
                return new DataAccessLayer.Board(backlog.ToDalObject(), inProgress.ToDalObject(), done.ToDalObject(), idGiver);
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

            public Column GetColumn(int columnOrdinal)
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

            public void SetLimit(int columnId, int limit)
            {
                activeBoard.SetLimit(columnId, limit);
            }

            public void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
            {
                activeBoard.UpdateTaskDescription(columnOrdinal, taskId, description);
            }

            public Column RemoveColumn(int columnOrdinal)
            {
                return activeBoard.RemoveColumn(columnOrdinal);
            }

            public Column MoveColumnLeft(int columnOrdinal)
            {
                return activeBoard.MoveColumnLeft(columnOrdinal);
            }

            public Column MoveColumnRight(int columnOrdinal)
            {
               return activeBoard.MoveColumnRight(columnOrdinal);
            }

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
            public Column()
            {
                taskList = new List<Task>();
                limit = -1;
                columnName = "";
            }
            
            public Column(string columnName)
            {
                this.taskList = new List<Task>();
                this.columnName = columnName;
                limit = -1;
            }

            public Column(List<Task> taskList, int limit, string columnName)
            {
                this.taskList = taskList;
                this.limit = limit;
                this.columnName = columnName;
            }

            public string GetColumnName()
            {
                return columnName;
            }

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

            public Task RemoveTask(int taskId)
            {
                Task toRemove = taskList.Find(x => x.GetTaskId() == taskId);
                taskList.Remove(toRemove);
                /*
                foreach (Task task in taskList)
                {
                    task.SetTaskId(taskList.IndexOf(task));
                }
                */
                return toRemove;
            }

            public void UpdateTaskDueDate(int taskId, DateTime dueDate)
            {
                Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
                toUpdate.UpdateTaskDueDate(dueDate);
            }

            public void UpdateTaskTitle(int taskId, string title)
            {
                Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
                toUpdate.UpdateTaskTitle(title);
            }

            public void UpdateTaskDescription(int taskId, string description)
            {
                Task toUpdate = taskList.Find(x => x.GetTaskId() == taskId);
                toUpdate.UpdateTaskDescription(description);
            }

            public void SetLimit(int newLim)
            {
                if (taskList.Count > newLim & newLim >= 0)
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
                List<DataAccessLayer.Task> DALList = new List<DataAccessLayer.Task>();
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
