using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{
    class ColumnModel
    {
        public string Name { get; set; }
        public string EmptyListErrorMessage { get; set; }
        private ObservableCollection<TaskModel> taskList;
        public ObservableCollection<TaskModel> TaskList
        {
            get => taskList;
            set
            {
                EmptyListErrorMessage = taskList.Count == 0 ? "Empty Column" : "";
                taskList = value;
            }
        }

        public ColumnModel()
        {
            this.TaskList = new ObservableCollection<TaskModel>();
            Name = "";
        }

        public ColumnModel(ObservableCollection<TaskModel> TaskList, string Name)
        {
            this.TaskList = TaskList;
            this.Name = Name;
        }
    }
}
