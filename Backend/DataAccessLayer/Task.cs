using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Task : DalObject<Task>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public int TaskId { get; set; }

        public Task(string Title,string Description,DateTime CreationDate,DateTime DueDate,int TaskId)
        {
            this.Title = Title;
            this.Description = Description;
            this.CreationDate = CreationDate;
            this.DueDate = DueDate;
            this.TaskId = TaskId;
        }

        public Task()
        {
            Title = null;
            Description = null;
            CreationDate = new DateTime();
            DueDate = new DateTime();
            TaskId = 0;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override Task Import()
        {
            throw new NotImplementedException();
        }

        public override string FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
