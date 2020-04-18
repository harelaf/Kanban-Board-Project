using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class Column : DalObject<Column>
    {
        public List<Task> TaskList { get; set; }
        public int Limit { get; set; }

        public Column(List<Task>TaskList, int Limit)
        {
            this.TaskList = TaskList;
            this.Limit = Limit;
        }

        public Column()
        {
            TaskList=new List<Task>();
            Limit = 0;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override Column Import()
        {
            throw new NotImplementedException();
        }

        public override string FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
