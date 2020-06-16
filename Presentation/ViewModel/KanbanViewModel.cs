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
            myHeight = 600;
        }

        private int myheight;
        public int myHeight
        {
            get => myheight;
            set
            {
                if(value > 550)
                {
                    myheight = value;
                    RaisePropertyChanged("myHeight");
                    myHeight2 = myheight - 120;
                }
                else
                {
                    myheight = 550;
                    RaisePropertyChanged("myHeight");
                    myHeight2 = myheight - 120;
                }
            }
        }

        private int myheight2;
        public int myHeight2
        {
            get => myheight2;
            set
            {
                myheight2 = value;
                RaisePropertyChanged("myHeight2");
            }
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
                if (value != null)
                {
                    taskSelectedItem = new TaskModel(Controller, value.Id, value.CreationDate, value.DueDate, value.Title, value.Description, value.EmailAssignee, value.ColumnName);
                    LastSelected = taskSelectedItem;
                    UpdateTitle = taskSelectedItem.Title;
                    UpdateDescription = taskSelectedItem.Description;
                    UpdateAssignee = taskSelectedItem.EmailAssignee;
                }
                else
                    taskSelectedItem = null;
                RaisePropertyChanged("TaskSelectedItem");
                Enabled = true;
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

        private bool enabled;
        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                RaisePropertyChanged("Enabled");
            }
        }

        private string updateTitle;
        public string UpdateTitle
        {
            get => updateTitle;
            set
            {
                updateTitle = value;
                RaisePropertyChanged("UpdateTitle");
            }
        }

        private string updateDescription;
        public string UpdateDescription
        {
            get => updateDescription;
            set
            {
                updateDescription = value;
                RaisePropertyChanged("UpdateDescription");
            }
        }

        private string updateAssignee;
        public string UpdateAssignee
        {
            get => updateAssignee;
            set
            {
                updateAssignee = value;
                RaisePropertyChanged("UpdateAssignee");
            }
        }

        private TaskModel lastSelected;
        public TaskModel LastSelected
        {
            get => lastSelected;
            set
            {
                lastSelected = value;
            }
        }

        public void UpdateTaskTitle()
        {
            int ColId = FindColumn(LastSelected.ColumnName);
            try
            {
                Controller.UpdateTaskTitle(Controller.Email, ColId, LastSelected.Id, UpdateTitle);
                ColumnList[ColId].TaskList.Where(x => x.Id == LastSelected.Id).ToList()[0].Title = UpdateTitle;
                RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Updated task's title successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void UpdateTaskDescription()
        {
            int ColId = FindColumn(LastSelected.ColumnName);
            try
            {
                Controller.UpdateTaskDescription(Controller.Email, ColId, LastSelected.Id, UpdateDescription);
                ColumnList[ColId].TaskList.Where(x => x.Id == LastSelected.Id).ToList()[0].Description = UpdateDescription;
                RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Updated task's description successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void AssignTask()
        {
            int ColId = FindColumn(LastSelected.ColumnName);
            try
            {
                Controller.AssignTask(Controller.Email, ColId, LastSelected.Id, UpdateAssignee);
                ColumnList[ColId].TaskList.Where(x => x.Id == LastSelected.Id).ToList()[0].EmailAssignee = UpdateAssignee;
                RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Assigned task successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
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
                ColumnList[columnId].TaskList.Remove(ColumnList[columnId].TaskList.Where(x => x.Id == myTask.Id).ToList()[0]);
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
            
            columnOrdinal = FindColumn(myTask.ColumnName);

            if (myTask != null)
            {
                try
                {
                    Controller.DeleteTask(Controller.Email, columnOrdinal, myTask.Id);
                    columnList[columnOrdinal].TaskList.Remove(ColumnList[columnOrdinal].TaskList.Where(x => x.Id == myTask.Id).ToList()[0]);
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
