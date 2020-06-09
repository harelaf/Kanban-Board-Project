using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class KanbanViewModel : NotifiableObject
    {
        private BackendController controller;
        public BoardModel board;

        public KanbanViewModel()
        {
            this.controller = new BackendController();
        }

        
    }
}
