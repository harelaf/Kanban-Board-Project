using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class TaskModel : NotifiableModelObject
    {
        public int Id;
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EmailAssignee { get; set; }
        public string ColumnName { get; set; }

        public TaskModel(BackendController controller,int id, DateTime CreationDate, DateTime dueDate, string title, string description, string emailAssignee, string ColumnName) : base(controller)
        {
            this.Id = id;
            this.CreationDate = CreationDate;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.EmailAssignee = emailAssignee;
            this.ColumnName = ColumnName;
        }

      
    }
}
