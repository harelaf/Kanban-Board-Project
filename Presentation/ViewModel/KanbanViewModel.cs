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

        private int FindSelectedColumn()
        {
            int columnOrdinal = -1;

            if (ColumnSelectedItem != null) {
                for (int i = 0; i < ColumnList.Count; i++)
                {
                    if (columnList[i] != null && ColumnList[i].Name.Equals(ColumnSelectedItem.Name))
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
            int columnId = -1;
            if (taskSelectedItem == null)
            {
                ErrorLabel1 = "no task was chosen to advanced";
                return;
            }

            for (int i = 0; i < columnList.Count; i++)
            {
                if (ColumnList[i] != null && columnList[i].Name.Equals(taskSelectedItem.ColumnName))
                {
                    columnId = i;
                    break;
                }
            }
            try
            {
                TaskModel currTask = taskSelectedItem;
                Controller.AdvanceTask(Controller.Email, columnId, currTask.Id);
                ColumnList[columnId + 1].TaskList.Add(currTask);
                ColumnList[columnId].TaskList.Remove(currTask);
                currTask.ColumnName = ColumnList[columnId + 1].Name;
                ErrorLabel1 = "The task has advanced successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void DeleteTask()
        {
            int columnOrdinal = -1;
            for (int i = 0; i < ColumnList.Count; i++)
            {
                if (columnList[i] != null && TaskSelectedItem!=null && ColumnList[i].Name.Equals(taskSelectedItem.ColumnName))
                    columnOrdinal = i;
            }

            if (TaskSelectedItem != null)
            {
                try
                {
                    Controller.DeleteTask(Controller.Email, columnOrdinal, TaskSelectedItem.Id);
                    columnList[columnOrdinal].TaskList.Remove(taskSelectedItem);
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
            if (columnSelectedItem == null)
            {
                ErrorLabel1 = "No column was chosen";
                return;
            }

            int id = FindSelectedColumn();

            if (id == ColumnList.Count - 1)
            {
                ErrorLabel1 = "can't move column right";
                return;
            }
            try
            {
                ColumnMethods columnMethod = new ColumnMethods(Controller);
                ColumnList = columnMethod.MoveColumnRight(id);
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void MoveColumnLeft()
        {
            if (columnSelectedItem == null)
            {
                ErrorLabel1 = "No column was chosen";
                return;
            }

            int id = FindSelectedColumn();

            if (id == 0)
            {
                ErrorLabel1 = "can't move column left";
                return;
            }
            
            try
            {
                ColumnMethods columnMethod = new ColumnMethods(Controller);
                ColumnList = columnMethod.MoveColumnLeft(id);
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        public void RemoveColumn()
        {
          
            int i = FindSelectedColumn();
            if (i == -2)
            {
                ErrorLabel1 = "no column was selected";
                return;
            }

            try
            {
                ColumnMethods columnMethod = new ColumnMethods(Controller);
                ColumnList = columnMethod.RemoveColumn(i);
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }


        public ObservableCollection<ColumnModel> AddColumn(int index, string name)
        {
            ColumnMethods columnMethod = new ColumnMethods(Controller);
            return columnMethod.AddColumn(index, name);
        }

        private void SortColumns()
        {

        }

        public void SortByDueDate()
        {
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
    }
}
