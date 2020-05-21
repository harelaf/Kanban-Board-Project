using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Task : IPersistedObject<DataAccessLayer.Task>
    {

        private string title;
        private string description;
        private DateTime creationDate;
        private DateTime dueDate;
        private int taskId;

        const int TITLE_MAX_LENGTH = 50;
        const int DESC_MAX_LENGTH = 300;
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

        /// <summary>
        /// Getter to the creation date of the task
        /// </summary>
        /// <returns>returns the creation date</returns>
        public DateTime GetCreationDate()
        {
            return creationDate;
        }

        /// <summary>
        /// Getter to the id of the task
        /// </summary>
        /// <returns>returns the task id</returns>
        public int GetTaskId()
        {
            return taskId;
        }

        /// <summary>
        /// Setter to the task id, this function get a new task id and updates the old one to this new task id.
        /// </summary>
        /// <param name="id"></param>
        public void SetTaskId(int id)
        {
            this.taskId = id;
        }

        /// <summary>
        /// Getter to the title of the task
        /// </summary>
        /// <returns>returns the title of the task</returns>
        public string GetTitle()
        {
            return title;
        }

        /// <summary>
        /// Getter to the description of the task 
        /// </summary>
        /// <returns>returns the description of the task</returns>
        public string GetDescription()
        {
            return description;
        }

        /// <summary>
        /// Getter to the due date of the task
        /// </summary>
        /// <returns>returns the due date of the task</returns>
        public DateTime GetDueDate()
        {
            return dueDate;
        }

        /// <summary>
        /// This function gets a new due date and updates the older one to the new due date
        /// </summary>
        /// <param name="dueDate"></param>
        public void UpdateTaskDueDate(DateTime dueDate)
        {
            if (!ValidateDueDate(dueDate))
                throw new Exception("The dueDate is Illegal(Date already passed)");
            this.dueDate = dueDate;
        }
        /// <summary>
        /// This function gets a new title and updates the older one to the new title
        /// </summary>
        /// <param name="title"></param>
        public void UpdateTaskTitle(string title)
        {
            if (!ValidateTitle(title))
                throw new Exception("Illegal title(over " + TITLE_MAX_LENGTH + " characters or empty)");
            this.title = title;
        }

        /// <summary>
        /// This function gets a new description and updates the older one to the new description
        /// </summary>
        /// <param name="description"></param>
        public void UpdateTaskDescription(string description)
        {
            if (description == null)
                description = "";
            if (!ValidateDescription(description))
                throw new Exception("Description not fit(over than " + DESC_MAX_LENGTH + "characters)");
            this.description = description;
        }

        /// <summary>
        /// This function gets a title and checks if the title is validate
        /// (the title can't be empty and must contains at most 50 characters)
        /// </summary>
        /// <param name="title"></param>
        /// <returns>returns true if the title is validate or false if not</returns>
        private bool ValidateTitle(string title)
        {
            return title != null && title.Length <= TITLE_MAX_LENGTH & title.Length > 0;
        }

        /// <summary>
        /// This function gets a description and checks if the description is validate
        /// (the description contains at most 300 characters)
        /// </summary>
        /// <param name="newDesc"></param>
        /// <returns>returns true if the description is validate or false if not</returns>
        private bool ValidateDescription(string newDesc)
        {
            return newDesc.Length <= DESC_MAX_LENGTH;
        }

        /// <summary>
        /// This function gets a new due date and checks if this new due date is possible
        /// (not passed yet)
        /// </summary>
        /// <param name="newDue"></param>
        /// <returns>returns true if the due date is validate or false if not</returns>
        private bool ValidateDueDate(DateTime newDue)
        {
            return newDue.CompareTo(DateTime.Today) >= 0;//new date is in the future.
        }

        public DataAccessLayer.Task ToDalObject(string Email, string column)
        {
            return new DataAccessLayer.Task(title, description, creationDate, dueDate, taskId, column, Email.ToLower());
        }
    }
}
