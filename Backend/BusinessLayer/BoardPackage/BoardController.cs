﻿using System;
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
            return activeBoard.AddColumn(columnOrdinal, name, email);
        }
    }
}