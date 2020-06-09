using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class KanbanViewModel
    {
        public List<TaskModel> board;
        private string[] testk;
        public string[] test
        {
            get { return testk; }
            set
            {
                testk = value;

            }
        }

        public KanbanViewModel()
        {
            
        }

        
    }
}
