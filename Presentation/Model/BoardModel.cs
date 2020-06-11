using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class BoardModel : NotifiableModelObject
    {
        private ObservableCollection<ColumnModel> colList;
        public ObservableCollection<ColumnModel> ColList
        {
            get => colList;
            set
            {
                colList = value;
            }
        }
        public readonly string CreatorEmail;

        public BoardModel(BackendController Controller, string CreatorEmail, ObservableCollection<ColumnModel> colList) : base(Controller)
        {
            this.colList = colList;
            this.CreatorEmail = CreatorEmail;
        }
    }
}
