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
        public int Limit { get; set; }

        public ColumnModel(BackendController Controller) : base(Controller)
        {
            this.TaskList = new ObservableCollection<TaskModel>();
            filter("");
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
        }

        /// <summary>
        /// Filters the task list with a given string
        /// </summary>
        /// <param name="s">the string to filter with</param>
        /// <returns>returns the filtered column</returns>
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
