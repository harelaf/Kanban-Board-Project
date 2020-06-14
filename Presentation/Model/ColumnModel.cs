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
            TaskList.CollectionChanged += HandleChange;
        }

        public ColumnModel(BackendController Controller, ObservableCollection<TaskModel> TaskList, string Name) : base(Controller)
        {
            this.TaskList = TaskList;
            filter("");
            this.Name = Name;
            TaskList.CollectionChanged += HandleChange;
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

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                
            }
        }
    }
}
