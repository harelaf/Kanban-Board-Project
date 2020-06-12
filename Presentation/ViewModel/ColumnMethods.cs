using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class ColumnMethods : NotifiableObject
    {
        public BackendController Controller { get; set; }

        public ColumnMethods()
        {
            Controller = new BackendController();
        }

        public ColumnMethods(BackendController Controller)
        {
            this.Controller = Controller;
        }

        public ObservableCollection<ColumnModel> MoveColumnRight(int id)
        {
            Controller.MoveColumnRight(Controller.Email, id);
            return Controller.GetBoard(Controller.Email).ColList;
        }

        public ObservableCollection<ColumnModel> MoveColumnLeft(int id)
        {
             Controller.MoveColumnLeft(Controller.Email, id);
             return Controller.GetBoard(Controller.Email).ColList;
        }

        public ObservableCollection<ColumnModel> RemoveColumn(int id)
        {
            Controller.RemoveColumn(Controller.Email, id);
            return Controller.GetBoard(Controller.Email).ColList;
        }

        public ObservableCollection<ColumnModel> AddColumn(int id,string name)
        {
            Controller.AddColumn(Controller.Email, id, name);
            return Controller.GetBoard(Controller.Email).ColList;
        }


    }
}
