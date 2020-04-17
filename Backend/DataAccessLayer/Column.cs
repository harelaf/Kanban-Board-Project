using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Column
    {
        public List<Task> TaskList { get; set; }
        public int Limit { get; set; }
        public int IdGiver { get; set; }

        public Column(List<Task>TaskList,int Limit,int IdGiver)
        {
            this.TaskList = TaskList;
            this.Limit = Limit;
            this.IdGiver = IdGiver;
        }

        public Column()
        {
            TaskList=new List<Task>();
            Limit = 0;
            IdGiver = 0;
        }


    }
}
