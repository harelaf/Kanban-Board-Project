using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class TaskModel
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly DateTime DueDate;
        public readonly string Title;
        public readonly string Description;
        public readonly string EmailAssignee;

        public TaskModel(int id, DateTime creationTime, DateTime dueDate, string title, string description, string emailAssignee)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.EmailAssignee = emailAssignee;
        }
        
    }
}
