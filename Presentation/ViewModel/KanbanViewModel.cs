using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class KanbanViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        public KanbanViewModel()
        {
            this.Controller = new BackendController();
            Board = null;
            ColumnList = null;
        }

        public KanbanViewModel(BackendController Controller)
        {
            this.Controller = Controller;
            Board = Controller.GetBoard(Controller.Email);
            ColumnList = Board.ColList;
        }

        private BoardModel board;
        public BoardModel Board
        {
            get => board;
            set
            {
                board = value;
                RaisePropertyChanged("Board");
            }
        }

        private ObservableCollection<ColumnModel> columnList;
        public ObservableCollection<ColumnModel> ColumnList
        {
            get => columnList;
            set
            {
                columnList = value;
                RaisePropertyChanged("ColumnList");
            }
        }

        private TaskModel taskSelectedItem;
        public TaskModel TaskSelectedItem
        {
            get => taskSelectedItem;
            set
            {
                taskSelectedItem = value;
                RaisePropertyChanged("TaskSelectedItem");
            }
        }

        private ColumnModel columnSelectedItem;
        public ColumnModel ColumnSelectedItem
        {
            get => columnSelectedItem;
            set
            {
                columnSelectedItem = value;
                RaisePropertyChanged("ColumnSelectedItem");
            }
        }

        private string errorLabel;
        public string ErrorLabel
        {
            get => errorLabel;

            set
            {
                errorLabel = value;
                RaisePropertyChanged("ErrorLabel");
            }
        }

        public void AdvanceTask()
        {
            int columnId = -1;
            for(int i = 0; i < columnList.Count; i++)
            {
                if (columnList[i].Name.Equals(taskSelectedItem.ColumnName))
                {
                    columnId = i;
                    break;
                }
            }
            if (columnId == -1)
                throw new Exception("couldn't find the task's next column");
            try
            {
                Controller.AdvanceTask(Controller.Email, columnId, TaskSelectedItem.Id);
            }catch(Exception e)
            {
                ErrorLabel = e.Message;
            }
        }
        
    }
}
