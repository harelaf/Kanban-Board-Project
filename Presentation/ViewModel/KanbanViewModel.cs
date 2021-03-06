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

        /// <summary>
        /// always gets an existing controller
        /// </summary>
        /// <param name="Controller"></param>
        public KanbanViewModel(BackendController Controller, BoardModel Board)
        {
            this.Controller = Controller;
            this.Board = Board;
            ColumnList = Board.ColList;
            ErrorLabel1 = "Hello there, " + Board.currUser.Nickname +"!";
        }

        private BoardModel board;
        public BoardModel Board
        {
            get => board;
            set
            {
                board = value;
            }
        }

        /// <summary>
        /// the main list to display. each one contains a task list
        /// </summary>
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

        /// <summary>
        /// the current item to operate on
        /// </summary>
        private TaskModel taskSelectedItem;
        public TaskModel TaskSelectedItem
        {
            get => taskSelectedItem;
            set
            {
                if (value != null)
                {
                    if(taskSelectedItem!=null)
                        taskSelectedItem.TitleFontSize = 14;
                    taskSelectedItem = value;
                    lastSelected = taskSelectedItem;
                    taskSelectedItem.TitleFontSize = 18;
                    UpdateTitle = taskSelectedItem.Title;
                    UpdateDescription = taskSelectedItem.Description;
                    UpdateAssignee = taskSelectedItem.EmailAssignee;
                    UpdateDueDate = taskSelectedItem.DueDate.ToString();
                    Enabled = true;
                }
                else
                {
                    if (taskSelectedItem != null)
                        taskSelectedItem.TitleFontSize = 14;
                    taskSelectedItem = null;
                    Enabled = false;
                }
            }
        }

        /// <summary>
        /// the current column to operate on
        /// </summary>
        private ColumnModel columnSelectedItem;
        public ColumnModel ColumnSelectedItem
        {
            get => columnSelectedItem;
            set
            {
                if (value != null)
                {
                    columnSelectedItem = value;
                    ColumnName = columnSelectedItem.Name;
                    ColumnLimit = columnSelectedItem.Limit;
                    EnabledColumn = true;
                }
                else
                {
                    columnSelectedItem = null;
                    EnabledColumn = false;
                }
                RaisePropertyChanged("ColumnSelectedItem");
            }
        }

        /// <summary>
        /// a label to display messages to the loggedIn user
        /// </summary>
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

        /// <summary>
        /// holds a string to filter by
        /// </summary>
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

        /// <summary>
        /// returns column index 
        /// </summary>
        /// <param name="myColumnName">the column name to search by</param>
        /// <returns></returns>
        private int FindColumn(string myColumnName)
        {
            int columnOrdinal = -1;

            if (myColumnName != null)
            {
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

        /// <summary>
        /// determine weather the task-change-buttons will be enabled
        /// </summary>
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

        /// <summary>
        /// determine weather the column-change-buttons will be enabled
        /// </summary>
        private bool enabledColumn;
        public bool EnabledColumn
        {
            get => enabledColumn;
            set
            {
                enabledColumn = value;
                RaisePropertyChanged("EnabledColumn");
            }
        }

        /// <summary>
        /// holds the title of the current task
        /// </summary>
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

        /// <summary>
        /// holds the description of the current task
        /// </summary>
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

        /// <summary>
        /// holds the dueDate of the current task
        /// </summary>
        private string updateDueDate;
        public string UpdateDueDate
        {
            get => updateDueDate;
            set
            {
                updateDueDate = value;
                RaisePropertyChanged("UpdateDueDate");
            }
        }

        /// <summary>
        /// holds the Assigned user's email of the current task
        /// </summary>
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

        /// <summary>
        /// holds the name of the current column
        /// </summary>
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

        /// <summary>
        /// holds the current task (since it changes on button click)
        /// </summary>
        private TaskModel lastSelected;
        public TaskModel LastSelected
        {
            get => lastSelected;
            set
            {
                lastSelected = value;
            }
        }

        private int columnLimit;
        public int ColumnLimit
        {
            get => columnLimit;
            set
            {
                columnLimit = value;
                RaisePropertyChanged("ColumnLimit");
            }
        }

        public void ChangeColumnLimit()
        {
            int ColId = FindColumn(ColumnSelectedItem.Name);
            try
            {
                Controller.LimitColumnTasks(Controller.Email, ColId, ColumnLimit);
                ColumnList[ColId].Limit = ColumnLimit;
                //RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Changed column's limit successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// Updates current task's title and displays a proper message
        /// </summary>
        public void UpdateTaskTitle()
        {
            int ColId = FindColumn(TaskSelectedItem.ColumnName);
            try
            {
                Controller.UpdateTaskTitle(Controller.Email, ColId, TaskSelectedItem.Id, UpdateTitle);
                TaskSelectedItem.Title = UpdateTitle;
                //RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Updated task's title successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// Updates current task's description and displays a proper message
        /// </summary>
        public void UpdateTaskDescription()
        {
            int ColId = FindColumn(TaskSelectedItem.ColumnName);
            try
            {
                Controller.UpdateTaskDescription(Controller.Email, ColId, TaskSelectedItem.Id, UpdateDescription);
                TaskSelectedItem.Description = UpdateDescription;
                //RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Updated task's description successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// Updates current task's dueDate and displays a proper message
        /// </summary>
        public void UpdateTaskDueDate()
        {
            int ColId = FindColumn(TaskSelectedItem.ColumnName);
            DateTime newDate;
            try
            {
                newDate = DateTime.Parse(UpdateDueDate);
                try
                {
                    Controller.UpdateTaskDueDate(Controller.Email, ColId, TaskSelectedItem.Id, newDate);
                    TaskSelectedItem.DueDate = newDate;
                    //RaisePropertyChanged("ColumnList");
                    ErrorLabel1 = "Updated task's due date successfully";
                }
                catch (Exception e)
                {
                    ErrorLabel1 = e.Message;
                }
            }
            catch
            {
                ErrorLabel1 = "Can't convert the string you have entered to a date";
            }
        }

        /// <summary>
        /// Updates current column's name and displays a proper message
        /// </summary>
        public void ChangeColumnName()
        {
            int ColId = FindColumn(ColumnSelectedItem.Name);
            try
            {
                Controller.ChangeColumnName(Controller.Email, ColId, ColumnName);
                ColumnList[ColId].Name = ColumnName;
                //RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Changed column's name successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// Updates current task's assigned user and displays a proper message
        /// </summary>
        public void AssignTask()
        {
            int ColId = FindColumn(TaskSelectedItem.ColumnName);
            try
            {
                Controller.AssignTask(Controller.Email, ColId, TaskSelectedItem.Id, UpdateAssignee);
                TaskSelectedItem.EmailAssignee = UpdateAssignee;
                RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "Assigned task successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// advances current task and displays a proper message
        /// </summary>
        public void AdvanceTask()
        {
            TaskModel myTask = TaskSelectedItem;
            
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
                ColumnList[columnId].TaskList.Remove(TaskSelectedItem);
                myTask.ColumnName = ColumnList[columnId + 1].Name;
                filterByString();
                RaisePropertyChanged("ColumnList");
                ErrorLabel1 = "The task has advanced successfully";
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// deletes current task and displays a proper message
        /// </summary>
        public void DeleteTask()
        {
            TaskModel myTask = TaskSelectedItem;
            int columnOrdinal = -1;

            columnOrdinal = FindColumn(myTask.ColumnName);

            if (myTask != null)
            {
                try
                {
                    Controller.DeleteTask(Controller.Email, columnOrdinal, myTask.Id);
                    columnList[columnOrdinal].TaskList.Remove(TaskSelectedItem);
                    filterByString();
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

        public AddColumnViewModel AddColumn()
        {
            return new AddColumnViewModel(Controller, ColumnList);
        }

        public AddTaskViewModel AddTask()
        {
            return new AddTaskViewModel(Controller, ColumnList[0]);
        }

        /// <summary>
        /// moves curent column to the right and displays a proper message
        /// </summary>
        public void MoveColumnRight()
        {
            ColumnModel myColumn = ColumnSelectedItem;
            if (myColumn == null)
            {
                ErrorLabel1 = "No column was chosen";
                return;
            }

            int id = columnList.IndexOf(myColumn);

            try
            {
                Controller.MoveColumnRight(Controller.Email, id);
                ColumnList = Controller.GetBoard(Controller.Email, board.currUser).ColList;
                filterByString();
                ErrorLabel1 = "The column has been moved successfully";
                ColumnSelectedItem = columnList[id + 1];
                TaskSelectedItem = null;
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// moves current column to the left and displays a proper message
        /// </summary>
        public void MoveColumnLeft()
        {
            ColumnModel myColumn = ColumnSelectedItem;
            if (myColumn == null)
            {
                ErrorLabel1 = "No column was chosen";
                return;
            }

            int id = columnList.IndexOf(myColumn);

            try
            {
                Controller.MoveColumnLeft(Controller.Email, id);
                ColumnList = Controller.GetBoard(Controller.Email, board.currUser).ColList;
                filterByString();
                ErrorLabel1 = "The column has been moved successfully";
                ColumnSelectedItem = columnList[id - 1];
                TaskSelectedItem = null;
            }
            catch (Exception e)
            {
                ColumnSelectedItem = myColumn;
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// delete current column from the board (takes care of its tasks) and display a proper messsage
        /// </summary>
        public void RemoveColumn()
        {
            ColumnModel myColumn = ColumnSelectedItem;
            int id = columnList.IndexOf(myColumn);
            if (id == -1)
            {
                ErrorLabel1 = "no column was selected";
                return;
            }

            try
            {
                Controller.RemoveColumn(Controller.Email, id);
                ColumnList = Controller.GetBoard(Controller.Email, board.currUser).ColList;
                filterByString();
                ErrorLabel1 = "The column has been removed successfully";
                ColumnSelectedItem = ColumnList[id > 0 ? id - 1 : id];
                TaskSelectedItem = null;
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
            }
        }

        /// <summary>
        /// sort the entire board by the task's dueDate
        /// </summary>
        public void SortByDueDate()
        {
            ObservableCollection<ColumnModel> newColumnList = new ObservableCollection<ColumnModel>();
            foreach (ColumnModel cm in columnList)
            {
                newColumnList.Add(new ColumnModel(Controller, new ObservableCollection<TaskModel>(cm.TaskList.OrderBy(x => x.DueDate).ToList()), cm.Name, cm.Limit));
            }
            ColumnList = newColumnList;
            filterByString();
            ErrorLabel1 = "the board has been sorted successfully";
        }

        /// <summary>
        /// user check out and displays a proper message
        /// </summary>
        public MainWindowViewModel logout()
        {
            try
            {
                Controller.Logout(Controller.Email);
                return new MainWindowViewModel(Controller);
            }
            catch (Exception e)
            {
                ErrorLabel1 = e.Message;
                return null;
            }
        }

        /// <summary>
        /// filter the entire board by the "searchValue"
        /// </summary>
        public void filterByString()
        {
            ObservableCollection<ColumnModel> newColumnList = new ObservableCollection<ColumnModel>();
            foreach (ColumnModel cm in columnList)
            {
                newColumnList.Add(cm.filter(SearchValue));
            }
            ColumnList = newColumnList;
        }

        /// <summary>
        /// deletes dataBase and logout of the board
        /// </summary>
        /// <returns></returns>
        public bool DeleteData()
        {
            DialogResult d = MessageBox.Show("ARE YOU SURE?", "Confirmation", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
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
