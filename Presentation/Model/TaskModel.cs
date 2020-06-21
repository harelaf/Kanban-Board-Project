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
                dueDate = value;
                RaisePropertyChanged("DueDate");
                this.BackgroundBrush = dueDate.Subtract(DateTime.Now).TotalMilliseconds <= 0 ? Brushes.Red :
                DateTime.Now.Subtract(CreationDate).TotalMilliseconds >= (0.75 * dueDate.Subtract(CreationDate).TotalMilliseconds)
                ? Brushes.Orange : Brushes.White; //First check if a red background is needed, then if an orange, otherwise white
            }
        }
        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        private string emailAssignee;
        public string EmailAssignee
        {
            get => emailAssignee;
            set
            {
                emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
                BorderBrush = Controller.Email.ToLower().Equals(EmailAssignee.ToLower()) ? Brushes.Blue : Brushes.White; //Checks if the user is the assignee and then puts a blue border
            }
        }
        public string ColumnName { get; set; }
        private SolidColorBrush borderBrush;
        public SolidColorBrush BorderBrush
        {
            get => borderBrush;
            set
            {
                borderBrush = value;
                RaisePropertyChanged("BorderBrush");
            }
        }

        private int titleFontSize;
        public int TitleFontSize
        {
            get => titleFontSize;
            set
            {
                titleFontSize = value;
                RaisePropertyChanged("TitleFontSize");
            }
        }

        private SolidColorBrush backgroundBrush;
        public SolidColorBrush BackgroundBrush
        {
            get => backgroundBrush;
            set
            {
                backgroundBrush = value;
                RaisePropertyChanged("BackgroundBrush");
            }
        }
        
        public TaskModel(BackendController controller, int id, DateTime CreationDate, DateTime dueDate, string title, string description, string emailAssignee, string ColumnName) : base(controller)
        {
            this.Id = id;
            this.CreationDate = CreationDate;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.EmailAssignee = emailAssignee;
            this.ColumnName = ColumnName;
            this.TitleFontSize = 14;
            this.BorderBrush = Controller.Email.ToLower().Equals(EmailAssignee.ToLower()) ? Brushes.Blue : Brushes.White; //Checks if the user is the assignee and then puts a blue border
            this.BackgroundBrush = this.BackgroundBrush = dueDate.Subtract(DateTime.Now).TotalMilliseconds <= 0 ? Brushes.Red : 
                DateTime.Now.Subtract(CreationDate).TotalMilliseconds >= (0.75 * dueDate.Subtract(CreationDate).TotalMilliseconds) 
                ? Brushes.Orange : Brushes.White; //First check if a red background is needed, then if an orange, otherwise white 
        }
    }
}
