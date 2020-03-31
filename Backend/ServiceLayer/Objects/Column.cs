using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit)
        {
            this.Tasks = tasks;
            this.Name = name;
            this.Limit = limit;
        }
        // You can add code here
    }
}
