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
        private string Name { get; set; }
        private ObservableCollection<TaskModel> taskList;
        public ObservableCollection<TaskModel> TaskList
        {
            get => taskList;
            set
            {
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
