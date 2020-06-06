using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
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
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <param name="DueDate"></param>
        /// <returns>This function returns the new added task</returns>
        public Task AddTask(string Email, string Title, string Description, DateTime DueDate)
        {
            return activeBoard.AddTask(Email, Title, Description, DueDate);
        }

        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            activeBoard.AssignTask(email, columnOrdinal, taskId, emailAssignee);
        }

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            activeBoard.DeleteTask(email, columnOrdinal, taskId);
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            activeBoard.ChangeColumnName(email, columnOrdinal, newName);
        }

        /// <summary>
        /// This function is advancing a task by giving the identify details of the task(column ordinal,task id) 
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        public void AdvanceTask(int ColumnOrdinal, int TaskId, string Email)
        {
            activeBoard.AdvanceTask(ColumnOrdinal, TaskId, Email);
        }

        /// <summary>
        /// This function gets The identify deatils of specific task(column ordinal and task id)
        /// and gets a new due date and update this task's due date
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskDueDate(string Email,int ColumnOrdinal, int TaskId, DateTime DueDate)
        {
            return activeBoard.UpdateTaskDueDate(Email,ColumnOrdinal, TaskId, DueDate);
        }

        /// <summary>
        /// /// This function gets The identify deatils of specific task(column ordinal and task id)
        /// and gets a new title date and update this task's title
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <returns><returns>returns the updated task</returns></returns>
        public Task UpdateTaskTitle(string Email,int ColumnOrdinal, int TaskId, string Title)
        {
            return activeBoard.UpdateTaskTitle(Email,ColumnOrdinal, TaskId, Title);
        }

        /// <summary>
        /// This function initizaling the limit of a specific column which identified by its column id
        /// </summary>
        /// <param name="columnId"></param>
        /// <param name="limit"></param>
        public void SetLimit(string Email,int ColumnId, int Limit)
        {
            activeBoard.SetLimit(Email,ColumnId, Limit);
        }

        /// <summary>
        /// /// This function gets The identify deatils of specific task(column ordinal and task id)
        /// and gets a new description and update this task's description
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        /// <returns>returns the updated task</returns>
        public Task UpdateTaskDescription(string Email,int ColumnOrdinal, int TaskId, string Description)
        {
            return activeBoard.UpdateTaskDescription(Email,ColumnOrdinal, TaskId, Description);
        }

        /// <summary>
        /// This function gets the column ordinal of a specific column and removes it from the board
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <returns>This function returns the removed column</returns>
        /// ////////////////////to complete/////////////////////////////
        public Column RemoveColumn(string Email,int ColumnOrdinal)
        {
            return activeBoard.RemoveColumn(Email,ColumnOrdinal);
        }

        /// <summary>
        /// This column is shifting a specific column one column left. TC:\Users\יועד אוחיון\Git-Workspace\milestones-2-doom\Backend\BusinessLayer\BoardPackage\BoardController.cshis column is identify by his column ordinal 
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <returns>This function returns the shifted column</returns>
        public Column MoveColumnLeft(string Email,int ColumnOrdinal)
        {
            return activeBoard.MoveColumnLeft(Email,ColumnOrdinal);
        }

        /// <summary>
        /// This column is shifting a specific column one column right. This column is identify by his column ordinal 
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <returns>This function returns the shifted column</returns>
        public Column MoveColumnRight(string Email,int ColumnOrdinal)
        {
            return activeBoard.MoveColumnRight(Email,ColumnOrdinal);
        }

        /// <summary>
        /// This function gets details of a new column (column ordinal and name) and add this column to the active board
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns>This function returns the added column</returns>
        public Column AddColumn(int ColumnOrdinal, string Name, string Email)
        {
            return activeBoard.AddColumn(ColumnOrdinal, Name, Email);
        }

        public void DeleteTask(string Email, int ColumnOrdinal, int TaskId)
        {
            activeBoard.DeleteTask(Email, ColumnOrdinal, TaskId);
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            activeBoard.ChangeColumnName(email, columnOrdinal, newName);
        }
    }
}
