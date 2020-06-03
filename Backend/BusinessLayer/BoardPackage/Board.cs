using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Board
    {

        private List<Column> list;
        private int idGiver;

        const int MAX_LENGTH_COLUMN_NAME = 15;
        const int MIN_AMOUNT_OF_COLUMNS = 2;

        public Board()
        {
            list = new List<Column>();
            idGiver = 0;
        }

        public Board(string email)
        {
            list = new List<Column>();
            AddColumn(0, "backlog", email);
            AddColumn(1, "in progress", email);
            AddColumn(2, "done", email);
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
            Column toAddto = list[ColumnOrdinal + 1];
            toAddto.AddTask(toRemove.GetTitle(), toRemove.GetDescription(), toRemove.GetDueDate(), taskId);//first tries to add to the next column and removes after if adding succeeded
            Task removed = list[ColumnOrdinal].RemoveTask(taskId);
            toRemove.ToDalObject(Email, toAddto.GetColumnName()).Save();
        }

        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {

        }

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {

        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {

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
            Column toUpdate = GetColumn(columnId);
            toUpdate.SetLimit(limit);
            toUpdate.ToDalObject(toUpdate.getEmail(), "").Save();
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
            if (list.Count == MIN_AMOUNT_OF_COLUMNS)
            {
                throw new Exception($"A board must have at least {MIN_AMOUNT_OF_COLUMNS} columns");
            }
            else if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
            {
                throw new Exception("This columnOrdinal does not exist");
            }

            Column removed = GetColumn(columnOrdinal);
            Column toAddTo = columnOrdinal == 0 ? GetColumn(columnOrdinal + 1) : GetColumn(columnOrdinal - 1);

            if (toAddTo.GetLimit() != -1 && removed.GetTaskList().Count > toAddTo.GetLimit() - toAddTo.GetTaskList().Count)
            {
                throw new Exception("There isn't enough available space in the right column");
            }
            foreach (Task toMove in removed.GetTaskList())
            {
                toAddTo.MoveExistingTaskHere(toMove);
                toMove.ToDalObject(removed.getEmail(), toAddTo.GetColumnName()).Save();
            }

            list.Remove(removed);
            foreach (Column col in list)
            {
                if (col.GetColumnOrdinal() >= removed.GetColumnOrdinal())
                {
                    col.SetColumnOrdinal(list.IndexOf(col));
                    col.ToDalObject(col.getEmail(), "").Save();
                }
            }
            removed.ToDalObject(removed.getEmail(), "").Delete();
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
            toMove.SetColumnOrdinal(list.IndexOf(toMove));
            Moved.SetColumnOrdinal(list.IndexOf(Moved));
            toMove.ToDalObject(toMove.getEmail(), "").Save();
            Moved.ToDalObject(Moved.getEmail(), "").Save();
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
            toMove.SetColumnOrdinal(list.IndexOf(toMove));
            Moved.SetColumnOrdinal(list.IndexOf(Moved));
            toMove.ToDalObject(toMove.getEmail(), "").Save();
            Moved.ToDalObject(Moved.getEmail(), "").Save();
            return toMove;
        }
        /// <summary>
        /// This function creates a new column with new name and column ordinal 
        /// and adds this column to the board
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="name"></param>
        /// <returns>This function returns the new column that added to the board</returns>

        public Column AddColumn(int columnOrdinal, string name, string email)
        {
            if (columnOrdinal < 0 | columnOrdinal > list.Count)
                throw new Exception("The columnOrdinal is ilegal");
            if (name == null)
            {
                throw new Exception("The name you entered for the column is null");
            }
            else if (name.Length > MAX_LENGTH_COLUMN_NAME)
            {
                throw new Exception("The name you entered for the column is too long");
            }

            foreach (Column col in list)
            {
                if (col.GetColumnName() == name)
                {
                    throw new Exception("There is already a column with that name");
                }
            }
            Column add = new Column(name, columnOrdinal, email);
            list.Insert(columnOrdinal, add);
            foreach (Column toUpdate in list)
            {
                if (toUpdate.GetColumnOrdinal() >= columnOrdinal)
                {
                    toUpdate.SetColumnOrdinal(list.IndexOf(toUpdate));
                    toUpdate.ToDalObject(email, "").Save();
                }
            }
            return add;
        }

    }
}
