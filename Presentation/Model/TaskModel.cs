using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.Model
{
    class TaskModel : NotifiableModelObject
    {
        public int Id;
        public DateTime CreationDate { get; set; }
        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                if (dueDate == default(DateTime)) { dueDate = value; }
                else
                {
                    int ColId = GetColumnId();
                    if (Controller.UpdateTaskDueDate(Controller.Email, ColId, Id, value)) dueDate = value;
                    BackgroundBrush = dueDate.Subtract(DateTime.Now).TotalMilliseconds <= 0 ? Brushes.Red :
                        DateTime.Now.Subtract(CreationDate).TotalMilliseconds >= (0.75 * dueDate.Subtract(CreationDate).TotalMilliseconds)
                        ? Brushes.Orange : Brushes.White;
                }
            }
        }
        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
            }
        }
        private string emailAssignee;
        public string EmailAssignee
        {
            get => emailAssignee;
            set
            {
                emailAssignee = value;
            }
        }
        public string ColumnName { get; set; }
        public SolidColorBrush BorderBrush { get; set; }
        public SolidColorBrush BackgroundBrush { get; set; }
        public TaskModel(BackendController controller, int id, DateTime CreationDate, DateTime dueDate, string title, string description, string emailAssignee, string ColumnName) : base(controller)
        {
            this.Id = id;
            this.CreationDate = CreationDate;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.EmailAssignee = emailAssignee;
            this.ColumnName = ColumnName;
            this.BorderBrush = Controller.Email.Equals(EmailAssignee) ? Brushes.Blue : Brushes.White;
            this.BackgroundBrush = this.BackgroundBrush = dueDate.Subtract(DateTime.Now).TotalMilliseconds <= 0 ? Brushes.Red : 
                DateTime.Now.Subtract(CreationDate).TotalMilliseconds >= (0.75 * dueDate.Subtract(CreationDate).TotalMilliseconds) 
                ? Brushes.Orange : Brushes.White;
        }

        private int GetColumnId()
        {
            BoardModel board = Controller.GetBoard(Controller.Email);
            int ColId = 0;
            foreach (ColumnModel col in board.ColList)
            {
                if (col.Name.Equals(ColumnName)) break;
                ColId++;
            }
            return ColId;
        }
    }
}
