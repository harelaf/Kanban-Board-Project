using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class BoardModel
    {
        public readonly List<ColumnModel> colList;
        public readonly string CreatorEmail;

        public BoardModel(string CreatorEmail, List<ColumnModel> colList)
        {
            this.colList = colList;
            this.CreatorEmail = CreatorEmail;
        }


    }
}
