﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{ 
    class BoardService
    {
        private readonly BoardController boardController;
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardService()
        {
            boardController = new BoardController();
        }

        public void SetActiveBoard(BusinessLayer.BoardPackage.Board newBoard)
        {
            boardController.SetActiveBoard(newBoard);
        }

        public Response<Task> AddTask(string title, string description, DateTime dueDate)
        {
            Response<Task> response;
            if(boardController.GetBoard() == null)
            {
                log.Warn("Tried addding a task with no user connected");
                response = new Response<Task>("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Task returnedTask = boardController.AddTask(title, description, dueDate);
                response = new Response<Task>(new Task(returnedTask.GetTaskId(), returnedTask.GetCreationDate(), returnedTask.GetDueDate(), returnedTask.GetTitle(), returnedTask.GetDescription()));
                log.Debug("Task added successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed at adding a task: " + e.Message);
                response = new Response<Task>(e.Message);
            }
            return response;
        }

        public Response AdvanceTask(int columnOrdinal, int taskId, string Email)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                log.Warn("Tried advancing a task with no user connected");
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                boardController.AdvanceTask(columnOrdinal, taskId, Email);
                response = new Response();
                log.Debug("Task has advanced successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed at advancing a task: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public Response UpdateTaskTitle(int columnOrdinal, int taskId, string title, string Email)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                log.Warn("Tried to update a task's title when a user wasn't connected");
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Task toSave = boardController.UpdateTaskTitle(columnOrdinal, taskId, title);
                response = new Response();
                log.Debug("Updated a task's title successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed at Updating a Task  Title: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public Response UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate, string Email)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                log.Warn("Tried to update a task's due date when a user wasn't connected");
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Task toSave = boardController.UpdateTaskDueDate(columnOrdinal, taskId, dueDate);
                response = new Response();
                log.Debug("Updated a task's due date successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed at Updating a Task DueDate: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public Response UpdateTaskDescription(int columnOrdinal, int taskId, string description, string Email)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                log.Warn("Tried to update a task's description when a user wasn't connected");
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Task toSave = boardController.UpdateTaskDescription(columnOrdinal, taskId, description);
                response = new Response();
                log.Debug("Updated a task's description successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed at Updating a Task Descriptiona: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public Response LimitColumnTasks(int columnOrdinal, int limit)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                log.Warn("Tried to update a column's task limit when a user wasn't connected");
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                boardController.SetLimit(columnOrdinal, limit);
                BusinessLayer.BoardPackage.Column toSave = boardController.GetColumn(columnOrdinal);
                response = new Response();
                log.Debug("Updated a column's task limit successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed at setting a new column limit: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public Response<Column> GetColumn(string columnName)
        {
            Response<Column> response;
            if (boardController.GetBoard() == null)
            {
                log.Warn("Tried to get a column when a user wasn't connected");
                response = new Response<Column>("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Column column = boardController.GetColumn(columnName);
                List<Task> serviceTasks = new List<Task>();
                foreach (BusinessLayer.BoardPackage.Task task in column.GetTaskList())
                {
                    serviceTasks.Add(new Task(task.GetTaskId(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription()));
                }
                Column responseColumn = new Column(serviceTasks, columnName, column.GetLimit());
                response = new Response<Column>(responseColumn);
            }
            catch (Exception e)
            {
                log.Warn("Failed at getting the wanted column: " + e.Message);

                response = new Response<Column>(e.Message);
            }
            return response;
        }

        public Response<Column> GetColumn(int columnOrdinal)
        {
            Response<Column> response;
            if (boardController.GetBoard() == null)
            {
                log.Warn("Tried to get a column when a user wasn't connected");
                response = new Response<Column>("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Column column = boardController.GetColumn(columnOrdinal);
                List<Task> serviceTasks = new List<Task>();
                foreach (BusinessLayer.BoardPackage.Task task in column.GetTaskList())
                {
                    serviceTasks.Add(new Task(task.GetTaskId(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription()));
                }
                Column responseColumn;
                responseColumn = new Column(serviceTasks, column.GetColumnName(), column.GetLimit());
                response = new Response<Column>(responseColumn);
            }
            catch (Exception e)
            {
                log.Warn("Failed at getting the wanted column: " + e.Message);
                response = new Response<Column>(e.Message);
            }
            return response;
        }

        public Response<Board> GetBoard()
        {
            Response<Board> response;
            BusinessLayer.BoardPackage.Board activeBoard = boardController.GetBoard();
            if (activeBoard == null)
            {
                log.Warn("Tried finding a board without a user connected");
                response = new Response<Board>("No user is logged in");
                return response;
            }
            List<string> names = new List<string>();
            
            foreach(BusinessLayer.BoardPackage.Column c in activeBoard.GetColumns())
            {
                names.Add(c.GetColumnName());
            }

            Board responseBoard = new Board(names);
            response = new Response<Board>(responseBoard);
            return response;
        }

        public Response<Column> AddColumn(int columnOrdinal,string name, string email)
        {
            
            try
            {
                BusinessLayer.BoardPackage.Column column = boardController.AddColumn(columnOrdinal, name, email);

                List<Task> tasks = new List<Task>();
                foreach (BusinessLayer.BoardPackage.Task task in column.GetTaskList())
                {
                    Task add = new Task(task.GetTaskId(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription());
                    tasks.Add(add);
                }

                Column responseColumn = new Column(tasks, name, column.GetLimit());
                log.Debug("Added the column successfully");
                return new Response<Column>(responseColumn);
            }
            catch (Exception e)
            {
                log.Warn("Failed at adding the wanted column: " + e.Message);
                return new Response<Column>(e.Message);
            }
           
        }

        public Response<Column> RemoveColumn(int columnOrdinal)
        {
            try
            {
                BusinessLayer.BoardPackage.Column removed = boardController.RemoveColumn(columnOrdinal);
                List<Task> tasks = new List<Task>();
                foreach(BusinessLayer.BoardPackage.Task task in removed.GetTaskList())
                {
                    Task add = new Task(task.GetTaskId(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription());
                    tasks.Add(add);
                }

                Column responseColumn = new Column(tasks, removed.GetColumnName(), removed.GetLimit());
                log.Debug("Removed the column successfully");
                return new Response<Column>(responseColumn);
            }
            catch(Exception e)
            {
                log.Warn("Failed at removing the wanted column: " + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        public Response<Column>MoveColumnRight(int columnOrdinal)
        {
            try
            {
                BusinessLayer.BoardPackage.Column moved = boardController.MoveColumnRight(columnOrdinal);
                List<Task> tasks = new List<Task>();
                foreach(BusinessLayer.BoardPackage.Task task in moved.GetTaskList())
                {
                    Task add = new Task(task.GetTaskId(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription());
                    tasks.Add(add);
                }
                Column responseColumn = new Column(tasks, moved.GetColumnName(), moved.GetLimit());
                log.Debug("Moved the column right successfully");
                return new Response<Column>(responseColumn);
            }catch(Exception e)
            {
                log.Warn("Failed at moving right the wanted column: " + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        public Response<Column> MoveColumnLeft(int columnOrdinal)
        {
            try
            {
                BusinessLayer.BoardPackage.Column moved = boardController.MoveColumnLeft(columnOrdinal);
                List<Task> tasks = new List<Task>();
                foreach (BusinessLayer.BoardPackage.Task task in moved.GetTaskList())
                {
                    Task add = new Task(task.GetTaskId(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription());
                    tasks.Add(add);
                }
                Column responseColumn = new Column(tasks, moved.GetColumnName(), moved.GetLimit());
                log.Debug("Moved the column left successfully");
                return new Response<Column>(responseColumn);
            }
            catch (Exception e)
            {
                log.Warn("Failed at moving left the wanted column: " + e.Message);
                return new Response<Column>(e.Message);
            }
        }


    }
}
