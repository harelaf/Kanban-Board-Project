using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{
    class ColumnModel : NotifiableModelObject
    {
        public string Name { get; set; }
        public ObservableCollection<TaskModel> TaskList { get; set; }
        public ObservableCollection<TaskModel> filteredTaskList{ get; set;}

        public ColumnModel(BackendController Controller) : base(Controller)
        {
            this.TaskList = new ObservableCollection<TaskModel>();
            filter("");
            Name = "";
        }

        public ColumnModel(BackendController Controller, ObservableCollection<TaskModel> TaskList, string Name) : base(Controller)
        {
            this.TaskList = TaskList;
            filter("");
            this.Name = Name;
        }

        public void AddTask(TaskModel task)
        {
            TaskList.Add(task);
        }

        public ColumnModel filter(string s)
        {
            if(s==null || s == "")
            {
                filteredTaskList = TaskList;
            }
            else
            {
                filteredTaskList = new ObservableCollection<TaskModel>(TaskList.Where(x => x.Description.Contains(s) | x.Title.Contains(s)));
            }
            return this;
        }
    }
}
