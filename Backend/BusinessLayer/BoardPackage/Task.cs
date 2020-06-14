using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Task : IPersistedObject<DataAccessLayer.Task>
    {

        private string Title;
        private string Description;
        private DateTime CreationDate;
        private DateTime DueDate;
        private int TaskId;
        private string EmailAssignee;

        const int Title_MAX_LENGTH = 50;
        const int DESC_MAX_LENGTH = 300;
        public Task(string Title, string Description, DateTime DueDate, int TaskId,string EmailAssignee)
        {
            if (!ValidateTitle(Title) | !ValidateDescription(Description) | !ValidateDueDate(DueDate))
                throw new Exception("One or more of the parameters illegal");
            this.Title = Title;
            this.Description = Description;
            this.CreationDate = DateTime.Now;
            this.DueDate = DueDate;
            this.TaskId = TaskId;
            this.EmailAssignee = EmailAssignee;
        }

        //Task constructor for LoadData
        public Task(string Title, string Description, DateTime DueDate, int TaskId, string EmailAssignee, DateTime CreationDate)
        {
            this.Title = Title;
            this.Description = Description;
            this.CreationDate = CreationDate;
            this.DueDate = DueDate;
            this.TaskId = TaskId;
            this.EmailAssignee = EmailAssignee;
        }

        public string GetEmailAssignee()
        {
            return EmailAssignee;
        }

        /// <summary>
        /// Getter to the creation date of the task
        /// </summary>
        /// <returns>returns the creation date</returns>
        public DateTime GetCreationDate()
        {
            return CreationDate;
        }

        /// <summary>
        /// Getter to the id of the task
        /// </summary>
        /// <returns>returns the task id</returns>
        public int GetTaskId()
        {
            return TaskId;
        }

        /// <summary>
        /// Setter to the task id, this function get a new task id and updates the old one to this new task id.
        /// </summary>
        /// <param name="id"></param>
        public void SetTaskId(int id)
        {
            this.TaskId = id;
        }

        public void SetEmailAssignee(string EmailAssignee)
        {
            this.EmailAssignee = EmailAssignee;
        }

        /// <summary>
        /// Getter to the Title of the task
        /// </summary>
        /// <returns>returns the Title of the task</returns>
        public string GetTitle()
        {
            return Title;
        }

        /// <summary>
        /// Getter to the Description of the task 
        /// </summary>
        /// <returns>returns the Description of the task</returns>
        public string GetDescription()
        {
            return Description;
        }

        /// <summary>
        /// Getter to the due date of the task
        /// </summary>
        /// <returns>returns the due date of the task</returns>
        public DateTime GetDueDate()
        {
            return DueDate;
        }

        /// <summary>
        /// This function gets a new due date and updates the older one to the new due date
        /// </summary>
        /// <param name="DueDate"></param>
        public void UpdateTaskDueDate(DateTime DueDate)
        {
            if (!ValidateDueDate(DueDate))
                throw new Exception("The DueDate is Illegal (Date already passed)");
            this.DueDate = DueDate;
        }
        /// <summary>
        /// This function gets a new Title and updates the older one to the new Title
        /// </summary>
        /// <param name="Title"></param>
        public void UpdateTaskTitle(string Title)
        {
            if (!ValidateTitle(Title))
                throw new Exception("Illegal Title (over " + Title_MAX_LENGTH + " characters or empty)");
            this.Title = Title;
        }

        /// <summary>
        /// This function gets a new Description and updates the older one to the new Description
        /// </summary>
        /// <param name="Description"></param>
        public void UpdateTaskDescription(string Description)
        {
            if (Description == null)
                Description = "";
            if (!ValidateDescription(Description))
                throw new Exception("Description not fit (over than " + DESC_MAX_LENGTH + "characters)");
            this.Description = Description;
        }

        /// <summary>
        /// This function gets a Title and checks if the Title is validate
        /// (the Title can't be empty and must contains at most 50 characters)
        /// </summary>
        /// <param name="Title"></param>
        /// <returns>returns true if the Title is validate or false if not</returns>
        private bool ValidateTitle(string Title)
        {
            return Title != null && Title.Length <= Title_MAX_LENGTH & Title.Length > 0;
        }

        /// <summary>
        /// This function gets a Description and checks if the Description is validate
        /// (the Description contains at most 300 characters)
        /// </summary>
        /// <param name="newDesc"></param>
        /// <returns>returns true if the Description is validate or false if not</returns>
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
            return newDue.CompareTo(DateTime.Now) > 0;//new date is in the future.
        }

        public DataAccessLayer.Task ToDalObject(string Email, string column)
        {
            return new DataAccessLayer.Task(Title, Description, CreationDate, DueDate, TaskId, column, Email.ToLower(), EmailAssignee);
        }
    }
}
