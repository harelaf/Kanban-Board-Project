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
        private BackendController Controller;
        public BoardModel Board;

        public KanbanViewModel()
        {
            this.Controller = new BackendController();
        }

        public KanbanViewModel(BackendController Controller)
        {
            this.Controller = Controller;
        }

    }
}
