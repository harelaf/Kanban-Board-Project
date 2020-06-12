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

        public void printColumn()
        {
            if (ColumnSelectedItem != null)
                Console.WriteLine("column name "+columnSelectedItem.Name);
        }

        public void printTask()
        {
            if(taskSelectedItem!=null)
             Console.WriteLine("Task's column name:" + taskSelectedItem.ColumnName+" Task description "+taskSelectedItem.Description);
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

        private string errorLabel1;
        public string ErrorLabel1
        {
            get => errorLabel1;

            set
            {
                errorLabel1 = value;
                RaisePropertyChanged("ErrorLabel1");
            }
        }

        public void AdvanceTask()
        {
            int columnId = -1;
            if (taskSelectedItem == null)
            {
                ErrorLabel1 = "no task was chosen to advanced";
                return;
            }
    
            for (int i = 0; i < columnList.Count; i++)
            {
                if (ColumnList[i]!=null&&columnList[i].Name.Equals(taskSelectedItem.ColumnName))
                {
                    columnId = i;
                    break;
                }
            }
            try
            {
                Controller.AdvanceTask(Controller.Email, columnId, TaskSelectedItem.Id);
            }catch(Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void DeleteTask()
        {
            int columnOrdinal = -1;
            for(int i = 0; i < ColumnList.Count; i++)
            {
                if (columnList[i]!=null&&ColumnList[i].Name.Equals(Controller.Email))
                    columnOrdinal = i;
            }

            if (TaskSelectedItem != null)
            {
                try
                {
                    Controller.DeleteTask(Controller.Email, columnOrdinal, TaskSelectedItem.Id);
                }catch(Exception e)
                {
                    ErrorLabel1 = e.Message;
                }
            }
            else
            {
                ErrorLabel1 = "no task was chosen";
            }
            
        }

        public void MoveColumnRight()
        {
            int id = -1;
            for(int i = 0; i < ColumnList.Count; i++)
            {
                if (ColumnList[i] != null && columnSelectedItem.Name.Equals(ColumnList[i].Name))
                    id = i;
            }
            try
            {
                Controller.MoveColumnRight(Controller.Email, id);
            }catch(Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void MoveColumnLeft()
        {
            int id = -1;
            for (int i = 0; i < ColumnList.Count; i++)
            {
                if (ColumnList[i] != null && columnSelectedItem.Name.Equals(ColumnList[i].Name))
                    id = i;
            }
            try
            {
                Controller.MoveColumnLeft(Controller.Email, id);
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }


    }
}
