using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddTaskViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        public ColumnModel Column { get; set; }

        public AddTaskViewModel()
        {
            Controller = new BackendController();
            Column = new ColumnModel(Controller);
        }

        public AddTaskViewModel(BackendController controller)
        {
            this.Controller = controller;
            Column = new ColumnModel(Controller);
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

        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
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

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        public void AddTask(ObservableCollection<ColumnModel> columns)
        {
            try
            {
                TaskModel task = Controller.AddTask(Controller.Email, Title, Description, DueDate);
                Column = columns[0];
                Column.AddTask(task);
                ErrorMessage = "Added the task successfully!";
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
