using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for KanbanWindow.xaml
    /// </summary>
    public partial class KanbanWindow : Window
    {
        private KanbanViewModel KVModel;

        internal KanbanWindow(BackendController Controller)
        {
            InitializeComponent();
            KVModel = new KanbanViewModel(Controller);
            this.DataContext = KVModel;
        }

        /// <summary>
        /// event that triggers when the "advance task" button is clicked.
        /// advances selected task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvanceTask_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.AdvanceTask();
	    }

        /// <summary>
        /// event that triggers when the "add task" button is clicked.
        /// boots up a new add task window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTask_Button_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow adw = new AddTaskWindow(KVModel.Controller, KVModel.ColumnList);
            adw.Show();
	    }

        /// <summary>
        /// event that triggers when the "delete task" button is clicked.
        /// deletes selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTask_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.DeleteTask();
        }

        /// <summary>
        /// event that triggers when the "move right" button is clicked
        /// move selected column right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveColumnRight_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.MoveColumnRight();
        }

        /// <summary>
        /// event that triggers when the "move left" button is clicked
        /// move selected column left
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveColumnLeft_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.MoveColumnLeft();
        }

        /// <summary>
        /// event that triggers when the "delete all boards" button is clicked
        /// deletes all the kanban data from the dataBase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteData_Button_Click(object sender, RoutedEventArgs e)
        {
            if (KVModel.DeleteData())
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        /// <summary>
        /// event that triggers when the "remove column" button is clicked
        /// removes selected column and takes care for its tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveColumn_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.RemoveColumn();
	    }
        private void AddColumn_Button_Click(object sender, RoutedEventArgs e)
        {
            AddColumn addColumn = new AddColumn(KVModel.Controller, KVModel.ColumnList);
            addColumn.Show();
        }

        /// <summary>
        /// event that triggers when the "LogOut" button is clicked
        /// checks out of the board and boots up the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.logout();
            MainWindow mw = new MainWindow(KVModel.Controller);
            mw.Show();
            this.Close();
        }

        /// <summary>
        /// event that triggers when the "sort by dueDate" button is clicked
        /// sort the entire board by the tasks' dueDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.SortByDueDate();
        }

        /// <summary>
        /// event that triggers when the "?" button is clicked
        /// filters the entire board by the search line's content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.filterByString();
        }

        /// <summary>
        /// event that triggers when the "instruction" button is clicked
        /// shows a msgbox that explains about the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Instructions_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Instructions for using the board:\n\n1. In order to add a task/column, click the appropriate button and fill in the fields, lastly click on the add task/column button.\n\n2. When a field requires a date enter the date using a syntax that represents a date, such as: MM/DD/YYYY or MM/DD/YYYY hh:mm:ss AM/PM (for more accuracy).\n\n3. In order to move/delete a task/column, first select one from the list and then click the appropriate button.\n\n4. In order to update a tasks field: first, select the desired task, then, on the left hand side of the board, under \"Task Display\" change the field you want and click the appropriate button to update the field.\n\n5. In order to filter out tasks, enter to the text box near the top right the desired key words, and then click the question mark.\n\nNote: If some buttons are disabled it means you haven't selected a task/column yet.", "Instructions");
        }

        /// <summary>
        /// event that triggers when the "update title" button is clicked
        /// update title for the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTitle_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.UpdateTaskTitle();
        }

        /// <summary>
        /// event that triggers when the "update description" button is clicked
        /// update description for the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDescription_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.UpdateTaskDescription();
        }
        /// <summary>
        /// event that triggers when the "update assignee" button is clicked
        /// update the assigned user for the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateAssignee_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.AssignTask();
        }

        /// <summary>
        /// event that triggers when the "update dueDate" button is clicked
        /// update dueDate for the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDueDate_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.UpdateTaskDueDate();
        }

        /// <summary>
        /// event that triggers when the "change name" button is clicked
        /// update name for the selected column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeColumnName_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.ChangeColumnName();
        }

        /// <summary>
        /// This is an event handler that triggers when a user selects a task
        /// In order to avoide "collisions" between selected tasks in different columns, we unselect all tasks and make the new selected task the last select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedItems.Count > 0)
            {
                ((ListBox)sender).UnselectAll();
                KVModel.TaskSelectedItem = KVModel.LastSelected;
            }
        }
    }

}
