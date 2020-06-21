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
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public ObservableCollection<TaskModel> TaskList { get; set; }
        public ObservableCollection<TaskModel> filteredTaskList{ get; set;}
        private int limit;
        public int Limit
        {
            get => limit;
            set
            {
                limit = value;
                RaisePropertyChanged("Limit");
            }
        }
        public string filteredBy;

        public ColumnModel(BackendController Controller) : base(Controller)
        {
            this.TaskList = new ObservableCollection<TaskModel>();
            filter(null);
            Name = "";
            Limit = 0;
        }

        public ColumnModel(BackendController Controller, ObservableCollection<TaskModel> TaskList, string Name, int Limit) : base(Controller)
        {
            this.TaskList = TaskList;
            filter("");
            this.Name = Name;
            this.Limit = Limit;
        }

        /// <summary>
        /// Adds a task to this columns task list
        /// </summary>
        /// <param name="task">The task to add</param>
        public void AddTask(TaskModel task)
        {
            TaskList.Add(task);
            filteredTaskList = new ObservableCollection<TaskModel>(TaskList.Where(x => x.Description.Contains(filteredBy) | x.Title.Contains(filteredBy)));
        }

        /// <summary>
        /// Filters the task list with a given string
        /// </summary>
        /// <param name="s">the string to filter with</param>
        /// <returns>returns the filtered column</returns>
        public ColumnModel filter(string s)
        {
            filteredBy = s==null ? "" : s;
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
