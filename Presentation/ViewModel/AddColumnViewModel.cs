using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddColumnViewModel : NotifiableObject
    {

        public BackendController Controller { get; private set; }
        public AddColumnViewModel()
        {
            Controller = new BackendController();
        }

        public AddColumnViewModel(BackendController Controller)
        {

            this.Controller = Controller;
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private int index;
        public int Index
        {
            get => index;
            set
            {
                index = value;
                RaisePropertyChanged("Index");
            }
        }

        private string errorMessage2;
        public string ErrorMessage2
        {
            get => errorMessage2;
            set
            {
                errorMessage2 = value;
                RaisePropertyChanged("ErrorMessage2");
            }
        }

        public void AddColumn(ObservableCollection<ColumnModel> columns)
        {
            ErrorMessage2 = "";
            try
            {
                Controller.AddColumn(Controller.Email, Index, Name);
                columns.Insert(Index, new ColumnModel(Controller, new ObservableCollection<TaskModel>(), Name));
                ErrorMessage2 = "The column has been added successfully";
            }
            catch (Exception e)
            {
                ErrorMessage2 = e.Message;
            }
        }
    }

}

