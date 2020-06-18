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

        /// <summary>
        /// always get an existing controller
        /// </summary>
        /// <param name="controller"></param>
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
            }
        }

        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
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

        /// <summary>
        /// displays all kind of messages to the user
        /// </summary>
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

        /// <summary>
        /// add new task to the first column and displays a proper message
        /// </summary>
        /// <param name="columns"></param>
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
