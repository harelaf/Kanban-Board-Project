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
        
        /// <summary>
        /// always get an existing controller
        /// </summary>
        /// <param name="Controller"></param>
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
            }
        }

        /// <summary>
        /// the index for insertion
        /// </summary>
        private int index;
        public int Index
        {
            get => index;
            set
            {
                index = value;
            }
        }

        /// <summary>
        /// displays all kind if messages to the user
        /// </summary>
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

        /// <summary>
        /// adds a new column to the recieved column list with the fields above^ and displays a proper message
        /// </summary>
        /// <param name="columns"></param>
        public void AddColumn(ObservableCollection<ColumnModel> columns)
        {
            ErrorMessage2 = "";
            try
            {
                ColumnModel col = Controller.AddColumn(Controller.Email, Index, Name);
                columns.Insert(Index, new ColumnModel(Controller, new ObservableCollection<TaskModel>(), Name, col.Limit));
                ErrorMessage2 = "The column has been added successfully";
            }
            catch (Exception e)
            {
                ErrorMessage2 = e.Message;
            }
        }
    }

}

