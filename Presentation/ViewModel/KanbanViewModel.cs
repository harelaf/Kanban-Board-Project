using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;
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

        private string searchValue;
        public string SearchValue
        {
            get
            {
                return searchValue;
            }
            set
            {
                searchValue = value;
                RaisePropertyChanged("SearchValue");
            }
        }

        private int FindColumn(string myColumnName)
        {
            SearchValue = null;
            filterByString();
            int columnOrdinal = -1;

            if (myColumnName != null) {
                for (int i = 0; i < ColumnList.Count; i++)
                {
                    if (columnList[i] != null && ColumnList[i].Name.Equals(myColumnName))
                    {
                        columnOrdinal = i;
                        break;
                    }
                }
            }
            return columnOrdinal;
        }

        public void AdvanceTask()
        {
            TaskModel myTask = TaskSelectedItem;
            SearchValue = null;
            filterByString();
            if (myTask == null)
            {
                ErrorLabel1 = "no task was chosen to advanced";
                return;
            }

            int columnId = FindColumn(myTask.ColumnName);

            try
            {
                Controller.AdvanceTask(Controller.Email, columnId, myTask.Id);
                
                ColumnList[columnId + 1].TaskList.Add(myTask);
                ColumnList[columnId].TaskList.Remove(myTask);
                myTask.ColumnName = ColumnList[columnId + 1].Name;
                ErrorLabel1 = "The task has advanced successfully";
                TaskSelectedItem = myTask;
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void DeleteTask()
        {
            TaskModel myTask = taskSelectedItem;
            SearchValue = null;
            filterByString();
            int columnOrdinal = -1;
            for (int i = 0; i < ColumnList.Count; i++)
            {
                if (columnList[i] != null && myTask!=null && ColumnList[i].Name.Equals(myTask.ColumnName))
                    columnOrdinal = i;
            }

            if (myTask != null)
            {
                try
                {
                    Controller.DeleteTask(Controller.Email, columnOrdinal, myTask.Id);
                    columnList[columnOrdinal].TaskList.Remove(myTask);
                    ErrorLabel1 = "The task was deleted successfully";
                }
                catch (Exception e)
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
            ColumnModel myColumn = ColumnSelectedItem;
            SearchValue = null;
            filterByString();
            if (myColumn == null)
            {
                ErrorLabel1 = "No column was chosen";
                return;
            }

            int id = columnList.IndexOf(myColumn);

            try
            {
                Controller.MoveColumnRight(Controller.Email, id);
                ColumnList = Controller.GetBoard(Controller.Email).ColList;
                ErrorLabel1 = "The column has been moved successfully";
                ColumnSelectedItem = columnList[id + 1];
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void MoveColumnLeft()
        {
            ColumnModel myColumn = ColumnSelectedItem;
            SearchValue = null;
            filterByString();
            if (myColumn == null)
            {
                ErrorLabel1 = "No column was chosen";
                return;
            }

            int id = columnList.IndexOf(myColumn);

            try
            {
                Controller.MoveColumnLeft(Controller.Email, id);
                ColumnList = Controller.GetBoard(Controller.Email).ColList;
                ErrorLabel1 = "The column has been moved successfully";
                ColumnSelectedItem = columnList[id - 1];
            }
            catch (Exception e)
            {
                ColumnSelectedItem = myColumn;
                ErrorLabel1 = e.Message;
            }
        }

        public void RemoveColumn()
        {
            ColumnModel myColumn = ColumnSelectedItem;
            SearchValue = null;
            filterByString();
            int id = columnList.IndexOf(myColumn);
            if (id == -1)
            {
                ErrorLabel1 = "no column was selected";
                return;
            }

            try
            {
                Controller.RemoveColumn(Controller.Email, id);
                ColumnList = Controller.GetBoard(Controller.Email).ColList;
                ErrorLabel1 = "The column has been removed successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        private string columnName;
        public string ColumnName
        {
            get => columnName;
            set
            {
                columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }

        public void ChangeColumnName(string name)
        {
            ColumnModel myColumn = ColumnSelectedItem;
            SearchValue = null;
            filterByString();
            int id = columnList.IndexOf(myColumn);
            if (id == -1)
            {
                ErrorLabel1 = "no column was selected";
                return;
            }

            try
            {
                Controller.ChangeColumnName(Controller.Email, id, name);
                ColumnList = Controller.GetBoard(Controller.Email).ColList;
                ErrorLabel1 = "Column's has been changed succsssfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void SortByDueDate()
        {
            SearchValue = null;
            filterByString();
            ObservableCollection<ColumnModel> newColumnList = new ObservableCollection<ColumnModel>();
            foreach(ColumnModel cm in columnList)
            {
                newColumnList.Add(new ColumnModel(Controller, new ObservableCollection<TaskModel>(cm.TaskList.OrderBy(x => x.DueDate).ToList()), cm.Name));
            }
            ColumnList = newColumnList;
        }

        public void logout()
        {
            try
            {
                Controller.Logout(Controller.Email);
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void filterByString()
        {
            ObservableCollection<ColumnModel> newColumnList = new ObservableCollection<ColumnModel>();
            foreach(ColumnModel cm in columnList)
            {
                newColumnList.Add(cm.filter(SearchValue));
            }
            ColumnList = newColumnList;
	    }

        public bool DeleteData()
        {
            SearchValue = null;
            filterByString();
            DialogResult d = MessageBox.Show("ARE YOU SURE?", "Confirmation", MessageBoxButtons.YesNo);
            if(d == DialogResult.Yes)
            {
                try
                {
                    Controller.DeleteData();
                    return true;
                }
                catch (Exception e)
                {
                    ErrorLabel1 = e.Message;
                    return false;
                }
            }
            return false;
        }
    }
}
