using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private readonly BoardController boardController;

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
                response = new Response<Task>("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Task returnedTask = boardController.AddTask(title, description, dueDate);
                response = new Response<Task>(new Task(returnedTask.GetTaskId(), returnedTask.GetCreationDate(), returnedTask.GetTitle(), returnedTask.GetDescription()));
            }
            catch (Exception e)
            {
                response = new Response<Task>(e.Message);
            }
            return response;
        }

        public Response AdvanceTask(int columnOrdinal, int taskId)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                boardController.AdvanceTask(columnOrdinal, taskId);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e.Message);
            }
            return response;
        }

        public Response UpdateTaskTitle(int columnOrdinal, int taskId, string title)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                boardController.UpdateTaskTitle(columnOrdinal, taskId, title);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e.Message);
            }
            return response;
        }

        public Response UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                boardController.UpdateTaskDueDate(columnOrdinal, taskId, dueDate);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e.Message);
            }
            return response;
        }

        public Response UpdateTaskDescription(int columnOrdinal, int taskId, string description)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                boardController.UpdateTaskDescription(columnOrdinal, taskId, description);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e.Message);
            }
            return response;
        }

        public Response LimitColumnTasks(int columnOrdinal, int limit)
        {
            Response response;
            if (boardController.GetBoard() == null)
            {
                response = new Response("No user is logged in");
                return response;
            }
            try
            {
                boardController.SetLimit(columnOrdinal, limit);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e.Message);
            }
            return response;
        }

        public Response<Column> GetColumn(string columnName)
        {
            Response<Column> response;
            if (boardController.GetBoard() == null)
            {
                response = new Response<Column>("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Column column = boardController.GetColumn(columnName);
                List<Task> serviceTasks = new List<Task>();
                foreach (BusinessLayer.BoardPackage.Task task in column.GetTaskList())
                {
                    serviceTasks.Add(new Task(task.GetTaskId(), task.GetCreationDate(), task.GetTitle(), task.GetDescription()));
                }
                Column responseColumn = new Column(serviceTasks, columnName, column.GetLimit());
                response = new Response<Column>(responseColumn);
            }
            catch (Exception e)
            {
                response = new Response<Column>(e.Message);
            }
            return response;
        }

        public Response<Column> GetColumn(int columnOrdinal)
        {
            Response<Column> response;
            if (boardController.GetBoard() == null)
            {
                response = new Response<Column>("No user is logged in");
                return response;
            }
            try
            {
                BusinessLayer.BoardPackage.Column column = boardController.GetColumn(columnOrdinal);
                List<Task> serviceTasks = new List<Task>();
                foreach (BusinessLayer.BoardPackage.Task task in column.GetTaskList())
                {
                    serviceTasks.Add(new Task(task.GetTaskId(), task.GetCreationDate(), task.GetTitle(), task.GetDescription()));
                }
                Column responseColumn;
                if (columnOrdinal == 0)
                    responseColumn = new Column(serviceTasks, "backlog", column.GetLimit());
                else if(columnOrdinal == 1)
                    responseColumn = new Column(serviceTasks, "in progress", column.GetLimit());
                else
                    responseColumn = new Column(serviceTasks, "done", column.GetLimit());
                response = new Response<Column>(responseColumn);
            }
            catch (Exception e)
            {
                response = new Response<Column>(e.Message);
            }
            return response;
        }

        public Response<Board> GetBoard()
        {
            Response<Board> response;
            if (boardController.GetBoard() == null)
            {
                response = new Response<Board>("No user is logged in");
                return response;
            }
            List<string> names = new List<string>();
            names.Add("backlog");
            names.Add("in progress");
            names.Add("done");
            Board responseBoard = new Board(names);
            response = new Response<Board>(responseBoard);
            return response;
        }
    }
}
