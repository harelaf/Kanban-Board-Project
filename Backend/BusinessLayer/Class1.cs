using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    namespace BoardPackage
    {
        class Column
        {
            List<Task> taskList;
            int limit;
            
            public Column()
            {
                taskList = new List<Task>();
                limit = -1;
            }

            public Task AddTask(string title, string description, DateTime dueDate)
            {
                Task toAdd = new Task(title, description, dueDate);
                taskList.Add(toAdd);
                return toAdd;
            }

            public void RemoveTask(int taskId)
            {
                taskList.RemoveAt(taskId);
            }


        }

        class Task
        {

        }
    }
}
