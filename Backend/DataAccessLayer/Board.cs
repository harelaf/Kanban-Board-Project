using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{


    class Board : DalObject<Board>
    {
        public Column BackLog { get; set; }
        public Column InProgress { get; set; }
        public Column Done { get; set; }
        public int idGiver { get; set; }

        public Board()
        {
            BackLog = new Column();
            InProgress = new Column();
            Done = new Column();
            idGiver = 0;
        }

        public Board(Column BackLog,Column InProgress,Column Done, int idGiver)
        {
            this.BackLog = BackLog;
            this.InProgress = InProgress;
            this.Done = Done;
            this.idGiver = idGiver;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override Board Import()
        {
            throw new NotImplementedException();
        }
    }
}
