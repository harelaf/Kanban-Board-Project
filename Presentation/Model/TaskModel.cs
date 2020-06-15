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
        public DateTime DueDate { get; set; }
        private string title;
        public string Title {
            get => title;
            set
            {
                Controller.UpdateTaskTitle(Controller.Email, Controller.GetColumn(Controller.Email, ColumnName)., Id, value);
                title = value;
            }
        }
        public string Description { get; set; }
        public string EmailAssignee { get; set; }
        public string ColumnName { get; set; }
        public SolidColorBrush BorderBrush { get; set; }
        public SolidColorBrush BackgroundBrush { get; set; }
        public TaskModel(BackendController controller,int id, DateTime CreationDate, DateTime dueDate, string title, string description, string emailAssignee, string ColumnName) : base(controller)
        {
            this.Id = id;
            this.CreationDate = CreationDate;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.EmailAssignee = emailAssignee;
            this.ColumnName = ColumnName;
            this.BorderBrush = Controller.Email.Equals(EmailAssignee) ? Brushes.Blue : Brushes.White;
            this.BackgroundBrush = (DateTime.Now.Subtract(CreationDate)).TotalMilliseconds >= (0.75 * (dueDate.Subtract(CreationDate)).TotalMilliseconds) ? Brushes.Orange : Brushes.White;
            this.BackgroundBrush = (dueDate.Subtract(DateTime.Now)).TotalMilliseconds <= 0 ? Brushes.Red : BackgroundBrush;
        }
    }
}
