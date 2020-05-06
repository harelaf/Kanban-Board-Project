using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{


    class Board : DalObject<Board>
    {
        public List<Column> columnList { get; set; }
        public int idGiver { get; set; }

        public Board()
        {
            columnList = new List<Column>();
            idGiver = 0;
        }

        public Board(List<Column> columnList, int idGiver)
        {
            this.columnList = columnList;
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
