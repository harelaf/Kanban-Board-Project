using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddColumnViewModel : NotifiableObject
    {

        public KanbanViewModel Kvm { get; private set; }
        public AddColumnViewModel()
        {
            Kvm = new KanbanViewModel();
        }

        public AddColumnViewModel(KanbanViewModel kvm)
        {

            this.Kvm = kvm;
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

        public void AddColumn()
        {
            ErrorMessage2 = "";
            try
            {
                Kvm.ColumnList = Kvm.AddColumn(index, name);
            }
            catch (Exception e)
            {
                ErrorMessage2 = e.Message;
            }
        }
    }

}

