using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Board : IPersistedObject<DataAccessLayer.Board>
    {

        private List<Column> list;
        private int IdGiver;

        const int MAX_LENGTH_COLUMN_NAME = 15;
        const int MIN_AMOUNT_OF_COLUMNS = 2;

        public Board()
        {
            list = new List<Column>();
            IdGiver = 0;
        }

        public Board(string email)
        {
            list = new List<Column>();
            AddColumn(0, "backlog", email);
            AddColumn(1, "in progress", email);
            AddColumn(2, "done", email);
            IdGiver = 0;
        }

        public Board(List<Column> list, int IdGiver)
        {
            this.list = list;
            this.IdGiver = IdGiver;
        }

        /// <summary>
        /// This function advance task from one column to the next column
        /// </summary>
        /// <param name="ColumnOrdinal"></param>
        /// <param name="taskId"></param>
        public void AdvanceTask(int ColumnOrdinal, int TaskId, string Email)
        {
            if (ColumnOrdinal == list.Count - 1)//cannot advance further than 'done'.
                throw new Exception("Can't advance mission that is already done");

            if (ColumnOrdinal > list.Count - 1 | ColumnOrdinal < 0)
                throw new Exception("This columnOrdinal is illegal");

            Task toRemove = list[ColumnOrdinal].GetTaskList().Find(x => x.GetTaskId() == TaskId);
            Column toAddto = list[ColumnOrdinal + 1];
            
            toAddto.AddTask(toRemove.GetTitle(), toRemove.GetDescription(), toRemove.GetDueDate(), TaskId, Email);//first tries to add to the next column and removes after if adding succeeded
            Task removed = list[ColumnOrdinal].RemoveTask(TaskId,Email);
          
            toRemove.ToDalObject(Email, toAddto.GetColumnName()).Save();
        }

        /// <summary>
        /// This function adds new task to the board by giving the title, description and dueDate of the new task
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <returns> This function returns the added task </returns>

        public Task AddTask(string Email,string Title, string Description, DateTime DueDate)
        {
            Task toAdd = list[0].AddTask(Title, Description, DueDate, IdGiver, Email);
            IdGiver++;
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
            return IdGiver;
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
        public void SetLimit(string Email,int ColumnId, int Limit)
        {
            //Un-needed test for limiting columns 1 and 3
            //if (columnId == 2 | columnId == 0)
            //  throw new Exception("Can not limit the amount of tasks in the first and third columns");
            if (ColumnId > list.Count - 1 | ColumnId < 0)
                throw new Exception("This columnOrdinal does not exist");
            if (!GetColumn(ColumnId).getEmail().Equals(Email))
                throw new Exception("This Column Limit can't changed because this user is not the creator of the column");
            Column toUpdate = GetColumn(ColumnId);
            toUpdate.SetLimit(Limit);
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
        public Task UpdateTaskDescription(string Email,int ColumnOrdinal, int TaskId, string Description)
        {
            if (ColumnOrdinal > list.Count - 1 | ColumnOrdinal < 0)
                throw new Exception("This columnOrdinal does not exist");

            if (ColumnOrdinal == list.Count - 1)
                throw new Exception("Cannot change tasks that are in the done column");

            return GetColumn(ColumnOrdinal).UpdateTaskDescription(Email,TaskId, Description);
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
        public Task UpdateTaskTitle(string Email,int columnOrdinal, int taskId, string title)
        {
            if (columnOrdinal == list.Count - 1)
                throw new Exception("Cannot change tasks that are in the done column");

            if (columnOrdinal > list.Count - 1 | columnOrdinal < 0)
                throw new Exception("This columnOrdinal does not exist");

            return GetColumn(columnOrdinal).UpdateTaskTitle(Email,taskId, title);
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
        public Task UpdateTaskDueDate(string Email,int ColumnOrdinal, int TaskId, DateTime DueDate)
        {
            if (ColumnOrdinal == list.Count - 1)
                throw new Exception("Cannot change tasks that are in the done column");

            if (ColumnOrdinal > list.Count - 1 | ColumnOrdinal < 0)
                throw new Exception("This columnOrdinal does not exist");

            return GetColumn(ColumnOrdinal).UpdateTaskDueDate(Email,TaskId, DueDate);
        }
        /// <summary>
        /// This function removes a column from the board by using the column ordinal of the unwanted column 
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <returns>This function returns the removed column</returns>
        public Column RemoveColumn(string Email,int ColumnOrdinal)
        {
            if (list.Count == MIN_AMOUNT_OF_COLUMNS)
            {
                throw new Exception($"A board must have at least {MIN_AMOUNT_OF_COLUMNS} columns");
            }
            else if (ColumnOrdinal > list.Count - 1 | ColumnOrdinal < 0)
            {
                throw new Exception("This columnOrdinal does not exist");
            }

            Column removed = GetColumn(ColumnOrdinal);
            Column toAddTo = ColumnOrdinal == 0 ? GetColumn(ColumnOrdinal + 1) : GetColumn(ColumnOrdinal - 1);

            if (toAddTo.GetLimit() != -1 && removed.GetTaskList().Count > toAddTo.GetLimit() - toAddTo.GetTaskList().Count)
            {
                throw new Exception("There isn't enough available space in the right column");
            }

            if (!Email.Equals(removed.getEmail()))
                throw new Exception("This user can't remove this column because he is not the creator of this column");

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
        public Column MoveColumnLeft(string Email,int ColumnOrdinal)
        {
            if (ColumnOrdinal == 0)
                throw new Exception("You can't move the first column left");
            if (!Email.Equals(GetColumn(ColumnOrdinal).getEmail()))
                throw new Exception("This user cant move this column left because he is not the creator of the column");

            Column toMove = GetColumn(ColumnOrdinal);
            Column Moved = GetColumn(ColumnOrdinal - 1);
            list.Remove(toMove);
            list.Insert(ColumnOrdinal - 1, toMove);
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
        public Column MoveColumnRight(string Email,int ColumnOrdinal)
        {
            if (ColumnOrdinal == list.Count - 1)
                throw new Exception("You can't move the last column right");
            if (!Email.Equals(GetColumn(ColumnOrdinal).getEmail()))
                throw new Exception("This user cant move this column right because he is not the creator of the column");
            Column toMove = GetColumn(ColumnOrdinal);
            Column Moved = GetColumn(ColumnOrdinal + 1);
            list.Remove(toMove);
            list.Insert(ColumnOrdinal + 1, toMove);
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

        public Column AddColumn(int ColumnOrdinal, string Name, string Email)
        {
            if (ColumnOrdinal < 0 | ColumnOrdinal > list.Count)
                throw new Exception("The columnOrdinal is ilegal");
            if (Name == null)
            {
                throw new Exception("The name you entered for the column is null");
            }
            else if (Name.Length > MAX_LENGTH_COLUMN_NAME)
            {
                throw new Exception("The name you entered for the column is too long");
            }

            if (list.Count != 0 & !list[0].getEmail().Equals(Email))
                throw new Exception("This user can't add a new column because he is not the creator");

            foreach (Column col in list)
            {
                if (col.GetColumnName().Equals(Name))
                {
                    throw new Exception("There is already a column with that name");
                }
            }
            Column add = new Column(Name, ColumnOrdinal, Email);
            list.Insert(ColumnOrdinal, add);
            foreach (Column toUpdate in list)
            {
                if (toUpdate.GetColumnOrdinal() >= ColumnOrdinal)
                {
                    toUpdate.SetColumnOrdinal(list.IndexOf(toUpdate));
                    toUpdate.ToDalObject(Email, "").Save();
                }
            }
            return add;
        }

        /// <summary>
        /// implements the method ToDalObject as promised. turn current board to dal board who is ready to save.
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public DataAccessLayer.Board ToDalObject(string Email, string column)
        {
            DataAccessLayer.Board DalBoard = new DataAccessLayer.Board(Email, IdGiver, GetNumOfColumns());///////////////////////////////////////
            return new DataAccessLayer.Board();
        }
        
        public void DeleteTask(string Email, int ColumnOrdinal, int TaskId)
        {
            if (ColumnOrdinal < 0 | ColumnOrdinal > list.Count)
                throw new Exception("The columnOrdinal is ilegal");
            Column column = list[ColumnOrdinal];
            column.RemoveTask(TaskId, Email);
            list[ColumnOrdinal] = column;
        }

        public void ChangeColumnName(string Email, int ColumnOrdinal, string NewName)
        {
            if (ColumnOrdinal < 0 | ColumnOrdinal > list.Count)
                throw new Exception("The columnOrdinal is ilegal");

            if (!list[ColumnOrdinal].getEmail().Equals(Email))
                throw new Exception("You can't change the column name because you are not the creator of this column");

            foreach(Column column in list)
            {
                if (column.GetColumnName().Equals(NewName) & column.GetColumnOrdinal() != ColumnOrdinal)
                    throw new Exception("you can't change the column name to this new name because there is already a column with this name");
            }

            list[ColumnOrdinal].SetColumnName(NewName);
        }

    }
}
