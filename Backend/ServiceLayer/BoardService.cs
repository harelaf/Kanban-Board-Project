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

        public BoardService(BusinessLayer.BoardPackage.Board activeBoard)
        {
            boardController = new BoardController(activeBoard);
        }

        public Response<Task> addTask(string email, string title, string description, DateTime dueDate)
        {
            Response<Task> response;
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

        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Response response;
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
    }
}
