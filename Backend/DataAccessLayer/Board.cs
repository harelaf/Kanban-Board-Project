using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{


    class Board
    {
        public Column BackLog { get; set; }
        public Column InProgress { get; set; }
        public Column Done { get; set; }

        public Board()
        {
            BackLog = new Column();
            InProgress = new Column();
            Done = new Column();
        }

        public Board(Column BackLog,Column InProgress,Column Done)
        {
            this.BackLog = BackLog;
            this.InProgress = InProgress;
            this.Done = Done;
        }
    }
}
