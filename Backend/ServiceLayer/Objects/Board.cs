using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly IReadOnlyCollection<string> ColumnsNames;
		public readonly string emailCreator;
        internal Board(IReadOnlyCollection<string> columnsNames, string emailCreator) 
        {
            this.ColumnsNames = columnsNames;
			this.emailCreator = emailCreator;
        }
        // You can add code here
    }
}
