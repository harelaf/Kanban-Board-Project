using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class ColumnModel
    {
        private List<TaskModel> TaskList;

        public ColumnModel(List<TaskModel> TaskList)
        {
            this.TaskList = TaskList;
        }
    }
}
