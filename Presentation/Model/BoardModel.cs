using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class BoardModel
    {
        private List<ColumnModel> colList;
        private string CreatorEmail;

        public BoardModel(string CreatorEmail, List<ColumnModel> colList)
        {
            this.colList = colList;
            this.CreatorEmail = CreatorEmail;
        }


    }
}
